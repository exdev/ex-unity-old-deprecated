// ======================================================================================
// File         : exAnimationDebugger.cs
// Author       : Wu Jie 
// Last Change  : 04/01/2012 | 14:39:58 PM | Sunday,April
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

class exAnimationDebugger : EditorWindow {

    ///////////////////////////////////////////////////////////////////////////////
    // members
    ///////////////////////////////////////////////////////////////////////////////

    public bool lockSelection = false;

    private Animation curEdit = null;
    private GUIStyle textStyle = new GUIStyle();

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    /// \return the editor
    /// Open the animation debug window
    // ------------------------------------------------------------------ 

    [MenuItem ("ex/Debugger/Animation Debugger")]
    public static exAnimationDebugger NewWindow () {
        exAnimationDebugger newWindow = EditorWindow.GetWindow<exAnimationDebugger>();
        return newWindow;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnEnable () {
        textStyle.alignment = TextAnchor.MiddleLeft;
        name = "Animation Debugger";
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
        GameObject go = _obj as GameObject;
        Animation animComp = go.GetComponent<Animation>();
        if ( animComp == null )
            return;

        // check if repaint
        if ( curEdit != animComp ) {
            curEdit = animComp;
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

        foreach ( AnimationState state in curEdit ) {
            GUILayout.BeginHorizontal ();
                GUILayout.Space(5);
                textStyle.normal.textColor = state.enabled ? Color.green : new Color( 0.5f, 0.5f, 0.5f );
                GUILayout.Label ( "[" + state.layer + "]", textStyle, GUILayout.Width(30) );
                GUILayout.Label ( state.name, textStyle, new GUILayoutOption[] {} );
                GUILayout.Label ( state.weight.ToString("f3"), textStyle, GUILayout.Width(50) );
                GUILayout.Label ( state.speed.ToString("f3"), textStyle, GUILayout.Width(50) );
            GUILayout.EndHorizontal ();
        }
    }
}
