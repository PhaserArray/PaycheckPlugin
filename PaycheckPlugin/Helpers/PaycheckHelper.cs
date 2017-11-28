using JetBrains.Annotations;
using PhaserArray.PaycheckPlugin.Serialization;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	public class PaycheckHelper
	{
		/// <summary>
		/// Finds the best matching paycheck. Searches based on names.
		/// </summary>
		/// <param name="search"></param>
		/// <returns>Paycheck</returns>
		[CanBeNull]
		public static Paycheck FindBestMatch(string search)
		{
			var index = FindBestMatchIndex(search);
			return index != null ? PaycheckPlugin.Config.Paychecks[index.Value] : null;
		}

		/// <summary>
		/// Finds the best matching paycheck's index. Searches based on names.
		/// </summary>
		/// <param name="search"></param>
		/// <returns>Paycheck Index</returns>
		public static int? FindBestMatchIndex(string search)
		{
			search = search.ToLower();

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (PaycheckPlugin.Config.Paychecks.Count == 0)
			{
				return null;
			}
			if (PaycheckPlugin.Config.Paychecks.Count == 1)
			{
				if (PaycheckPlugin.Config.Paychecks[0].Name.ToLower().Contains(search))
				{
					return 0;
				}
				return null;
			}

			var bestMatchPercentage = 0f;
			int? bestMatch = null;
			for (var i = 0; i < PaycheckPlugin.Config.Paychecks.Count; i++)
			{
				var paycheck = PaycheckPlugin.Config.Paychecks[i];

				if (!paycheck.Name.ToLower().Contains(search)) continue;

				if (paycheck.Name.Length == search.Length) return i;

				var matchPercentage = (float)search.Length / paycheck.Name.Length;
				if (!(matchPercentage > bestMatchPercentage)) continue;
				bestMatchPercentage = matchPercentage;
				bestMatch = i;
			}
			return bestMatch;
		}
	}
}