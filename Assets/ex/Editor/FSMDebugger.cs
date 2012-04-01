// ======================================================================================
// File         : FSMDebugger.cs
// Author       : Wu Jie 
// Last Change  : 04/01/2012 | 14:33:33 PM | Sunday,April
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
/// the unity animation debugger
///
///////////////////////////////////////////////////////////////////////////////

class FSMDebugger : EditorWindow {

    ///////////////////////////////////////////////////////////////////////////////
    // members
    ///////////////////////////////////////////////////////////////////////////////

    public bool lockSelection = false;

    private fsm.Machine curEdit = null;
    private GameObject curGO = null;
    private GUIStyle textStyle = new GUIStyle();

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // NOTE: you should inherit the FSMDebugger and override this
    // ------------------------------------------------------------------ 

    protected virtual fsm.Machine GetStateMachine ( GameObject _go ) {
        return null;
    }

    // ------------------------------------------------------------------ 
    /// \return the editor
    /// Open the animation debug window
    // ------------------------------------------------------------------ 

    [MenuItem ("ex/Debugger/FSM Debugger")]
    public static FSMDebugger NewWindow () {
        FSMDebugger newWindow = EditorWindow.GetWindow<FSMDebugger>();
        return newWindow;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void OnEnable () {
        textStyle.alignment = TextAnchor.MiddleLeft;
        name = "FSM Debugger";
        wantsMouseMove = false;
        autoRepaintOnSceneChange = true;
        // position = new Rect ( 50, 50, 800, 600 );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Init () {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Debug ( Object _obj ) {
        if ( _obj is GameObject == false ) {
            return;
        }

        //
        GameObject go = _obj as GameObject;
        fsm.Machine machine = GetStateMachine (go);

        // check if repaint
        if ( curEdit != machine ) {
            curEdit = machine;
            curGO = go;
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

        if ( curEdit != null && curGO != null )
            curEdit.ShowDebugGUI( curGO.name, textStyle );
    }
}
