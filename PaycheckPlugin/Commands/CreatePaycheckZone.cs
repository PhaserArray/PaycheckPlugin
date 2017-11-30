using System.Collections.Generic;
using PhaserArray.PaycheckPlugin.Helpers;
using PhaserArray.PaycheckPlugin.Serialization;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class CreatePaycheckZone : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "createpaycheckzone";
		public string Help => "Placeholder";
		public string Syntax => "<paycheck> <node | x,y,z> [radius] [multiplier]";
		public List<string> Aliases => new List<string> {"cpayz"};
		public List<string> Permissions => new List<string> {"paychecks.commands.manage"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			if (command.Length != 2 && command.Length != 3 && command.Length != 4)
			{
				UnturnedChat.Say(caller, $"Use /{Name} {Syntax}", Color.yellow);
				return;
			}

			if (!float.TryParse(command[command.Length - 1], out var multiplier))
			{
				UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_parse_multiplier", command[command.Length - 1]), Color.yellow);
				return;
			}

			if (!float.TryParse(command[command.Length - 2], out var radius))
			{
				UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_parse_radius", command[command.Length - 2]), Color.yellow);
				return;
			}

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (command.Length == 2)
			{
				if (caller.Id == "Console")
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_console"), Color.yellow);
					return;
				}
				var zone = new PaycheckZone(Vector3Helper.Round(((UnturnedPlayer)caller).Position), radius, multiplier);
				PaycheckPlugin.Config.PaycheckZones.Add(zone);
				UnturnedChat.Say(caller, 
					PaycheckPlugin.Instance.Translate("command_created_zone_default", 
						ZoneHelper.GetLocationString(zone),
						zone.Multiplier+"x",
						zone.Radius), 
					Color.cyan);
			}
			else if (command.Length == 3)
			{
				var paycheckIndex = PaycheckHelper.FindBestMatchIndex(command[0]);
				if (paycheckIndex != null)
				{
					if (caller.Id == "Console")
					{
						UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_console"), Color.yellow);
						return;
					}
					var zone = new PaycheckZone(Vector3Helper.Round(((UnturnedPlayer)caller).Position), radius, multiplier);
					PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].PaycheckZones.Add(zone);
					UnturnedChat.Say(caller,
						PaycheckPlugin.Instance.Translate("command_created_zone_paycheck",
							PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].Name,
							ZoneHelper.GetLocationString(zone),
							zone.Multiplier + "x",
							zone.Radius),
						Color.cyan);
				}
				else
				{
					var pointResult = Vector3Helper.Parse(command[0]);
					if (pointResult != null)
					{
						var zone = new PaycheckZone(Vector3Helper.Round(pointResult.Value), radius, multiplier);
						PaycheckPlugin.Config.PaycheckZones.Add(zone);
						UnturnedChat.Say(caller,
							PaycheckPlugin.Instance.Translate("command_created_zone_default",
								ZoneHelper.GetLocationString(zone),
								zone.Multiplier + "x",
								zone.Radius),
							Color.cyan);
					}
					else if (NodeHelper.Exists(command[0]))
					{
						var zone = new PaycheckZone(command[0], radius, multiplier);
						PaycheckPlugin.Config.PaycheckZones.Add(zone);
						UnturnedChat.Say(caller,
							PaycheckPlugin.Instance.Translate("command_created_zone_default",
								ZoneHelper.GetLocationString(zone),
								zone.Multiplier + "x",
								zone.Radius),
							Color.cyan);
					}
					else
					{
						UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_parse_paycheck_or_location", command[0]), Color.yellow);
						return;
					}
				}
			}
			else if (command.Length == 4)
			{
				var paycheckIndex = PaycheckHelper.FindBestMatchIndex(command[0]);
				if (paycheckIndex == null)
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_created_zone_paycheck", command[0]), Color.yellow);
					return;
				}
				var pointResult = Vector3Helper.Parse(command[1]);
				if (pointResult != null)
				{
					var zone = new PaycheckZone(Vector3Helper.Round(pointResult.Value), radius, multiplier);
					PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].PaycheckZones.Add(zone);
					UnturnedChat.Say(caller,
						PaycheckPlugin.Instance.Translate("command_created_zone_paycheck",
							PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].Name,
							ZoneHelper.GetLocationString(zone),
							zone.Multiplier + "x",
							zone.Radius),
						Color.cyan);
				}
				else if (NodeHelper.Exists(command[1]))
				{
					var zone = new PaycheckZone(command[1], radius, multiplier);
					PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].PaycheckZones.Add(zone);
					UnturnedChat.Say(caller,
						PaycheckPlugin.Instance.Translate("command_created_zone_paycheck",
							PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].Name,
							ZoneHelper.GetLocationString(zone),
							zone.Multiplier + "x",
							zone.Radius),
						Color.cyan);
				}
				else
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_parse_location", command[1]), Color.yellow);
					return;
				}
			}
			PaycheckPlugin.Config.IsDirty = true;
		}
	}
}