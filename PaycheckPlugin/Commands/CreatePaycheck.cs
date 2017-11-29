using System.Collections.Generic;
using PhaserArray.PaycheckPlugin.Helpers;
using PhaserArray.PaycheckPlugin.Serialization;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class CreatePaycheck : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "createpaycheck";
		public string Help => "Creates a paycheck!";
		public string Syntax => "[name] [experience] <paychecktocopyzonesfrom>";
		public List<string> Aliases => new List<string> {"cpay"};
		public List<string> Permissions => new List<string> {"paychecks.commands.manage"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			if (command.Length != 2 && command.Length != 3)
			{
				UnturnedChat.Say(caller, $"Use /{Name} {Syntax}", Color.yellow);
				return;
			}

			if (!int.TryParse(command[1], out var experience))
			{
				UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_no_parse_experience", command[1]), Color.yellow);
				return;
			}

			var zones = new List<PaycheckZone>();
			if (command.Length == 3)
			{
				var paycheckResult = PaycheckHelper.FindBestMatch(command[2]);
				if (paycheckResult != null)
				{
					zones = paycheckResult.PaycheckZones;
				}
				else
				{
					UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_paycheck_not_found", command[2]), Color.yellow);
					return;
				}
			}

			var paycheck = new Paycheck(command[0].ToLower(), experience, zones);
			PaycheckPlugin.Config.Paychecks.Add(paycheck);
			PaycheckPlugin.Config.IsDirty = true;
			UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_paycheck_created", paycheck.Name, paycheck.Experience), Color.cyan);
		}
	}
}