// ======================================================================================
// File         : exDebugHelperEditor.cs
// Author       : Wu Jie 
// Last Change  : 11/25/2011 | 23:49:23 PM | Friday,November
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

[CustomEditor(typeof(exDebugHelper))]
public class exDebugHelperEditor : Editor {

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    exDebugHelper curEdit;
    SerializedProperty propDebugTextPool;

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
        propDebugTextPool = serializedObject.FindProperty ("debugTextPool");
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
        if ( EditorGUILayout.PropertyField ( propDebugTextPool, new GUIContent("Debug Text Pool") ) ) 
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
        serializedObject.ApplyModifiedProperties();

        // ======================================================== 
        // text print
        // ======================================================== 

        curEdit.txtPrint = (exSpriteFont)EditorGUILayout.ObjectField( "Text Print"
                                                                      , curEdit.txtPrint
                                                                      , typeof(exSpriteFont)
                                                                      , false 
                                                                    );

        // ======================================================== 
        // text FPS 
        // ======================================================== 

        curEdit.txtFPS = (exSpriteFont)EditorGUILayout.ObjectField( "Text FPS"
                                                                    , curEdit.txtFPS
                                                                    , typeof(exSpriteFont)
                                                                    , false 
                                                                  );

        // ======================================================== 
        // text Log 
        // ======================================================== 

        curEdit.txtLog = (exSpriteFont)EditorGUILayout.ObjectField( "Text Log"
                                                                    , curEdit.txtLog
                                                                    , typeof(exSpriteFont)
                                                                    , false 
                                                                  );


        curEdit.showFps = EditorGUILayout.Toggle( "Show Fps", curEdit.showFps );
        curEdit.showScreenPrint = EditorGUILayout.Toggle( "Show Screen Print", curEdit.showScreenPrint );
        curEdit.showScreenLog = EditorGUILayout.Toggle( "Show Screen Log", curEdit.showScreenLog );
        curEdit.showScreenDebugText = EditorGUILayout.Toggle( "Show Screen Debug Text", curEdit.showScreenDebugText );

        // ======================================================== 
        // check dirty 
        // ======================================================== 

        if ( GUI.changed ) {
            EditorUtility.SetDirty(curEdit);
        }
	}
}

