//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Player interface used to query HMD transforms and VR hands
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	// Singleton representing the local VR player/user, with methods for getting
	// the player's hands, head, tracking origin, and guesses for various properties.
	//-------------------------------------------------------------------------
	public class Player : MonoBehaviour
	{
		[Tooltip( "Virtual transform corresponding to the meatspace tracking origin. Devices are tracked relative to this." )]
        public Transform TrackingOriginTransform;

		[Tooltip( "List of possible transforms for the head/HMD, including the no-SteamVR fallback camera." )]
        public Transform[] HmdTransforms;

		[Tooltip( "List of possible Hands, including no-SteamVR fallback Hands." )]
        public Hand[] Hands;

		[Tooltip( "Reference to the physics collider that follows the player's HMD position." )]
        public Collider HeadCollider;

		[Tooltip( "These objects are enabled when SteamVR is available" )]
        public GameObject RigSteamVR;

		[Tooltip( "These objects are enabled when SteamVR is not available, or when the user toggles out of VR" )]
        public GameObject Rig2DFallback;

		[Tooltip( "The audio listener for this player" )]
        public Transform AudioListener;

        public bool AllowToggleTo2D = true;

		//-------------------------------------------------
		// Singleton instance of the Player. Only one can exist at a time.
		//-------------------------------------------------
		private static Player instance;
		public static Player Instance
		{
			get
			{
				if ( instance == null )
				{
					instance = FindObjectOfType<Player>();
				}
				return instance;
			}
		}

		//-------------------------------------------------
		// Get the number of active Hands.
		//-------------------------------------------------
		public int HandCount
		{
			get
			{
				int count = 0;
				for ( int i = 0; i < Hands.Length; i++ )
				{
					if ( Hands[i].gameObject.activeInHierarchy )
					{
						count++;
					}
				}
				return count;
			}
		}

		//-------------------------------------------------
		// Get the i-th active Hand.
		//
		// i - Zero-based index of the active Hand to get
		//-------------------------------------------------
		public Hand GetHand( int i )
		{
			for ( int j = 0; j < Hands.Length; j++ )
			{
				if ( !Hands[j].gameObject.activeInHierarchy )
				{
					continue;
				}

				if ( i > 0 )
				{
					i--;
					continue;
				}

				return Hands[j];
			}

			return null;
		}

		//-------------------------------------------------
		public Hand LeftHand
		{
			get
			{
				for ( int j = 0; j < Hands.Length; j++ )
				{
					if ( !Hands[j].gameObject.activeInHierarchy )
					{
						continue;
					}

					if ( Hands[j].HandType != SteamVR_Input_Sources.LeftHand)
					{
						continue;
					}

					return Hands[j];
				}

				return null;
			}
		}

		//-------------------------------------------------
		public Hand RightHand
		{
			get
			{
				for ( int j = 0; j < Hands.Length; j++ )
				{
					if ( !Hands[j].gameObject.activeInHierarchy )
					{
						continue;
					}

					if ( Hands[j].HandType != SteamVR_Input_Sources.RightHand)
					{
						continue;
					}

					return Hands[j];
				}

				return null;
			}
		}

        //-------------------------------------------------
        // Get Player scale. Assumes it is scaled equally on all axes.
        //-------------------------------------------------

        public float Scale
        {
            get
            {
                return transform.lossyScale.x;
            }
        }

        //-------------------------------------------------
        // Get the HMD transform. This might return the fallback camera transform if SteamVR is unavailable or disabled.
        //-------------------------------------------------
        public Transform HmdTransform
		{
			get
			{
                if (HmdTransforms != null)
                {
                    for (int i = 0; i < HmdTransforms.Length; i++)
                    {
                        if (HmdTransforms[i].gameObject.activeInHierarchy)
                            return HmdTransforms[i];
                    }
                }
				return null;
			}
		}

		//-------------------------------------------------
		// Height of the eyes above the ground - useful for estimating player height.
		//-------------------------------------------------
		public float EyeHeight
		{
			get
			{
				Transform hmd = HmdTransform;
				if ( hmd )
				{
					Vector3 eyeOffset = Vector3.Project( hmd.position - TrackingOriginTransform.position, TrackingOriginTransform.up );
					return eyeOffset.magnitude / TrackingOriginTransform.lossyScale.x;
				}
				return 0.0f;
			}
		}

		//-------------------------------------------------
		// Guess for the world-space position of the player's feet, directly beneath the HMD.
		//-------------------------------------------------
		public Vector3 FeetPositionGuess
		{
			get
			{
				Transform hmd = HmdTransform;
				if ( hmd )
				{
					return TrackingOriginTransform.position + Vector3.ProjectOnPlane( hmd.position - TrackingOriginTransform.position, TrackingOriginTransform.up );
				}
				return TrackingOriginTransform.position;
			}
		}

		//-------------------------------------------------
		// Guess for the world-space direction of the player's hips/torso. This is effectively just the gaze direction projected onto the floor plane.
		//-------------------------------------------------
		public Vector3 BodyDirectionGuess
		{
			get
			{
				Transform hmd = HmdTransform;
				if ( hmd )
				{
					Vector3 direction = Vector3.ProjectOnPlane( hmd.forward, TrackingOriginTransform.up );
					if ( Vector3.Dot( hmd.up, TrackingOriginTransform.up ) < 0.0f )
					{
						// The HMD is upside-down. Either
						// -The player is bending over backwards
						// -The player is bent over looking through their legs
						direction = -direction;
					}
					return direction;
				}
				return TrackingOriginTransform.forward;
			}
		}

		//-------------------------------------------------
		private void Awake()
		{
            SteamVR.Initialize(true); //force openvr

			if ( TrackingOriginTransform == null )
			{
				TrackingOriginTransform = this.transform;
			}
		}

		//-------------------------------------------------
		private IEnumerator Start()
		{
			instance = this;

            while (SteamVR_Behaviour.instance.forcingInitialization)
                yield return null;

			if ( SteamVR.instance != null )
			{
				ActivateRig( RigSteamVR );
			}
			else
			{
#if !HIDE_DEBUG_UI
				ActivateRig( Rig2DFallback );
#endif
			}
		}

		//-------------------------------------------------
		private void OnDrawGizmos()
		{
			if ( this != Instance )
			{
				return;
			}

			//NOTE: These gizmo icons don't work in the plugin since the icons need to exist in a specific "Gizmos"
			//		folder in your Asset tree. These icons are included under Core/Icons. Moving them into a
			//		"Gizmos" folder should make them work again.

			Gizmos.color = Color.white;
			Gizmos.DrawIcon( FeetPositionGuess, "vr_interaction_system_feet.png" );

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine( FeetPositionGuess, FeetPositionGuess + TrackingOriginTransform.up * EyeHeight );

			// Body direction arrow
			Gizmos.color = Color.blue;
			Vector3 bodyDirection = BodyDirectionGuess;
			Vector3 bodyDirectionTangent = Vector3.Cross( TrackingOriginTransform.up, bodyDirection );
			Vector3 startForward = FeetPositionGuess + TrackingOriginTransform.up * EyeHeight * 0.75f;
			Vector3 endForward = startForward + bodyDirection * 0.33f;
			Gizmos.DrawLine( startForward, endForward );
			Gizmos.DrawLine( endForward, endForward - 0.033f * ( bodyDirection + bodyDirectionTangent ) );
			Gizmos.DrawLine( endForward, endForward - 0.033f * ( bodyDirection - bodyDirectionTangent ) );

			Gizmos.color = Color.red;
			int count = HandCount;
			for ( int i = 0; i < count; i++ )
			{
                Hand hand = GetHand(i);

				if ( hand.HandType == SteamVR_Input_Sources.LeftHand)
				{
					Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_left_hand.png" );
				}
				else if ( hand.HandType == SteamVR_Input_Sources.RightHand)
				{
					Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_right_hand.png" );
				}
				else
				{
                    /*
					Hand.HandType guessHandType = hand.currentHandType;

					if ( guessHandType == Hand.HandType.Left )
					{
						Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_left_hand_question.png" );
					}
					else if ( guessHandType == Hand.HandType.Right )
					{
						Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_right_hand_question.png" );
					}
					else
					{
						Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_unknown_hand.png" );
					}
                    */
				}
			}
		}

		//-------------------------------------------------
		public void Draw2DDebug()
		{
			if ( !AllowToggleTo2D )
				return;

			if ( !SteamVR.active )
				return;

			int width = 100;
			int height = 25;
			int left = Screen.width / 2 - width / 2;
			int top = Screen.height - height - 10;

            string text = (RigSteamVR.activeSelf) ? "2D Debug" : "VR";

            if (GUI.Button(new Rect(left, top, width, height), text))
			{
                if (RigSteamVR.activeSelf) 
				{
                    ActivateRig(Rig2DFallback);
				}
				else
				{
                    ActivateRig(RigSteamVR);
				}
			}
		}

		//-------------------------------------------------
		private void ActivateRig( GameObject rig )
		{
			RigSteamVR.SetActive( rig == RigSteamVR );
			Rig2DFallback.SetActive( rig == Rig2DFallback );

			if ( AudioListener )
			{
				AudioListener.transform.parent = HmdTransform;
				AudioListener.transform.localPosition = Vector3.zero;
				AudioListener.transform.localRotation = Quaternion.identity;
			}
		}

		//-------------------------------------------------
		public void PlayerShotSelf()
		{
			//Do something appropriate here
		}
	}
}
