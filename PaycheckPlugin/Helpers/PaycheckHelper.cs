using JetBrains.Annotations;
using PhaserArray.PaycheckPlugin.Serialization;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	public class PaycheckHelper
	{
		[CanBeNull]
		public static Paycheck FindBestMatch(string search)
		{
			search = search.ToLower();

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (PaycheckPlugin.Config.Paychecks.Count == 0)
			{
				return null;
			}
			if (PaycheckPlugin.Config.Paychecks.Count == 1)
			{
				return PaycheckPlugin.Config.Paychecks[0].Name.ToLower().Contains(search)
					? PaycheckPlugin.Config.Paychecks[0]
					: null;
			}

			var bestMatchPercentage = 0f;
			Paycheck bestMatch = null;
			foreach (var paycheck in PaycheckPlugin.Config.Paychecks)
			{
				if (!paycheck.Name.ToLower().Contains(search)) continue;

				var matchPercentage = search.Length / paycheck.Name.Length;
				if (!(matchPercentage > bestMatchPercentage)) continue;
				bestMatchPercentage = matchPercentage;
				bestMatch = paycheck;
			}
			return bestMatch;
		}
	}
}
