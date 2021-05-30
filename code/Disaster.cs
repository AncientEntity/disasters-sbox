using Sandbox;

namespace Disasters
{
	public class Disaster
	{
		public string name { get; set; }
		public string desc { get; set; }
		public EventEntity eventEntity = null;


		public Disaster() { }

		public Disaster(string name, string desc, EventEntity eventEntity)
		{
			this.name = name;
			this.desc = desc;
			this.eventEntity = eventEntity;
		}

		public void StartDisaster()
		{
			if(eventEntity == null)
			{
				return;
			}
			eventEntity.enabled = true;
			eventEntity.OnDisasterStart();
		}

		public void EndDisaster()
		{
			if ( eventEntity == null )
			{
				return;
			}
			eventEntity.enabled = false;
		}
	}

	public partial class DisastersGame
	{
		public void InitializeDisasters()
		{
			if ( !IsServer )
			{
				disasters.Add( new Disaster( "Melon Hail", "It's raining watermelons!", null ) );
				disasters.Add( new Disaster( "Errorpocolypse", "Avoid the errors or you'll become one!", null ) );
			} else
			{
				disasters.Add( new Disaster( "Errorpocolypse", "Avoid the errors or you'll become one!", new ErrorEventEntity() ) );
			}
			if ( !IsServer )
			{
				disasters.Add( new Disaster( "Melon Hail", "It's raining watermelons!", null ) );
			}
			else
			{
				disasters.Add( new Disaster( "Melon Hail", "It's raining watermelons!", new MelonHailEventEntity() ) );

			}
			//disasters.Add( new Disaster( "Terrynado", "Tornado but it's a bunch of Terries!", null ) );

		}
	}

}
