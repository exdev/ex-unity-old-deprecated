// ======================================================================================
// File         : exDebugHelperEditor.cs
// Author       : Wu Jie 
// Last Change  : 11/25/2011 | 23:49:23 PM | Friday,November
// Description  : 
// ======================================================================================

#define EX2D

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

[CustomEditor(typeof(exDebugHelper))]
public class exDebugHelperEditor : Editor {

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    exDebugHelper curEdit;
#if EX2D
    SerializedProperty debugTextPoolProp;
    SerializedProperty txtPrintProp;
    SerializedProperty txtFPSProp;
    SerializedProperty txtLogProp;
    SerializedProperty txtTimeScaleProp;
#else
    SerializedProperty printStyleProp;
    SerializedProperty fpsStyleProp;
    SerializedProperty logStyleProp;
    SerializedProperty timeScaleStyleProp;
#endif

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnEnable () {
        if ( target != curEdit ) {
            curEdit = target as exDebugHelper;
        }
#if EX2D
        debugTextPoolProp = serializedObject.FindProperty ("debugTextPool");
        txtPrintProp = serializedObject.FindProperty ("txtPrint");
        txtFPSProp = serializedObject.FindProperty ("txtFPS");
        txtLogProp = serializedObject.FindProperty ("txtLog");
        txtTimeScaleProp = serializedObject.FindProperty ("txtTimeScale");
#else
        printStyleProp = serializedObject.FindProperty ("printStyle");
        fpsStyleProp = serializedObject.FindProperty ("fpsStyle");
        logStyleProp = serializedObject.FindProperty ("logStyle");
        timeScaleStyleProp = serializedObject.FindProperty ("timeScaleStyle");
#endif
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    override public void OnInspectorGUI () {

        EditorGUIUtility.LookLikeInspector ();
        EditorGUILayout.Space ();
        EditorGUI.indentLevel = 0;

        // ======================================================== 
        // pool settings 
        // ======================================================== 

        serializedObject.Update();
#if EX2D
            if ( EditorGUILayout.PropertyField ( debugTextPoolProp, new GUIContent("Debug Text Pool") ) ) 
            {
                EditorGUI.indentLevel = 1;
                curEdit.debugTextPool.prefab = EditorGUILayout.ObjectField( "Prefab"
                                                                            , curEdit.debugTextPool.prefab
                                                                            , typeof(GameObject)
                                                                            , true 
                                                                          ) as GameObject;
                curEdit.debugTextPool.size = EditorGUILayout.IntField( "Size", curEdit.debugTextPool.size );
                EditorGUI.indentLevel = 0;
            }

            EditorGUILayout.PropertyField (txtPrintProp);
            EditorGUILayout.PropertyField (txtFPSProp);
            EditorGUILayout.PropertyField (txtLogProp);
            EditorGUILayout.PropertyField (txtTimeScaleProp);
#else
            EditorGUILayout.PropertyField (printStyleProp, true);
            EditorGUILayout.PropertyField (fpsStyleProp, true);
            EditorGUILayout.PropertyField (logStyleProp, true);
            EditorGUILayout.PropertyField (timeScaleStyleProp, true);
#endif
        serializedObject.ApplyModifiedProperties();

        curEdit.showFps = EditorGUILayout.Toggle( "Show Fps", curEdit.showFps );
        curEdit.showScreenPrint = EditorGUILayout.Toggle( "Show Screen Print", curEdit.showScreenPrint );
        curEdit.showScreenLog = EditorGUILayout.Toggle( "Show Screen Log", curEdit.showScreenLog );
        curEdit.showScreenDebugText = EditorGUILayout.Toggle( "Show Screen Debug Text", curEdit.showScreenDebugText );
        curEdit.enableTimeScaleDebug = EditorGUILayout.Toggle( "Enable Time Scale Debug", curEdit.enableTimeScaleDebug );

        // ======================================================== 
        // check dirty 
        // ======================================================== 

        if ( GUI.changed ) {
            EditorUtility.SetDirty(curEdit);
        }
    }
}

