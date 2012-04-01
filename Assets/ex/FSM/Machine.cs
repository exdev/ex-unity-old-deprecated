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

        public delegate void OnEventHandler ();

        // DEBUG { 
        public bool showDebugInfo = true;
        public bool logDebugInfo = false;
        // } DEBUG end 

        ///////////////////////////////////////////////////////////////////////////////
        // public
        ///////////////////////////////////////////////////////////////////////////////

        public OnEventHandler onStart;
        public OnEventHandler onStop;

        ///////////////////////////////////////////////////////////////////////////////
        // non-serializable
        ///////////////////////////////////////////////////////////////////////////////

        protected MachineState machineState = MachineState.Stopped;

        protected State startState = new State( "fsm_start" ); // NOTE: startState is different than initState, startState --transition--> initState 
        protected List<Event>[] eventBuffer = new List<Event>[2] { new List<Event>(), new List<Event>() };
        protected int curEventBufferIdx = 0; 
        protected int nextEventBufferIdx = 1; 
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

        public void Start () {
            if ( machineState == MachineState.Running ||
                 machineState == MachineState.Paused )
                return;

            machineState = MachineState.Running;
            if ( onStart != null )
                onStart ();

            Event nullEvent = new Event( Event.NULL );  
            if ( mode == State.Mode.Exclusive ) {
                if ( initState != null ) {
                    EnterStates( nullEvent, initState, startState );
                }
                else {
                    Debug.LogError( "FSM error: can't find initial state in " + name );
                }
            }
            else { // if ( _toEnter.mode == State.Mode.Parallel )
                foreach ( State child in children ) {
                    EnterStates( nullEvent, child, startState );
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

        protected void ProcessStop () {
            eventBuffer[0].Clear();
            eventBuffer[1].Clear();
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
                // now switch event list
                int tmp = curEventBufferIdx;
                curEventBufferIdx = nextEventBufferIdx;
                nextEventBufferIdx = tmp;

                //
                bool doStop = false;
                List<Event> eventList = eventBuffer[curEventBufferIdx];
                // Debug.Log( "eventList [" + curEventBufferIdx + "] = " + eventList.Count );
                foreach ( Event ent in eventList ) {
                    // if we can stop the machine, ignore rest events and do stop
                    if ( HandleEvent (ent) ) {
                        doStop = true;
                        break;
                    }
                }
                eventList.Clear();

                // on event in current states 
                if ( doStop ) {
                    Stop ();
                }
                else {
                    OnAction ();
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

        public void Pause () { machineState = MachineState.Paused; }
        public void Resume () { machineState = MachineState.Running; }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected bool HandleEvent ( Event _event ) {
            // on event in current states 
            OnEvent (_event);

            // 
            List<Transition> validTransitions = new List<Transition>(); 
            TestTransitions ( ref validTransitions, _event );

            //
            ExitStates ( _event, validTransitions ); // invoke State.OnExit
            ExecTransitions ( _event, validTransitions ); // invoke Transition.OnTransition
            EnterStates ( _event, validTransitions ); // invoke State.OnEnter

            // check if we need to stop the stateMachine
            if ( _event.id == Event.FINISHED ) {
                bool canStop = true;
                foreach ( State state in currentStates ) {
                    if ( (state is FinalState) == false ) {
                        canStop = false;
                        break;
                    }
                }
                if ( canStop ) {
                    return true;
                }
            }

            return false;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Send ( int _eventID ) { Send ( new Event(_eventID) ); }
        public void Send ( Event _event ) { 
            if ( machineState == MachineState.Stopped )
                return;
            eventBuffer[nextEventBufferIdx].Add (_event); 
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void EnterStates ( Event _event, List<Transition> _transitionList ) {
            foreach ( Transition transition in _transitionList ) {
                State targetState = transition.target;
                if ( targetState == null )
                    targetState = transition.source;
                targetState.parent.EnterStates ( _event, targetState, transition.source );
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void ExitStates ( Event _event, List<Transition> _transitionList ) {
            foreach ( Transition transition in _transitionList ) {
                transition.source.parent.ExitStates ( _event, transition.target, transition.source );
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void ExecTransitions ( Event _event, List<Transition> _transitionList ) {
            foreach ( Transition transition in _transitionList ) {
                transition.OnTransition (_event);
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
                    y = ShowDebugInfo ( x, y, 0, true );
                }
        }
    }
}
