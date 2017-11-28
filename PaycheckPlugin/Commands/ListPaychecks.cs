using System.Collections.Generic;
using System.Text;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class ListPaychecks : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "listpaychecks";
		public string Help => "Lists all paychecks";
		public string Syntax => "";
		public List<string> Aliases => new List<string> {"lpay", "lpays", "listpaycheck"};
		public List<string> Permissions => new List<string> {"paychecks.commands.view"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			if (PaycheckPlugin.Config.Paychecks.Count == 0)
			{
				UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_paycheck_not_found"), Color.yellow);
				return;
			}

			var paychecksString = new StringBuilder();
			paychecksString.Append(" ");
			foreach (var paycheck in PaycheckPlugin.Config.Paychecks)
			{
				paychecksString.AppendFormat(" {0} ({1}XP),", paycheck.Name, paycheck.Experience);
			}
			paychecksString.Remove(paychecksString.Length - 1, 1);
			UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_list_paychecks", paychecksString.ToString()));
		}
	}
}