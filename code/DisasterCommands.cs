using System;
using Sandbox;
namespace Disasters
{
	public class Commands
	{
		[ServerCmd("ds_endRound")]
		public static void EndRound()
		{
			Log.Info( "Round Ended." );
			DisastersGame.game.EndRound();
		}
	}
}
