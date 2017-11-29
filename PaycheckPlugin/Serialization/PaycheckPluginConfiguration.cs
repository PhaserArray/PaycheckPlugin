using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;

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
			Interval = 600f;
			DisplayNotification = true;
			AllowMultipleMultipliers = true;
			AllowPaychecksWhenDead = false;
			AllowPaychecksInSafezone = false;
			AllowMultiplePaychecks = false;
			Paychecks = new List<Paycheck>
			{
				new Paycheck("pvt", 100),
				new Paycheck("pfc", 150),
				new Paycheck("cpl", 200),
				new Paycheck("sgt", 250),
				new Paycheck("ssgt", 300),
				new Paycheck("1sgt", 350),
				new Paycheck("msgt", 400),
				new Paycheck("2lt", 500),
				new Paycheck("1lt", 600),
				new Paycheck("cpt", 700),
				new Paycheck("maj", 1200),
				new Paycheck("ltc", 1700),
				new Paycheck("col", 2200),
				new Paycheck("bg", 2700)
			};
			PaycheckZones = new List<PaycheckZone>
			{
				new PaycheckZone(" HQ", 270f, 0.5f)
			};
		}
	}
}