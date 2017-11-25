using System.Collections.Generic;
using System.Text;
using Rocket.API;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class ListPaychecks : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "listpaychecks";
		public string Help => "Placeholder"; // TODO
		public string Syntax => "Placeholder"; // TODO
		public List<string> Aliases => new List<string> {"lpay", "lpays", "listpaycheck"};
		public List<string> Permissions => new List<string>();

		public void Execute(IRocketPlayer caller, string[] command)
		{
			var response = new StringBuilder();
			foreach (var paycheck in COLLECTION)
			{
				
			}
		}
	}
}