// ======================================================================================
// File         : exFSMDebugger.cs
// Author       : Wu Jie 
// Last Change  : 04/02/2012 | 00:44:48 AM | Monday,April
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

class exFSMDebugger : EditorWindow {

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
    // NOTE: you should inherit the exFSMDebugger and override this
    // ------------------------------------------------------------------ 

    protected virtual fsm.Machine GetStateMachine ( GameObject _go ) {
        FSMBase fsm = _go.GetComponent<FSMBase>();
        if ( fsm != null )
            return fsm.stateMachine;
        return null;
    }

    // ------------------------------------------------------------------ 
    /// \return the editor
    /// Open the animation debug window
    // ------------------------------------------------------------------ 

    [MenuItem ("ex/Debugger/FSM Debugger")]
    public static exFSMDebugger NewWindow () {
        exFSMDebugger newWindow = EditorWindow.GetWindow<exFSMDebugger>();
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
        GameObject go = _obj as GameObject;
        if ( go == null ) {
            return;
        }

        //
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
