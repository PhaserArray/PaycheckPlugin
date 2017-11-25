using System;
using System.Collections.Generic;
using Rocket.API;

namespace PhaserArray.AutomaticPaychecks.Commands
{
	public class ListPaycheckZones : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "listpaycheckzones";
		public string Help => "Placeholder"; // TODO
		public string Syntax => "Placeholder"; // TODO
		public List<string> Aliases => new List<string> {"lpayzone", "lpayzones", "listpaycheckzone"};
		public List<string> Permissions => new List<string>();

		public void Execute(IRocketPlayer caller, string[] command)
		{
			throw new NotImplementedException();
		}
	}
}