using System.Collections.Generic;
using Rocket.API;

namespace PhaserArray.AutomaticPaychecks.Serialization
{
	public class AutomaticPaycheckConfiguration : IRocketPluginConfiguration
	{
		public float Interval;
		public bool DisplayNotification;
		public bool AllowPaychecksWhenDead;
		public bool AllowMultiplePaychecks;
		public bool OnlyUseClosestZone;
		public List<Paycheck> Paychecks;
		public List<PaycheckZone> PaycheckZones;

		public void LoadDefaults()
		{
			Interval = 10;
			DisplayNotification = true;
			AllowPaychecksWhenDead = false;
			AllowMultiplePaychecks = false;
			OnlyUseClosestZone = false;
			Paychecks = new List<Paycheck>
			{
				new Paycheck("default", 500)
			};
			PaycheckZones = new List<PaycheckZone>
			{
				new PaycheckZone(" HQ", 400f, 0f)
			};
		}
	}
}