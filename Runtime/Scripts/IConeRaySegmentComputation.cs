using System.Collections.Generic;
using ubco.ovilab.HPUI.Core.Interaction;
using UnityEngine.XR.Hands;

namespace ubco.ovilab.HPUI.Cone
{
    public interface IConeRaySegmentComputation
    {
        /// <summary>
        /// For a given segment, computes the List of <see cref="HPUIInteractorRayAngle">.
        /// </summary>
        /// <param name="segment">
        ///   The <see cref="HPUIInteractorConeRayAngleSegment"/> for which cone
        ///   angles are being computed.
        /// </param>
        List<HPUIInteractorRayAngle> EstimateConeAnglesForSegment(HPUIInteractorConeRayAngleSegment segment, IEnumerable<ConeRayComputationDataRecord> interactionRecords);
    }

    /// <summary>
    /// Holds all the data collected for a single gesture event.
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
    /// Container for a list of <see cref="HPUIRayCastDetectionBaseLogic.RaycastDataRecord"/> emitted with
    /// <see cref="HPUIRayCastDetectionBaseLogic.raycastData"/>. Also contains the closest
    /// <see cref="FingerSide">side</see> and <see cref="XRHandJoint">joint</see>, and the time at which the
    /// frame was collected.
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
