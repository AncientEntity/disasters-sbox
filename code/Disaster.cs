using Sandbox;

namespace Disasters
{
	public class Disaster
	{
		public string name { get; set; }
		public string desc { get; set; }

		public Disaster() { }

		public Disaster(string name, string desc)
		{
			this.name = name;
			this.desc = desc;
		}
	}

	public partial class DisastersGame
	{
		public void InitializeDisasters()
		{
			disasters.Add(new Disaster("Errorpocolypse","Avoid the errors or you'll become one!"));
			disasters.Add(new Disaster("Terrynado","Tornado but it's a bunch of Terries!"));
		}
	}

}
