using System.Collections.Generic;
using ubco.ovilab.HPUI.Core.Interaction;
using UnityEngine.XR.Hands;

namespace ubco.ovilab.HPUI.Cone
{
    public interface IConeRaySegmentComputation
    {
        /// <summary>
        /// Computes the ray angles for one cone segment from the supplied interaction records.
        /// </summary>
        /// <param name="segment">The cone segment for which angles are being computed.</param>
        /// <param name="interactionRecords">The recorded interaction data to analyze.</param>
        /// <returns>The estimated ray angles for <paramref name="segment"/>.</returns>
        List<HPUIInteractorRayAngle> EstimateConeAnglesForSegment(HPUIInteractorConeRayAngleSegment segment, IEnumerable<ConeRayComputationDataRecord> interactionRecords);
    }

    /// <summary>
    /// Associates the raycast frames collected during one gesture with the cone segment
    /// assigned to that gesture.
    /// </summary>
    public struct ConeRayComputationDataRecord
    {
        public List<RaycastDataRecordsContainer> records;
        public HPUIInteractorConeRayAngleSegment segment;

        public ConeRayComputationDataRecord(List<RaycastDataRecordsContainer> records, HPUIInteractorConeRayAngleSegment segment) : this()
        {
            this.records = records;
            this.segment = segment;
        }
    }

    /// <summary>
    /// Contains the raycast records collected during one frame, together with the closest
    /// finger side, hand joint, and collection timestamp.
    /// </summary>
    public struct RaycastDataRecordsContainer
    {
        public List<HPUIRayCastDetectionBaseLogic.RaycastDataRecord> raycastDataRecordsList;
        public FingerSide fingerSide;
        public XRHandJointID handJointID;

        /// <summary>
        /// Unscaled real time, in seconds since application startup, at which this frame was collected.
        /// </summary>
        public double timestampSeconds;

        public RaycastDataRecordsContainer(List<HPUIRayCastDetectionBaseLogic.RaycastDataRecord> raycastDataRecord, FingerSide fingerSide, XRHandJointID handJointID, double timestampSeconds) : this()
        {
            this.raycastDataRecordsList = raycastDataRecord;
            this.fingerSide = fingerSide;
            this.handJointID = handJointID;
            this.timestampSeconds = timestampSeconds;
        }
    }
}
