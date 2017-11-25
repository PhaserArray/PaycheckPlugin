using System;
using System.Collections.Generic;
using Rocket.API;

namespace PhaserArray.AutomaticPaychecks.Commands
{
	public class CreatePaycheck : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "createpaycheck";
		public string Help => "Placeholder"; // TODO
		public string Syntax => "Placeholder"; // TODO
		public List<string> Aliases => new List<string> {"cpay"};
		public List<string> Permissions => new List<string> {"paychecks.commands.manage"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			throw new NotImplementedException();
		}
	}
}