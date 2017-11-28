using System.Collections.Generic;
using PhaserArray.PaycheckPlugin.Helpers;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class DeletePaycheckZone : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "deletepaycheckzone";
		public string Help => "Placeholder";
		public string Syntax => "<paycheck> [index | node | (x,y,z)]";
		public List<string> Aliases => new List<string> {"dpayz"};
		public List<string> Permissions => new List<string> {"paychecks.commands.manage"};
		
		public void Execute(IRocketPlayer caller, string[] command)
		{
			if (command.Length != 1 && command.Length != 2)
			{
				UnturnedChat.Say(caller, $"Use /{Name} {Syntax}", Color.yellow);
				return;
			}

			var paycheckIndex = 0;
			var zones = PaycheckPlugin.Config.PaycheckZones;
			if (command.Length == 2)
			{
				var paycheck = PaycheckHelper.FindBestMatchIndex(command[0]);
				if (paycheck == null)
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_paycheck_not_found", command[0]), Color.yellow);
					return;
				}
				paycheckIndex = paycheck.Value;
				zones = PaycheckPlugin.Config.Paychecks[paycheckIndex].PaycheckZones;
			}

			if (zones.Count == 0)
			{
				UnturnedChat.Say(caller,
					command.Length == 0
						? PaycheckPlugin.Instance.Translate("command_default_no_zones")
						: PaycheckPlugin.Instance.Translate("command_paycheck_no_zones", PaycheckPlugin.Config.Paychecks[paycheckIndex].Name), Color.yellow);
				return;
			}

			if (!int.TryParse(command[command.Length - 1], out var index))
			{
				var bestMatchIndex = ZoneHelper.FindBestMatchIndex(zones, command[command.Length - 1]);
				if (bestMatchIndex != null)
				{
					index = bestMatchIndex.Value;
				}
				else
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_delete_zone_no_parse"), Color.yellow);
					return;
				}
			}
			else
			{
				index--;
				if (index >= zones.Count || index < 0)
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_invalid_out_of_bounds", index + 1, 1, zones.Count), Color.yellow);
					return;
				}
			}

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (command.Length == 1)
			{
				UnturnedChat.Say(caller, 
					PaycheckPlugin.Instance.Translate("command_removed_zone_default",
						ZoneHelper.GetLocationString(PaycheckPlugin.Config.PaycheckZones[index])),
					Color.magenta);
				PaycheckPlugin.Config.PaycheckZones.RemoveAt(index);
			}
			else if (command.Length == 2)
			{
				UnturnedChat.Say(caller,
					PaycheckPlugin.Instance.Translate("command_removed_zone_paycheck",
						PaycheckPlugin.Config.Paychecks[paycheckIndex].Name,
						ZoneHelper.GetLocationString(PaycheckPlugin.Config.Paychecks[paycheckIndex].PaycheckZones[index])),
					Color.magenta);
				PaycheckPlugin.Config.Paychecks[paycheckIndex].PaycheckZones.RemoveAt(index);
			}
			PaycheckPlugin.Config.IsDirty = true;
		}
	}
}