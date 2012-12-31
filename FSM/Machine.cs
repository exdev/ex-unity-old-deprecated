// ======================================================================================
// File         : Machine.cs
// Author       : Wu Jie 
// Last Change  : 12/20/2011 | 13:15:20 PM | Tuesday,December
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

namespace fsm {

    public class Machine : State {

        public enum MachineState {
            Running,
            Paused,
            Stopping,
            Stopped
        }

        // DEBUG { 
        public bool showDebugInfo = true;
        public bool logDebugInfo = false;
        // } DEBUG end 

        ///////////////////////////////////////////////////////////////////////////////
        // public
        ///////////////////////////////////////////////////////////////////////////////

        public System.Action onStart;
        public System.Action onStop;

        ///////////////////////////////////////////////////////////////////////////////
        // non-serializable
        ///////////////////////////////////////////////////////////////////////////////

        protected MachineState machineState = MachineState.Stopped;

        protected State startState = new State( "fsm_start" ); // NOTE: startState is different than initState, startState --transition--> initState 
        protected bool isUpdating = false; 

        ///////////////////////////////////////////////////////////////////////////////
        // functions
        ///////////////////////////////////////////////////////////////////////////////

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public Machine () 
            : base ( "fsm_state_machine" )
        {
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Restart () {
            Stop ();
            Start ();
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Start () {
            if ( machineState == MachineState.Running ||
                 machineState == MachineState.Paused )
                return;

            machineState = MachineState.Running;
            if ( onStart != null )
                onStart ();

            if ( mode == State.Mode.Exclusive ) {
                if ( initState != null ) {
                    EnterStates( initState, startState );
                }
                else {
                    Debug.LogError( "FSM error: can't find initial state in " + name );
                }
            }
            else { // if ( _toEnter.mode == State.Mode.Parallel )
                for ( int i = 0; i < children.Count; ++i ) {
                    EnterStates( children[i], startState );
                }
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Stop () {
            if ( machineState == MachineState.Stopped )
                return;

            if ( isUpdating ) {
                machineState = MachineState.Stopping;
            }
            else {
                ProcessStop ();
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Pause () { machineState = MachineState.Paused; }
        public void Resume () { machineState = MachineState.Running; }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void ProcessStop () {
            ClearCurrentStatesRecursively();

            if ( onStop != null )
                onStop ();

            machineState = MachineState.Stopped;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Update () {
            if ( machineState == MachineState.Paused ||
                 machineState == MachineState.Stopped )
                return;

            isUpdating = true;

            // update machine if it is not stopping
            if ( machineState != MachineState.Stopping ) {
                //
                bool doStop = false;

                //
                CheckConditions (); // invoke Transition.onCheck, also invoke State.onExit
                OnAction (); // invoke State.onAction
                UpdateTransitions (); // invoke Transition.onTransition, also invoke State.onEnter

                // on event in current states 
                if ( doStop ) {
                    Stop ();
                }
            }

            isUpdating = false;

            // check if we change the machine to stop during update
            if ( machineState == MachineState.Stopping ) {
                ProcessStop ();
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void ShowDebugGUI ( string _name, GUIStyle _textStyle ) {
            GUILayout.Label( "State Machine (" + _name + ")" );
            showDebugInfo = GUILayout.Toggle( showDebugInfo, "Show States" );
            logDebugInfo = GUILayout.Toggle( logDebugInfo, "Log States" );

            if ( showDebugInfo ) {
                ShowDebugInfo ( 0, true, _textStyle );
            }
        }
    }
}
