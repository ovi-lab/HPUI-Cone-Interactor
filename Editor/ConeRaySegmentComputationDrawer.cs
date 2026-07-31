using UnityEngine;
using UnityEditor;

namespace ubco.ovilab.HPUI.Cone.Editor
{
    [CustomPropertyDrawer(typeof(ConeRaySegmentComputation))]
    public class ConeRaySegmentComputationDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 8;

            // Show percentile only if EstimateTechnique is Percentile
            SerializedProperty estimateTechniqueProp = property.FindPropertyRelative("estimateTechnique");
            if ((ConeRaySegmentComputation.Estimate)estimateTechniqueProp.enumValueIndex
                 == ConeRaySegmentComputation.Estimate.Percentile)
            {
                lines++; // show percentile
            }
            return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines-1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect lineRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            SerializedProperty longGestureThresholdSecondsProp = property.FindPropertyRelative("longGestureThresholdSeconds");
            SerializedProperty shortGestureWindowSecondsProp = property.FindPropertyRelative("shortGestureWindowSeconds");
            SerializedProperty estimateTechniqueProp = property.FindPropertyRelative("estimateTechnique");
            SerializedProperty minRayInteractionsThresholdProp = property.FindPropertyRelative("minRayInteractionsThreshold");
            SerializedProperty percentileProp = property.FindPropertyRelative("percentile");
            SerializedProperty multiplierProp = property.FindPropertyRelative("multiplier");

            SerializedProperty cullRaysByDistanceToCentroidProp = property.FindPropertyRelative("cullRaysByDistanceToCentroid");
            SerializedProperty cullingDistanceThresholdNormalizedProp = property.FindPropertyRelative("cullingDistanceThresholdNormalized");

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, longGestureThresholdSecondsProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, shortGestureWindowSecondsProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, estimateTechniqueProp);

            if ((ConeRaySegmentComputation.Estimate)estimateTechniqueProp.enumValueIndex
                == ConeRaySegmentComputation.Estimate.Percentile)
            {
                lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(lineRect, percentileProp);
            }

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, minRayInteractionsThresholdProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, multiplierProp);

            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, cullRaysByDistanceToCentroidProp);

            GUI.enabled = cullRaysByDistanceToCentroidProp.boolValue;
            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(lineRect, cullingDistanceThresholdNormalizedProp);
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
    }
}
