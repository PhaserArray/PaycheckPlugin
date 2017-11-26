using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;
using UnityEngine;

namespace PhaserArray.PaycheckPlugin.Serialization
{
	public class PaycheckPluginConfiguration : IRocketPluginConfiguration
	{
		public float Interval;
		public bool DisplayNotification;
		public bool AllowMultipleMultipliers;
		public bool AllowPaychecksWhenDead;
		public bool AllowPaychecksInSafezone;
		public bool AllowMultiplePaychecks;
		public List<Paycheck> Paychecks;
		public List<PaycheckZone> PaycheckZones;

		[XmlIgnore]
		public bool IsDirty = false;

		public void LoadDefaults()
		{
			Interval = 10;
			DisplayNotification = true;
			AllowMultipleMultipliers = true;
			AllowPaychecksWhenDead = false;
			AllowPaychecksInSafezone = false;
			AllowMultiplePaychecks = false;
			Paychecks = new List<Paycheck>
			{
				new Paycheck("default", 100)
			};
			PaycheckZones = new List<PaycheckZone>
			{
				new PaycheckZone(" HQ", 400f, 0f),
				new PaycheckZone(new Vector3(1f,2f,3f), 200f, 1.2f)
			};
		}
	}
}