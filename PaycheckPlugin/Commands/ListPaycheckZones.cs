using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhaserArray.PaycheckPlugin.Helpers;
using PhaserArray.PaycheckPlugin.Serialization;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class ListPaycheckZones : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "listpaycheckzones";
		public string Help => "Lists all zones for the provided paycheck, lists global zones if paycheck is not provided."; // TODO
		public string Syntax => "<paycheck>"; // TODO
		public List<string> Aliases => new List<string> {"lpayzone", "lpayzones", "listpaycheckzone"};
		public List<string> Permissions => new List<string> {"paychecks.commands.view"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			List<PaycheckZone> zones;
			Paycheck paycheck = null;
			if (command.Length > 0)
			{
				paycheck = PaycheckHelper.FindBestMatch(command[0]);
				if (paycheck == null)
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_paycheck_not_found", command[0]));
					return;
				}
				zones = paycheck.PaycheckZones;
			}
			else
			{
				zones = PaycheckPlugin.Config.PaycheckZones;
			}

			if (zones.Count == 0)
			{
				UnturnedChat.Say(caller,
					command.Length == 0
						? PaycheckPlugin.Instance.Translate("command_default_no_zones")
						: PaycheckPlugin.Instance.Translate("command_paycheck_no_zones", paycheck?.Name));
				return;
			}

			var zonesString = new StringBuilder();
			zonesString.Append(" ");
			for (var i = 0; i < zones.Count; i++)
			{
				if (zones[i].Point != null)
				{
					var vector = zones[i].Point.Value;
					var vectorString = $"({Mathf.Round(vector.x)},{Mathf.Round(vector.y)},{Mathf.Round(vector.z)})";
					zonesString.AppendFormat(" [{0}] - {1},", i + 1, vectorString);
				}
				else if (zones[i].Node != null)
				{
					zonesString.AppendFormat(" [{0}] - {1},", i + 1, zones[i].Node);
				}
			}
			zonesString.Remove(zonesString.Length - 1, 1);
			UnturnedChat.Say(caller,
				command.Length == 0
					? PaycheckPlugin.Instance.Translate("command_list_default_zones", zonesString.ToString())
					: PaycheckPlugin.Instance.Translate("command_list_paycheck_zones", paycheck?.Name, zonesString.ToString()));
		}
	}
}