// ======================================================================================
// File         : exGenericComponentDebugger.cs
// Author       : Wu Jie 
// Last Change  : 04/02/2012 | 00:47:18 AM | Monday,April
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
///
/// the unity component debugger
///
///////////////////////////////////////////////////////////////////////////////

class exGenericComponentDebugger<T> : EditorWindow 
    where T : Component
{

    ///////////////////////////////////////////////////////////////////////////////
    // members
    ///////////////////////////////////////////////////////////////////////////////

    public bool lockSelection = false;

    protected T curEdit = null;
    protected GUIStyle textStyle = new GUIStyle();

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void OnEnable () {
        textStyle.alignment = TextAnchor.MiddleLeft;
        name = "Generic Component Debugger";
        wantsMouseMove = false;
        autoRepaintOnSceneChange = true;
        // position = new Rect ( 50, 50, 800, 600 );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void Init () {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Debug ( Object _obj ) {
        GameObject go = _obj as GameObject;
        if ( go == null ) {
            return;
        }
        T comp = go.GetComponent<T>();
        if ( comp == null )
            return;

        // check if repaint
        if ( curEdit != comp ) {
            curEdit = comp;
            Init();

            Repaint ();
            return;
        }
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnSelectionChange () {
        if ( curEdit == null || lockSelection == false ) {
            GameObject go = Selection.activeGameObject;
            if ( go ) {
                Debug (go);
            }
        }
        Repaint ();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnGUI () {

        EditorGUI.indentLevel = 0;

        // ======================================================== 
        // check if selection valid 
        // ======================================================== 

        //
        if ( curEdit == null ) {
            GUILayout.Space(10);
            GUILayout.Label ( "Please select a GameObject for editing" );
            return;
        }

        // ======================================================== 
        // toolbar 
        // ======================================================== 

        GUILayout.BeginHorizontal ( EditorStyles.toolbar );

            // ======================================================== 
            // update button
            // ======================================================== 

            GUILayout.FlexibleSpace();

            // ======================================================== 
            // lock button
            // ======================================================== 

            lockSelection = GUILayout.Toggle ( lockSelection, "Lock", EditorStyles.toolbarButton );
        GUILayout.EndHorizontal ();

        GUILayout.Space(5);

        // ======================================================== 
        // 
        // ======================================================== 

        ShowDebugInfo();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void ShowDebugInfo () {
    } 
}
