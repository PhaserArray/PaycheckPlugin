using System.Collections.Generic;
using System.Text;
using Rocket.API;
using Rocket.Unturned.Chat;

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
			var response = new StringBuilder();
			response.Append("Current paychecks: ");
			foreach (var paycheck in PaycheckPlugin.Config.Paychecks)
			{
				response.AppendFormat(" {0}({1}),", paycheck.Name, paycheck.Experience);
			}
			response.Remove(response.Length - 1, 1);
			UnturnedChat.Say(caller, response.ToString());
		}
	}
}