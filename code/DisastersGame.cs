
using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Disasters
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	[Library( "disasters" )]
	public partial class DisastersGame : Sandbox.Game
	{
		public static DisastersGame game = null;
		public static Random random = new Random();
		private static List<Entity> eventEntities = new List<Entity>();

		//Game settings
		[Net]
		public int maxRounds { get; set; }
		public float roundDuration = 60f;
		public float waitingDuration = 5f;
		public List<Disaster> disasters = new List<Disaster>();

		//Current Game Data
		[Net]
		public gameStates currentState { get; set; }
		[Net]
		public int roundNumber { get; set; }
		//[Net]
		//public float timeLeft { get; set; }
		[Net]
		TimeSince timeLeft { get; set; }
		[Net]
		public int currentDisasterIndex { get; set; }
		public Disaster currentDisaster { get { return disasters[currentDisasterIndex]; } }



		public DisastersGame()
		{
			if(game == null)
			{
				game = this;
			}

			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				if(maxRounds == 0)
				{
					maxRounds = 10;
				}
				currentState = gameStates.waiting;
				timeLeft = 0;

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new DisastersHUD();
			}
			InitializeDisasters();
			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}

		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new DisasterPlayer();
			client.Pawn = player;

			if ( currentState == gameStates.waiting )
			{
				player.Respawn();
			}
		}

		public enum roundStates
		{
			waiting,
			running,
		}
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			if(IsServer)
			{
				RoundManagement();
			}

		}

		void RoundManagement()
		{
			//Round must be happening
			if ( GetTimeLeft() <= 0 )
			{
				//Start a new round and exit waiting mode.
				if ( currentState == gameStates.waiting )
				{
					timeLeft = 0;
					roundNumber++;
					currentDisasterIndex = random.Next( 0, disasters.Count );
					currentDisaster.StartDisaster();
					Log.Info( "Starting next round: " + currentDisaster.name + "|" + currentDisaster.desc );
					currentState = gameStates.playing;


				} else if (currentState == gameStates.playing)
				{
					timeLeft = 0;
					currentDisaster.EndDisaster();
					currentState = gameStates.waiting;
					foreach(Entity e in eventEntities)
					{
						if(e == null || !e.IsValid())
						{
							continue;
						}
						e.Delete();
					}
					eventEntities = new List<Entity>();

				}
			} else if (currentState == gameStates.playing)
			{
				currentDisaster.eventEntity.WhileEvent();
			}
			
		}

		public enum gameStates
		{
			waiting,
			playing,
		}

		public float GetTimeLeft()
		{
			if ( currentState == gameStates.waiting )
			{
				return waitingDuration - timeLeft; 
			} else if (currentState == gameStates.playing)
			{
				return roundDuration - timeLeft;
			}
			return 0;
		}

		public static void RegisterEventEntity(Entity ent)
		{
			eventEntities.Add( ent );
		}

		public void EndRound()
		{
			timeLeft = 99999;
		}

	}

}
