// ======================================================================================
// File         : exTimebasedCurveEditor.cs
// Author       : Wu Jie 
// Last Change  : 07/20/2011 | 14:47:07 PM | Wednesday,July
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

[CustomEditor(typeof(exTimebasedCurveInfo))]
[CanEditMultipleObjects]
public class exTimebasedCurveEditor : Editor {

    SerializedProperty wrapModeProp;
    SerializedProperty lengthProp;
    SerializedProperty useEaseCurveProp;
    SerializedProperty easeCurveTypeProp;
    SerializedProperty animationCurveProp;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    [MenuItem ("Assets/Create/ex/Timebased Curve Info")]
    public static void CreateAsset () {
        exGenericAssetUtility<exTimebasedCurveInfo>.CreateInCurrentDirectory("New CurveInfo");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnEnable () {
        wrapModeProp = serializedObject.FindProperty ("wrapMode");
        lengthProp = serializedObject.FindProperty ("length");
        useEaseCurveProp = serializedObject.FindProperty ("useEaseCurve");
        easeCurveTypeProp = serializedObject.FindProperty ("easeCurveType");
        animationCurveProp = serializedObject.FindProperty ("animationCurve");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public override void OnInspectorGUI () {
        serializedObject.Update ();

            EditorGUILayout.PropertyField (wrapModeProp);
            EditorGUILayout.PropertyField (lengthProp);
            EditorGUILayout.PropertyField (useEaseCurveProp);

            GUI.enabled = useEaseCurveProp.boolValue;
            EditorGUILayout.PropertyField (easeCurveTypeProp);

            GUI.enabled = !useEaseCurveProp.boolValue;
            EditorGUILayout.PropertyField (animationCurveProp);

        serializedObject.ApplyModifiedProperties ();
    }
}
