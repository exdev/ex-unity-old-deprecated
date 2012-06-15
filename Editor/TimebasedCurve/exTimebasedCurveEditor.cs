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
public class exTimebasedCurveEditor : Editor {

    private exTimebasedCurveInfo curEditTarget;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public override void OnInspectorGUI () {
        //
        if ( target != curEditTarget ) {
            curEditTarget = target as exTimebasedCurveInfo;
        }

        EditorGUIUtility.LookLikeInspector ();
        EditorGUI.indentLevel = 1;

        // ======================================================== 
        // wrap mode 
        // ======================================================== 

        curEditTarget.wrapMode = 
            (exTimebasedCurveInfo.WrapMode)EditorGUILayout.EnumPopup( "Wrap Mode", curEditTarget.wrapMode );

        // ======================================================== 
        // length 
        // ======================================================== 

        curEditTarget.length = EditorGUILayout.FloatField( "Length", curEditTarget.length );

        // ======================================================== 
        // use ease curve 
        // ======================================================== 

        curEditTarget.useEaseCurve = EditorGUILayout.Toggle( "Use exEase Curve", curEditTarget.useEaseCurve );
        GUI.enabled = curEditTarget.useEaseCurve;
        EditorGUI.indentLevel = 2;
        curEditTarget.easeCurveType = (exEase.Type)EditorGUILayout.EnumPopup( "exEase Curve Type", curEditTarget.easeCurveType );
        EditorGUI.indentLevel = 1;
        GUI.enabled = true;

        // ======================================================== 
        // animation curve
        // ======================================================== 

        GUI.enabled = !curEditTarget.useEaseCurve;
        curEditTarget.animationCurve = EditorGUILayout.CurveField( "Animation Curve", curEditTarget.animationCurve );
        GUI.enabled = true;

        // ======================================================== 
        // set dirty if anything changed
        // ======================================================== 

        if ( GUI.changed ) {
            EditorUtility.SetDirty(curEditTarget);
        }
    }
}
