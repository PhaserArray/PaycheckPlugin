using JetBrains.Annotations;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Serialization
{
	public class PaycheckZone
	{
		[CanBeNull] public string Node;
		[CanBeNull] public Vector3? Point;
		public float Radius;
		public float Multiplier;

		public PaycheckZone() { }

		public PaycheckZone(string node, float radius, float multiplier)
		{
			Node = node;
			Point = null;
			Radius = radius;
			Multiplier = multiplier;
		}

		public PaycheckZone(Vector3 point, float radius, float multiplier)
		{
			Point = point;
			Node = null;
			Radius = radius;
			Multiplier = multiplier;
		}
	}
}