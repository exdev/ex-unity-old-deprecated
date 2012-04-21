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

class exAnimationDebugger : exGenericComponentDebugger<Animation> {

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

    protected new void OnEnable () {
        base.OnEnable();
        name = "Animation Debugger";
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void ShowDebugInfo () {
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
