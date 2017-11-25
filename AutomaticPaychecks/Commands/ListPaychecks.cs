using System;
using System.Collections.Generic;
using Rocket.API;

namespace PhaserArray.AutomaticPaychecks.Commands
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
			throw new NotImplementedException();
		}
	}
}