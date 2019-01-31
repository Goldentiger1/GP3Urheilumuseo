//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class SkeletonUIOptions : MonoBehaviour
    {

        public void AnimateHandWithController()
        {
            for (int handIndex = 0; handIndex < Player.Instance.Hands.Length; handIndex++)
            {
                Hand hand = Player.Instance.Hands[handIndex];
                if (hand != null)
                {
                    hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
                }
            }
        }

        public void AnimateHandWithoutController()
        {
            for (int handIndex = 0; handIndex < Player.Instance.Hands.Length; handIndex++)
            {
                Hand hand = Player.Instance.Hands[handIndex];
                if (hand != null)
                {
                    hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
                }
            }
        }

        public void ShowController()
        {
            for (int handIndex = 0; handIndex < Player.Instance.Hands.Length; handIndex++)
            {
                Hand hand = Player.Instance.Hands[handIndex];
                if (hand != null)
                {
                    hand.ShowController(true);
                }
            }
        }

        public void SetRenderModel(RenderModelHolder prefabs)
        {
            for (int handIndex = 0; handIndex < Player.Instance.Hands.Length; handIndex++)
            {
                Hand hand = Player.Instance.Hands[handIndex];
                if (hand != null)
                {
                    if (hand.HandType == SteamVR_Input_Sources.RightHand)
                        hand.SetRenderModel(prefabs.rightPrefab);
                    if (hand.HandType == SteamVR_Input_Sources.LeftHand)
                        hand.SetRenderModel(prefabs.leftPrefab);
                }
            }
        }

        public void HideController()
        {
            for (int handIndex = 0; handIndex < Player.Instance.Hands.Length; handIndex++)
            {
                Hand hand = Player.Instance.Hands[handIndex];
                if (hand != null)
                {
                    hand.HideController(true);
                }
            }
        }
    }
}