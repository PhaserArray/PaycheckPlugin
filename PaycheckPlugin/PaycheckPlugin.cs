using System.Collections.Generic;
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
    public class PaycheckPlugin : RocketPlugin<PaycheckPluginConfiguration>
    {
	    public static PaycheckPlugin Instance;
	    public static PaycheckPluginConfiguration Config;
		public const string Version = "v1.0";

	    protected override void Load()
	    {
			Logger.Log($"Loading PhaserArray's Paycheck Plugin {Version}");
		    Instance = this;
		    Config = Configuration.Instance;

			InvokeRepeating(nameof(GiveAllPaychecks), Config.Interval, Config.Interval);
	    }

		protected override void Unload()
		{
			Logger.Log($"Unloading PhaserArray's Paycheck Plugin {Version}");
			if (Config.IsDirty)
			{
				Logger.Log("Configuration has been changed in-game, saving!");
				Configuration.Save();
			}
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
			if (paychecks.Count == 0)
			{
				return;
			}
			if (!Config.AllowPaychecksWhenDead && player.Dead)
		    {
			    if (!Config.DisplayNotification) return;
				UnturnedChat.Say(player, Translate("paycheck_dead"), Color.yellow);
			    return;
			}
		    if (!Config.AllowPaychecksInSafezone && player.Player.movement.isSafe)
		    {
			    if (!Config.DisplayNotification) return;
			    UnturnedChat.Say(player, Translate("paycheck_safezone"), Color.yellow);
			    return;
		    }
		    var experience = GetPaycheckExperience(player, paychecks);
		    var multiplier = GetPaycheckMultiplier(player.Position, paychecks);
			
			if (Mathf.Abs(multiplier) > 0.0001f)
			{
				var change = (int) (experience * multiplier);
				var experienceGiven = ExperienceHelper.ChangeExperience(player, change);
				if (!Config.DisplayNotification) return;
				if (experienceGiven != 0)
				{
					UnturnedChat.Say(player, Translate("paycheck_given", experienceGiven), Color.green);
				}
				else if (change != 0)
				{
					UnturnedChat.Say(player, Translate("paycheck_notgiven", change), Color.yellow);
				}
			}
		    else
			{
				if (!Config.DisplayNotification) return;
				UnturnedChat.Say(player, Translate("paycheck_zero_multiplier"), Color.yellow);
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
		    var zones = new List<PaycheckZone>();
		    zones.AddRange(Config.PaycheckZones);
		    foreach (var paycheck in paychecks)
		    {
			    zones.AddRange(paycheck.PaycheckZones);
		    }

			var multiplier = 1f;
		    var closestDistance = Mathf.Infinity;

		    foreach (var zone in zones)
		    {
			    if (zone.Point != null)
			    {
				    var distance = (position - zone.Point.GetValueOrDefault()).sqrMagnitude;
				    if (!(distance <= Mathf.Pow(zone.Radius, 2f))) continue;

				    if (Config.AllowMultipleMultipliers)
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

						if (Config.AllowMultipleMultipliers)
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
		    return multiplier;
	    }

	    public override TranslationList DefaultTranslations => new TranslationList
	    {
		    {"paycheck_zero_multiplier", "You cannot earn experience in this area!"},
		    {"paycheck_given", "You have received your paycheck of {0} experience!"},
		    {"paycheck_notgiven", "Your paycheck was {0}, but you were unable to receive it!"},
		    {"paycheck_dead", "You cannot receive paychecks while dead!"},
		    {"paycheck_safezone", "You cannot receive paychecks in a safezone!"},
		    {"command_paycheck_not_found", "Paycheck \"{0}\" could not be found!"},
		    {"command_list_paychecks", "Current paychecks:{0}"},
		    {"command_no_paychecks", "There are no paychecks set up!"},
			{"command_default_no_zones", "There are no global zones set up!"},
		    {"command_paycheck_no_zones", "\"{0}\" has no zones set up!"},
		    {"command_list_default_zones", "Default paycheck zones:{0}"},
		    {"command_list_paycheck_zones", "Paycheck \"{0}\" paycheck zones:{1}"},
		    {"command_paycheck_deleted", "Paycheck \"{0}\" has been deleted!"},
		    {"command_delete_zone_no_parse", "Could not find zone!"},
		    {"command_invalid_out_of_bounds", "Index {0} is out of bounds {1} to {2}!"},
		    {"command_removed_zone_default", "Removed zone at {0} from global zones!"},
		    {"command_removed_zone_paycheck", "Removed zone at {1} from paycheck \"{0}\"!"},
		    {"command_no_parse_experience", "Could not parse \"{0}\" as the experience!"},
		    {"command_paycheck_created", "Created paycheck named \"{0}\" with {1}XP, players with \"paycheck.{0}\" permissions will have access to it!"},
		    {"command_no_console", "This command cannot be called from the console in this way!"},
		    {"command_no_parse_multiplier", "Could not parse \"{0}\" as the multiplier!"},
		    {"command_no_parse_radius", "Could not parse \"{0}\" as the radius!"},
		    {"command_no_parse_location", "Could not parse \"{0}\" as coordinates or a node!"},
		    {"command_created_zone_default", "Created a global zone at {0} with a multiplier of {1} and radius of {2}!"},
		    {"command_created_zone_paycheck", "Created a zone for \"{0}\" at {1} with a multiplier of {2} and radius of {3}!"},
		    {"command_no_parse_paycheck_or_location", "Could not parse \"{0}\" as a paycheck, coordinates or a node!"}
		};
    }
}