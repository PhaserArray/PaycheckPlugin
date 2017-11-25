using Rocket.Unturned.Player;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	internal class ExperienceHelper
	{
		/// <summary>
		/// Changes the player's experience safely, avoiding overflow.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="change"></param>
		/// <returns>True Change</returns>
		public static long ChangeExperience(UnturnedPlayer player, int change)
		{
			var trueChange = change < -player.Experience
				? -player.Experience
				: change > uint.MaxValue - player.Experience
					? (int) (uint.MaxValue - player.Experience)
					: change;
			player.Experience = (uint)(player.Experience + trueChange);
			return trueChange;
		}
	}
}
