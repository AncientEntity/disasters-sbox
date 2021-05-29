using Sandbox.UI;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Disasters
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class DisastersHUD : Sandbox.HudEntity<RootPanel>
	{

		public DisastersHUD()
		{
			if ( IsClient )
			{


				RootPanel.AddChild<GameStats>();
				RootPanel.AddChild<ChatBox>();
			}
		}


		public class GameStats : Panel
		{
			public string DisasterName
			{
				get
				{
					if ( DisastersGame.game.currentState == DisastersGame.gameStates.waiting )
					{
						return "Waiting...";
					}
					if ( DisastersGame.game.currentDisaster == null )
					{
						return "Disasters";
					}
					return DisastersGame.game.currentDisaster.name;
				}
			}
			public string DisasterDesc
			{
				get
				{
					if ( DisastersGame.game.currentState == DisastersGame.gameStates.waiting )
					{
						return "Round starting soon...";
					}
					if ( DisastersGame.game.currentDisaster == null )
					{
						return "Game is loading...";
					}
					return DisastersGame.game.currentDisaster.desc;
				}
			}
			public string RoundText { 
				get {
					return "Round:    "+DisastersGame.game.roundNumber + "/"+DisastersGame.game.maxRounds+"   Time Remaining: "+(int)DisastersGame.game.GetTimeLeft(); 
				}
			}
			public GameStats()
			{
				SetTemplate( "/DisastersHUD.html" );
			}
		}
	}

}
