using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PhaserArray.PaycheckPlugin.Serialization;
using PhaserArray.PaycheckPlugin.Helpers;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PhaserArray.PaycheckPlugin
{
    public class PaycheckPlugin: RocketPlugin<PaycheckPluginConfiguration>
    {
	    public static PaycheckPlugin Instance;
	    public static PaycheckPluginConfiguration Config;

	    protected override void Load()
	    {
		    Instance = this;
		    Config = Configuration.Instance;
			InvokeRepeating(nameof(GiveAllPaychecks), Config.Interval, Config.Interval);
	    }

	    protected override void Unload()
	    {
			// TODO: Implement commands to make making paychecks easier so saving this has a point.
			Configuration.Save();
			CancelInvoke(nameof(GiveAllPaychecks));
	    }

		/// <summary>
		/// Gives all players paychecks.
		/// </summary>
	    public void GiveAllPaychecks()
	    {
		    if (!Provider.isInitialized || !Level.isLoaded) return;
		    foreach (var client in Provider.clients)
		    {
			    GivePaycheck(UnturnedPlayer.FromSteamPlayer(client));
		    }
	    }

		/// <summary>
		/// Gives the player their paycheck(s) w/ multipliers applied, also send paycheck notifications.
		/// </summary>
		/// <param name="player"></param>
	    public void GivePaycheck(UnturnedPlayer player)
	    {
		    var paychecks = GetAvailablePaychecks(player);
		    var experience = GetPaycheckExperience(player, paychecks);
		    var multiplier = GetPaycheckMultiplier(player.Position, paychecks);

			// TODO: Add check for AllowPaychecksWhenDead
			if (Mathf.Abs(multiplier) > 0.0001f)
			{
			    var experienceGiven = ExperienceHelper.ChangeExperience(player, (int)(experience * multiplier));
			    if (experienceGiven != 0 && Config.DisplayNotification)
			    {
				    UnturnedChat.Say(player, Translate("paycheck_given", experienceGiven));
			    }
		    }
		    else
		    {
			    if (Config.DisplayNotification)
				{
					UnturnedChat.Say(player, Translate("paycheck_zero_multiplier"));
				}
		    }
	    }

		/// <summary>
		/// Gets the experience sum for all provided paychecks.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="paychecks"></param>
		/// <returns>Sum</returns>
		public int GetPaycheckExperience(UnturnedPlayer player, List<Paycheck> paychecks)
		{
			return paychecks.Sum(paycheck => paycheck.Experience);
	    }

		/// <summary>
		/// Gets all paychecks that the player has access to.
		/// </summary>
		/// <param name="player"></param>
		/// <returns>List of Paychecks</returns>
	    public List<Paycheck> GetAvailablePaychecks(UnturnedPlayer player)
	    {
			var paychecks = Config.Paychecks.Where(paycheck => 
				PermissionsHelper.HasPermission(player, "paycheck." + paycheck.Name.ToLower())).ToList();

		    if (Config.AllowMultiplePaychecks || paychecks.Count <= 1) return paychecks;

		    var highestPaycheck = paychecks[0];
		    for (var i = 1; i < paychecks.Count; i++)
		    {
			    if (paychecks[i].Experience > highestPaycheck.Experience)
			    {
				    highestPaycheck = paychecks[i];
			    }
		    }
			return new List<Paycheck> {highestPaycheck};
	    }

		/// <summary>
		/// Gets the multiplier for the provided paychecks at the provided location.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="paychecks"></param>
		/// <returns>Paycheck Experience Multiplier</returns>
	    public float GetPaycheckMultiplier(Vector3 position, List<Paycheck> paychecks)
	    {
			Logger.Log("begin");
		    var zones = new List<PaycheckZone>();
			Logger.Log("initzones");
		    zones.AddRange(Config.PaycheckZones);
		    Logger.Log("addeddefaultzones");
		    foreach (var paycheck in paychecks)
		    {
			    zones.AddRange(paycheck.PaycheckZones);
		    }
		    Logger.Log("addedpaycheckspecificzones");

			var multiplier = 1f;
		    var closestDistance = Mathf.Infinity;

		    foreach (var zone in zones)
		    {
			    if (zone.Point != null)
			    {
				    var distance = (position - zone.Point.GetValueOrDefault()).sqrMagnitude;
				    if (!(distance <= Mathf.Pow(zone.Radius, 2f))) continue;

				    if (!Config.OnlyUseClosestZone)
				    {
					    multiplier *= zone.Multiplier;
				    }
				    else if (distance < closestDistance)
				    {
					    closestDistance = distance;
					    multiplier = zone.Multiplier;
				    }
				}
				else if (zone.Node != null)
			    {
				    foreach (var node in LevelNodes.nodes)
				    {
					    if (node.type != ENodeType.LOCATION) continue;
					    if (!((LocationNode) node).name.Contains(zone.Node)) continue;

					    var distance = (position - node.point).sqrMagnitude;
					    if (!(distance <= Mathf.Pow(zone.Radius, 2f))) continue;

						if (!Config.OnlyUseClosestZone)
						{
							multiplier *= zone.Multiplier;
						}
					    else if (distance < closestDistance)
						{
							closestDistance = distance;
							multiplier = zone.Multiplier;
						}
					    break;
				    }
			    }
		    }

		    Logger.Log(multiplier.ToString(CultureInfo.InvariantCulture));

		    return multiplier;
	    }

	    public override TranslationList DefaultTranslations => new TranslationList()
	    {
		    {"paycheck_zero_multiplier", "You cannot earn experience in this area!"},
		    {"paycheck_given", "You have received your paycheck of {0} experience!"}
	    };
    }
}