using System.Collections.Generic;
using Rocket.API;
using Rocket.Core;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	internal class PermissionsHelper
	{
		/// <summary>
		/// Checks whether the player has a specific permission. Does not give special treatment to admins.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="permission"></param>
		public static bool HasPermission(IRocketPlayer player, string permission)
		{
			return HasPermissions(player, new List<string> {permission});
		}
		
		/// <summary>
		/// Checks whether the player has a specific permissions. Does not give special treatment to admins.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="permissions"></param>
		public static bool HasPermissions(IRocketPlayer player, List<string> permissions)
		{
			return R.Permissions.GetPermissions(player, permissions).Count != 0;
		}
	}
}
