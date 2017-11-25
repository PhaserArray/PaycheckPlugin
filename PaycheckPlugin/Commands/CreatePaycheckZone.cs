using System;
using System.Collections.Generic;
using Rocket.API;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class CreatePaycheckZone : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "createpaycheckzone";
		public string Help => "Placeholder"; // TODO
		public string Syntax => "Placeholder"; // TODO
		public List<string> Aliases => new List<string> {"cpayzone"};
		public List<string> Permissions => new List<string> {"paychecks.commands.manage"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			throw new NotImplementedException();
		}
	}
}