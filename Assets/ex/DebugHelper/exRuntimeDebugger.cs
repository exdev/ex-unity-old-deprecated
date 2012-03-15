// ======================================================================================
// File         : exRuntimeDebugger.cs
// Author       : Wu Jie 
// Last Change  : 02/19/2012 | 21:20:14 PM | Sunday,February
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

public class exRuntimeDebugger : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // properites
    ///////////////////////////////////////////////////////////////////////////////

    public bool debug = true;
    public bool debugAnimation = true;
    public bool debugStateMachine = true;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    protected fsm.Machine[] smList = new fsm.Machine[0];

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnGUI () {
        if ( debug == false )
            return;

        // if ( GUI.Button ( new Rect ( 2.0f, 2.0f, 200.0f, 22.0f ),  "Refresh FSM (Debug)" ) ) {
        //     smList = Object.FindObjectsOfType ( typeof (Creature) ) as Creature[];
        // }
        GUILayoutUtility.GetRect( 200.0f, 22.0f );

        if ( debugAnimation ) {
            ShowDebugAnimationGUI ();
        }
        if ( debugStateMachine ) {
            ShowDebugStateMachineGUI ();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ShowDebugAnimationGUI () {
        // GUILayout.Space(1.0f);
        // float y = GUILayoutUtility.GetLastRect().yMax + 5.0f;
        // foreach ( CreatureAnimation anim in creatureAnimList ) {
        //     y = anim.ShowDebugGUI ( 20.0f, y );
        // }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ShowDebugStateMachineGUI () {
        float x = Screen.width - 200.0f - 20.0f;
        float y = 0.0f;
        foreach ( fsm.Machine sm in smList ) {
            if ( sm != null )
                y = sm.ShowDebugGUI ( sm.name, x, y );
        }
    }
}
