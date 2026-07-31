using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using System;
using System.Linq;
using ubco.ovilab.HPUI.Core.Interaction;

namespace ubco.ovilab.HPUI.Cone
{
    /// <summary>
    /// Contains the angles for the cone ray cast to be used with the <see cref="HPUIInteractor"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "HPUIInteractorConeRayAngles", menuName = "HPUI/HPUI Interactor Cone Ray Angles", order = 1)]
    public class HPUIInteractorConeRayAngles: ScriptableObject
    {
        [SerializeField, Tooltip("The side of the finger to fall back when a side is missing.")]
        private FingerSide fallbackSide = FingerSide.volar;

        /// <summary>
        /// The side of the finger to fall back when a side is missing.
        /// </summary>
        public FingerSide FallbackSide { get => fallbackSide; set => fallbackSide = value; }

        // These angle lists are populated from calibration data and may be generated
        // by ConeRayEstimator.
        public List<HPUIInteractorConeRayAngleSides> IndexDistalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> IndexIntermediateAngles = new();
        public List<HPUIInteractorConeRayAngleSides> IndexProximalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> MiddleDistalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> MiddleIntermediateAngles = new();
        public List<HPUIInteractorConeRayAngleSides> MiddleProximalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> RingDistalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> RingIntermediateAngles = new();
        public List<HPUIInteractorConeRayAngleSides> RingProximalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> LittleDistalAngles = new();
        public List<HPUIInteractorConeRayAngleSides> LittleIntermediateAngles = new();
        public List<HPUIInteractorConeRayAngleSides> LittleProximalAngles = new();

        private Dictionary<(XRHandJointID, FingerSide), List<HPUIInteractorRayAngle>> ActiveFingerAngles;

        public void OnEnable()
        {
            RefreshCache();
        }

        /// <summary>
        /// Rebuilds the lookup cache from the serialized angle lists.
        /// Call this after modifying the angle lists at runtime.
        /// </summary>
        public void RefreshCache()
        {
            ActiveFingerAngles = new();

            Dictionary<XRHandJointID, List<HPUIInteractorConeRayAngleSides>> jointToAnglesMapping = new()
            {
                { XRHandJointID.IndexProximal, IndexProximalAngles },
                { XRHandJointID.IndexIntermediate, IndexIntermediateAngles },
                { XRHandJointID.IndexDistal, IndexDistalAngles },
                { XRHandJointID.MiddleProximal, MiddleProximalAngles },
                { XRHandJointID.MiddleIntermediate, MiddleIntermediateAngles },
                { XRHandJointID.MiddleDistal, MiddleDistalAngles },
                { XRHandJointID.RingProximal, RingProximalAngles },
                { XRHandJointID.RingIntermediate, RingIntermediateAngles },
                { XRHandJointID.RingDistal, RingDistalAngles },
                { XRHandJointID.LittleProximal, LittleProximalAngles },
                { XRHandJointID.LittleIntermediate, LittleIntermediateAngles },
                { XRHandJointID.LittleDistal, LittleDistalAngles }
            };

            foreach (KeyValuePair<XRHandJointID, List<HPUIInteractorConeRayAngleSides>> kvp in jointToAnglesMapping)
            {
                foreach (HPUIInteractorConeRayAngleSides angleSide in kvp.Value)
                {
                    if (!ActiveFingerAngles.ContainsKey((kvp.Key, angleSide.side)))
                    {
                        List<HPUIInteractorRayAngle> angles = angleSide.rayAngles.Where(a => a.RaySelectionThreshold >= 0).ToList();
                        if (angleSide.rayAngles.Count != angles.Count)
                        {
                            Debug.Log($"Removed {angleSide.rayAngles.Count - angles.Count} rays as they had RaySelectionThreshold below 0.");
                        }
                        ActiveFingerAngles.Add((kvp.Key, angleSide.side), angles);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the ray angles for a hand joint and finger side.
        /// If no angles are configured for the requested side, the angles for
        /// <see cref="FallbackSide"/> will be returned if it exists.
        /// Otherwise, this returns <c>null</c>.
        /// </summary>
        public IReadOnlyList<HPUIInteractorRayAngle> GetAngles(XRHandJointID joint, FingerSide side)
        {
            List<HPUIInteractorRayAngle> angles;
            if (ActiveFingerAngles.TryGetValue((joint, side), out angles) && angles.Count > 0)
            {
                return angles.AsReadOnly();
            }
            if (ActiveFingerAngles.TryGetValue((joint, FallbackSide), out angles) && angles.Count > 0)
            {
                return angles.AsReadOnly();
            }
            return null;
        }

        /// <summary>
        /// Serialize the data as a json object.
        /// <seealso cref="LoadAndSaveConeDataFromJson"/>
        /// </summary>
        public string ToJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    /// <summary>
    /// Enum representing the sides of the finger
    /// </summary>
    public enum FingerSide
    {
        volar = 0,
        radial = 1
        // Ulnar and dorsal sides are not currently supported.
    }

    [Serializable]
    public struct HPUIInteractorConeRayAngleSides
    {
        /// <summary>
        /// The side of the finger the <see cref="rayAngles"/> corresponds to.
        /// </summary>
        public FingerSide side;

        /// <summary>
        /// The list of ray angles
        /// </summary>
        public List<HPUIInteractorRayAngle> rayAngles;

        public HPUIInteractorConeRayAngleSides(FingerSide side, List<HPUIInteractorRayAngle> rayAngles) : this()
        {
            this.side = side;
            this.rayAngles = rayAngles;
        }
    }
}

