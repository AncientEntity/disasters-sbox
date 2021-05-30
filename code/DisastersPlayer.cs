using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Disasters
{
	partial class DisasterPlayer : Player
	{

		public static List<DisasterPlayer> allPlayers = new List<DisasterPlayer>();

		public ICamera lastCam = null;

		public DisasterPlayer()
		{
			if(!allPlayers.Contains(this)) {
				allPlayers.Add( this );
			}
			SetupPhysicsFromModel( PhysicsMotionType.Static, false );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new WalkController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			if ( lastCam == null )
			{
				Camera = new FirstPersonCamera();
				lastCam = Camera;
			} else
			{
				Camera = lastCam;
			}

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			Dress();
			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			if ( Input.Pressed( InputButton.View ) && LifeState == LifeState.Alive)
			{
				if ( Camera is FirstPersonCamera )
				{
					Camera = new ThirdPersonCamera();
				}
				else
				{
					Camera = new FirstPersonCamera();
				}
			}

			if ( IsServer && LifeState == LifeState.Dead)
			{
				if ( DisastersGame.game.currentState == DisastersGame.gameStates.waiting )
				{
					Respawn();
				}
			}

			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );

			//If we're running serverside and Attack1 was just pressed, spawn a ragdoll


			//if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			//{
			//	var ragdoll = new MelonHailEventEntity.MelonEntity();
			//	ragdoll.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
			//	ragdoll.Position = EyePos + EyeRot.Forward * 40;
			//	ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
			//	ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			//	ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
			//}
		}

		public override void OnKilled()
		{
			base.OnKilled();
			lastCam = Camera;
			Camera = new SpectateRagdollCamera();
			Controller = new NoclipController();

			EnableAllCollisions = false;

			RemoveClothes();
			SetModel( "models/citizen/clothes/ghost.vmdl_c" );
			//EnableDrawing = false;
		}
	}
}
