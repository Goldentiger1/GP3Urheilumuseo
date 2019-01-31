//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Displays 2d content on a large virtual screen.
//
//=============================================================================

using UnityEngine;

namespace Valve.VR
{
    public class SteamVR_Overlay : MonoBehaviour
    {
        public Texture Texture;
        public bool Curved = true;
        public bool Antialias = true;
        public bool Highquality = true;

        [Tooltip("Size of overlay view.")]
        public float Scale = 3.0f;

        [Tooltip("Distance from surface.")]
        public float Distance = 1.25f;

        [Tooltip("Opacity"), Range(0.0f, 1.0f)]
        public float Alpha = 1.0f;

        public Vector4 UvOffset = new Vector4(0, 0, 1, 1);
        public Vector2 MouseScale = new Vector2(1, 1);
        public Vector2 CurvedRange = new Vector2(1, 2);

        public VROverlayInputMethod InputMethod = VROverlayInputMethod.None;

        public static SteamVR_Overlay Instance { get; private set; }

        public static string Key { get { return "unity:" + Application.companyName + "." + Application.productName; } }

        private ulong handle = OpenVR.k_ulOverlayHandleInvalid;

        private void OnEnable()
        {
            var overlay = OpenVR.Overlay;
            if (overlay != null)
            {
                var error = overlay.CreateOverlay(Key, gameObject.name, ref handle);
                if (error != EVROverlayError.None)
                {
                    Debug.Log(overlay.GetOverlayErrorNameFromEnum(error));
                    enabled = false;
                    return;
                }
            }

            Instance = this;
        }

        private void OnDisable()
        {
            if (handle != OpenVR.k_ulOverlayHandleInvalid)
            {
                var overlay = OpenVR.Overlay;
                if (overlay != null)
                {
                    overlay.DestroyOverlay(handle);
                }

                handle = OpenVR.k_ulOverlayHandleInvalid;
            }

            Instance = null;
        }

        public void UpdateOverlay()
        {
            var overlay = OpenVR.Overlay;
            if (overlay == null)
                return;

            if (Texture != null)
            {
                var error = overlay.ShowOverlay(handle);
                if (error == EVROverlayError.InvalidHandle || error == EVROverlayError.UnknownOverlay)
                {
                    if (overlay.FindOverlay(Key, ref handle) != EVROverlayError.None)
                        return;
                }

                var tex = new Texture_t
                {
                    handle = Texture.GetNativeTexturePtr(),
                    eType = SteamVR.instance.textureType,
                    eColorSpace = EColorSpace.Auto
                };
                overlay.SetOverlayTexture(handle, ref tex);

                overlay.SetOverlayAlpha(handle, Alpha);
                overlay.SetOverlayWidthInMeters(handle, Scale);
                overlay.SetOverlayAutoCurveDistanceRangeInMeters(handle, CurvedRange.x, CurvedRange.y);

                var textureBounds = new VRTextureBounds_t
                {
                    uMin = (0 + UvOffset.x) * UvOffset.z,
                    vMin = (1 + UvOffset.y) * UvOffset.w,
                    uMax = (1 + UvOffset.x) * UvOffset.z,
                    vMax = (0 + UvOffset.y) * UvOffset.w
                };
                overlay.SetOverlayTextureBounds(handle, ref textureBounds);

                var vecMouseScale = new HmdVector2_t
                {
                    v0 = MouseScale.x,
                    v1 = MouseScale.y
                };
                overlay.SetOverlayMouseScale(handle, ref vecMouseScale);

                var vrcam = SteamVR_Render.Top();
                if (vrcam != null && vrcam.origin != null)
                {
                    var offset = new SteamVR_Utils.RigidTransform(vrcam.origin, transform);
                    offset.pos.x /= vrcam.origin.localScale.x;
                    offset.pos.y /= vrcam.origin.localScale.y;
                    offset.pos.z /= vrcam.origin.localScale.z;

                    offset.pos.z += Distance;

                    var t = offset.ToHmdMatrix34();
                    overlay.SetOverlayTransformAbsolute(handle, SteamVR.settings.trackingSpace, ref t);
                }

                overlay.SetOverlayInputMethod(handle, InputMethod);

                if (Curved || Antialias)
                    Highquality = true;

                if (Highquality)
                {
                    overlay.SetHighQualityOverlay(handle);
                    overlay.SetOverlayFlag(handle, VROverlayFlags.Curved, Curved);
                    overlay.SetOverlayFlag(handle, VROverlayFlags.RGSS4X, Antialias);
                }
                else if (overlay.GetHighQualityOverlay() == handle)
                {
                    overlay.SetHighQualityOverlay(OpenVR.k_ulOverlayHandleInvalid);
                }
            }
            else
            {
                overlay.HideOverlay(handle);
            }
        }

        public bool PollNextEvent(ref VREvent_t pEvent)
        {
            var overlay = OpenVR.Overlay;
            if (overlay == null)
                return false;

            var size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(Valve.VR.VREvent_t));
            return overlay.PollNextOverlayEvent(handle, ref pEvent, size);
        }

        public struct IntersectionResults
        {
            public Vector3 point;
            public Vector3 normal;
            public Vector2 UVs;
            public float distance;
        }

        public bool ComputeIntersection(Vector3 source, Vector3 direction, ref IntersectionResults results)
        {
            var overlay = OpenVR.Overlay;
            if (overlay == null)
                return false;

            var input = new VROverlayIntersectionParams_t
            {
                eOrigin = SteamVR.settings.trackingSpace
            };
            input.vSource.v0 = source.x;
            input.vSource.v1 = source.y;
            input.vSource.v2 = -source.z;
            input.vDirection.v0 = direction.x;
            input.vDirection.v1 = direction.y;
            input.vDirection.v2 = -direction.z;

            var output = new VROverlayIntersectionResults_t();
            if (!overlay.ComputeOverlayIntersection(handle, ref input, ref output))
                return false;

            results.point = new Vector3(output.vPoint.v0, output.vPoint.v1, -output.vPoint.v2);
            results.normal = new Vector3(output.vNormal.v0, output.vNormal.v1, -output.vNormal.v2);
            results.UVs = new Vector2(output.vUVs.v0, output.vUVs.v1);
            results.distance = output.fDistance;
            return true;
        }
    }
}