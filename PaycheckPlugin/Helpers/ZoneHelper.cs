using System.Collections.Generic;
using PhaserArray.PaycheckPlugin.Serialization;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Helpers
{
	public class ZoneHelper
	{
		/// <summary>
		/// Finds the best matching zone based on a string. If the string can be converted to Vector3, it searches based on Vector3.
		/// </summary>
		/// <param name="zones"></param>
		/// <param name="search"></param>
		/// <returns>Best Matching Zone Index</returns>
		public static int? FindBestMatchIndex(List<PaycheckZone> zones, string search)
		{
			if (zones.Count == 0) return null;
			var vector = Vector3Helper.Parse(search);
			if (vector != null)
			{
				return FindBestMatchIndex(zones, vector.Value);
			}

			search = search.ToLower();
			if (zones.Count == 1)
			{
				if (zones[0].Node != null && zones[0].Node.ToLower().Contains(search))
				{
					return 0;
				}
				return null;
			}
			var bestMatchPercentage = 0f;
			int? bestMatch = null;
			for (var i = 0; i < zones.Count; i++)
			{
				var zone = zones[i];
				if (zone.Node == null) continue;
				if (!zone.Node.ToLower().Contains(search)) continue;

				if (zone.Node.Length == search.Length) return i;
				
				var matchPercentage = (float)search.Length / zone.Node.Length;
				if (!(matchPercentage > bestMatchPercentage)) continue;
				bestMatchPercentage = matchPercentage;
				bestMatch = i;
			}
			return bestMatch;
		}

		/// <summary>
		/// Finds the best matching zone based on the Vector3, a small difference between the vector and zone's point is allowed.
		/// </summary>
		/// <param name="zones"></param>
		/// <param name="vector"></param>
		/// <returns>Best Matching Zone Index</returns>
		public static int? FindBestMatchIndex(List<PaycheckZone> zones, Vector3 vector)
		{
			if (zones.Count == 0) return null;
			var bestMatchDifference = Mathf.Infinity;
			int? bestMatchIndex = null;
			var roundedVector = Vector3Helper.Round(vector);
			for (var i = 0; i < zones.Count; i++)
			{
				var zone = zones[i];
				if (zone.Point == null) continue;
				var point = zone.Point.Value;
				if (point == vector)
				{
					return i;
				}
				if (Vector3Helper.Round(point) != roundedVector) continue;
				var matchDifference = (point - vector).sqrMagnitude;
				if (!(matchDifference < bestMatchDifference)) continue;
				bestMatchDifference = matchDifference;
				bestMatchIndex = i;
			}
			return bestMatchIndex;
		}

		/// <summary>
		/// Gets the zone location, either a node or point and returns it as a string.
		/// </summary>
		/// <param name="zones"></param>
		/// <param name="vector"></param>
		/// <returns>Location String</returns>
		public static string GetLocationString(PaycheckZone zone)
		{
			if (zone.Point != null)
			{
				return zone.Point.ToString();
			}
			return zone.Node != null ? $"\"{zone.Node}\"" : "N/A";
		}
	}
}
