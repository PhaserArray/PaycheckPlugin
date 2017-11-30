using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Commands
{
	public class NextPaycheck : IRocketCommand
	{
		public AllowedCaller AllowedCaller => AllowedCaller.Both;
		public string Name => "nextpaycheck";
		public string Help => "Shows time remaining until the next paycheck";
		public string Syntax => "";
		public List<string> Aliases => new List<string> {"npay"};
		public List<string> Permissions => new List<string> {"paychecks.commands.view"};

		public void Execute(IRocketPlayer caller, string[] command)
		{
			var timeToNext = PaycheckPlugin.Instance.SecondsToNextPaycheck;
			if (timeToNext > 60f)
			{
				UnturnedChat.Say(caller, 
					PaycheckPlugin.Instance.Translate("command_time_to_next_paycheck_minutes", 
						Mathf.Floor(timeToNext/60f), Mathf.Round(timeToNext % 60f)), 
					Color.green);
			}
			else
			{
				UnturnedChat.Say(caller,
					PaycheckPlugin.Instance.Translate("command_time_to_next_paycheck",
						Mathf.Round(timeToNext)),
					Color.green);
			}
		}
	}
}