using System;
using System.Collections.Generic;
using UnityEngine;

namespace ubco.ovilab.HPUI.Cone
{
    /// <summary>
    /// Collects calibration data while a target cone segment is selected manually.
    /// </summary>
    public class GuidedDataCollector : RaycastDataCollectorBase
    {
        [SerializeField]
        [Tooltip("Cone segment currently being calibrated.")]
        private HPUIInteractorConeRayAngleSegment targetSegment;

        /// <summary>
        /// Gets or sets the cone segment to which newly collected calibration data is assigned.
        /// When a custom calibration order is configured, the value must be included in that order.
        /// </summary>
        public HPUIInteractorConeRayAngleSegment TargetSegment
        {
            get => targetSegment;
            set
            {
                if (orderOfCalibration != null && orderOfCalibration.Count > 0)
                {
                    if (orderOfCalibration.Contains(value))
                    {
                        targetSegment = value;
                    }
                    else
                    {
                        Debug.LogError($"Attempted to set TargetSegment to a value not in OrderOfCalibration: {value}");
                    }
                }
                else
                {
                    targetSegment = value;
                }
            }
        }

        [SerializeField]
        [Tooltip("Keep only one calibration record for each cone segment when enabled.")]
        private bool uniqueDataRecordPerPhalange = true;

        /// <summary>
        /// Gets or sets whether a new calibration replaces an existing record for the same segment.
        /// When disabled, multiple records can be collected for a segment.
        /// </summary>
        public bool UniqueDataRecordPerPhalange { get => uniqueDataRecordPerPhalange; set => uniqueDataRecordPerPhalange = value; }

        [SerializeField]
        [Tooltip("Optional order in which cone segments are calibrated.")]
        private List<HPUIInteractorConeRayAngleSegment> orderOfCalibration;

        /// <summary>
        /// Gets the optional custom order used when stepping through calibration segments.
        /// If the list is empty, all cone segments can be selected directly.
        /// </summary>
        public List<HPUIInteractorConeRayAngleSegment> OrderOfCalibration { get => orderOfCalibration; }

        private int currentPhalangeIndex;

        /// <summary>
        /// Stores the currently buffered samples as a <see cref="ConeRayComputationDataRecord"/>
        /// for the specified segment and clears the sample buffer.
        /// </summary>
        /// <param name="segment">The segment to associate with the buffered samples.</param>
        public void EndCalibrationForSegment(HPUIInteractorConeRayAngleSegment segment)
        {
            if (uniqueDataRecordPerPhalange)
            {
                foreach (ConeRayComputationDataRecord dataRecord in DataRecords)
                {
                    if (dataRecord.segment == segment)
                    {
                        DataRecords.Remove(dataRecord);
                        break;
                    }
                }
            }

            DataRecords.Add(new ConeRayComputationDataRecord(currentInteractionData, segment));

            currentInteractionData = new();
        }

        /// <summary>
        /// Stores the buffered samples for the current <see cref="TargetSegment"/>
        /// and pauses collection until it is resumed for the next segment.
        /// </summary>
        public void EndDataCollectionForTargetSegment()
        {
            EndCalibrationForSegment(TargetSegment);
            PauseDataCollection = true;
        }

        /// <summary>
        /// Resumes collection after <see cref="EndDataCollectionForTargetSegment"/>
        /// has completed the current segment.
        /// </summary>
        public void StartDataCollectionForNextTargetSegment()
        {
            PauseDataCollection = false;
        }

        /// <summary>
        /// Moves the target to another entry in <see cref="OrderOfCalibration"/>
        /// using modular indexing, allowing forward or backward steps.
        /// </summary>
        /// <param name="stepCount">The number of entries to move.</param>
        public void StepThroughCustomPhalanges(int stepCount = 1)
        {
            currentPhalangeIndex = (currentPhalangeIndex + stepCount) % OrderOfCalibration.Count;
            if (currentPhalangeIndex < 0)
            {
                currentPhalangeIndex += OrderOfCalibration.Count;
            }
            HPUIInteractorConeRayAngleSegment currentTargetSegment = OrderOfCalibration[currentPhalangeIndex];
            TargetSegment = currentTargetSegment;
        }

        /// <summary>
        /// Moves the target through the complete <see cref="HPUIInteractorConeRayAngleSegment"/>
        /// enumeration, wrapping at either end.
        /// </summary>
        /// <param name="stepCount">The number of segments to move.</param>
        public void StepThroughAllPhalanges(int stepCount = 1)
        {
            int phalangeCount = Enum.GetNames(typeof(HPUIInteractorConeRayAngleSegment)).Length;
            int targetSegmentIndex = Array.IndexOf(Enum.GetValues(typeof(HPUIInteractorConeRayAngleSegment)), TargetSegment);
            if (stepCount > 0)
            {
                if (targetSegmentIndex < phalangeCount - 1)
                {
                    TargetSegment = (HPUIInteractorConeRayAngleSegment)targetSegmentIndex + stepCount;
                }
                else
                {
                    TargetSegment = 0;
                }
            }
            else
            {
                if (targetSegmentIndex == 0)
                {
                    TargetSegment = (HPUIInteractorConeRayAngleSegment)phalangeCount - 1;
                }
                else
                {
                    TargetSegment = (HPUIInteractorConeRayAngleSegment)targetSegmentIndex + stepCount;
                }
            }
        }
    }
}
