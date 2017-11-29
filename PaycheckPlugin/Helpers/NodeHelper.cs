using System.Linq;
using SDG.Unturned;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	public class NodeHelper
	{
		public static bool Exists(string search)
		{
			return LevelNodes.nodes.Where(node => node.type == ENodeType.LOCATION).Any(node => ((LocationNode)node).name.Contains(search));
		}
	}
}
