using System.Collections.Generic;

namespace PhaserArray.PaycheckPlugin.Serialization
{
	public class Paycheck
	{
		public string Name;
		public int Experience;
		public List<PaycheckZone> PaycheckZones;

		public Paycheck() { }

		public Paycheck(string name, int experience)
		{
			Name = name;
			Experience = experience;
			PaycheckZones = new List<PaycheckZone>();
		}

		public Paycheck(string name, int experience, List<PaycheckZone> paycheckZones)
		{
			Name = name;
			Experience = experience;
			PaycheckZones = paycheckZones;
		}
	}
}