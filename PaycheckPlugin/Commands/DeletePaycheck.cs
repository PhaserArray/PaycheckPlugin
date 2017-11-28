using System.Collections.Generic;
using PhaserArray.PaycheckPlugin.Helpers;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class DeletePaycheck : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "deletepaycheck";
		public string Help => "Deletes a paycheck with the given name"; // TODO
		public string Syntax => "[paycheck]"; // TODO
		public List<string> Aliases => new List<string> {"dpay"};
		public List<string> Permissions => new List<string> {"paychecks.commands.manage"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			if (command.Length == 1)
			{
				var paycheckIndex = PaycheckHelper.FindBestMatchIndex(command[0]);
				if (paycheckIndex != null)
				{
					UnturnedChat.Say(caller, 
						PaycheckPlugin.Instance.Translate("command_paycheck_deleted", 
							PaycheckPlugin.Config.Paychecks[paycheckIndex.Value].Name), 
						Color.magenta);
					PaycheckPlugin.Config.Paychecks.RemoveAt(paycheckIndex.Value);
					PaycheckPlugin.Config.IsDirty = true;
					return;
				}
				UnturnedChat.Say(caller, PaycheckPlugin.Instance.Translate("command_paycheck_not_found", command[0]), Color.yellow);
				return;
			}
			UnturnedChat.Say(caller, $"Use /{Name} {Syntax}", Color.yellow);
		}
	}
}