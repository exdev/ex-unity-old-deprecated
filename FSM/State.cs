// ======================================================================================
// File         : State.cs
// Author       : Wu Jie 
// Last Change  : 12/20/2011 | 11:51:04 AM | Tuesday,December
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

    ///////////////////////////////////////////////////////////////////////////////
    // State
    ///////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    public class State {

        public enum Mode {
            Exclusive,
            Parallel,
        }

        public delegate void TransitionEventHandler ( State _from, State _to, Event _event );
        public delegate void OnEventHandler ( State _curState, Event _event );
        public delegate void OnActionHandler ( State _curState );

        ///////////////////////////////////////////////////////////////////////////////
        // properties
        ///////////////////////////////////////////////////////////////////////////////

        public string name = "";

        protected State parent_ = null;
        public State parent {
            set {
                if ( parent_ != value ) {
                    State oldParent = parent_;

                    // check if it is parent layer or child
                    while ( parent_ != null ) {
                        if ( parent_ == this ) {
                            Debug.LogWarning("can't add self or child as parent");
                            return;
                        } 
                        parent_ = parent_.parent;
                    }

                    //
                    if ( oldParent != null ) {
                        if ( oldParent.initState == this )
                            oldParent.initState = null;
                        oldParent.children.Remove(this);
                    }

                    //
                    if ( value != null ) {
                        value.children.Add(this);
                        // if this is first child 
                        if ( value.children.Count == 1 )
                            value.initState = this; 
                    }
                    parent_ = value;
                }
            }
            get { return parent_; }
        }

        protected Machine machine_ = null;
        public Machine machine {
            get {
                if ( machine_ != null )
                    return machine_;

                State last = this; 
                State root = parent; 
                while ( root != null ) {
                    last = root;
                    root = root.parent;
                }
                machine_ = last as Machine; // null is possible
                return machine_;
            }
        }

        protected State initState_ = null;
        public State initState {
            get { return initState_; }
            set {
                if ( initState_ != value ) {
                    if ( value != null && children.IndexOf(value) == -1 ) {
                        Debug.LogError ( "FSM error: You must use child state as initial state." );
                        initState_ = null;
                        return;
                    }
                    initState_ = value;
                }
            }
        }

        public Mode mode = Mode.Exclusive;
        public List<Transition> transitionList = new List<Transition>();
        protected List<State> currentStates = new List<State>();
        protected List<State> children = new List<State>();

        ///////////////////////////////////////////////////////////////////////////////
        // event handles
        ///////////////////////////////////////////////////////////////////////////////

        public TransitionEventHandler onEnter;
        public TransitionEventHandler onExit;
        public OnEventHandler onEvent;
        public OnActionHandler onAction;

        // public System.EventHandler<Event> onEvent;  // void EventHandler ( object _sender, Event _event )
        // public System.EventHandler onAction;        // void ActionHandler ( object _sender )

        ///////////////////////////////////////////////////////////////////////////////
        // functions
        ///////////////////////////////////////////////////////////////////////////////

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public State ( string _name, State _parent = null ) {
            name = _name;
            parent = _parent;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void ClearCurrentStatesRecursively () {
            currentStates.Clear();
            foreach ( State child in children ) {
                child.ClearCurrentStatesRecursively ();
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: add transition
        // ------------------------------------------------------------------ 

        public T Add<T> ( State _targetState ) where T : Transition, new() {
            T newTranstion = new T();
            newTranstion.source = this;
            newTranstion.target = _targetState;
            transitionList.Add ( newTranstion );
            return newTranstion;
        }

        // ------------------------------------------------------------------ 
        // Desc: add transition
        // ------------------------------------------------------------------ 

        public T Add<T> ( State _targetState, int _id ) where T : EventTransition, new() {
            T newTranstion = Add<T> (_targetState);
            newTranstion.eventID = _id;
            return newTranstion;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void OnAction () { 
            if ( onAction != null ) { 
                onAction ( this );         
            } 
            foreach ( State activeChild in currentStates ) {
                activeChild.OnAction ();
            }
        } 

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void OnEvent ( Event _event ) { 
            if ( onEvent != null ) { 
                onEvent ( this, _event );         
            } 
            foreach ( State activeChild in currentStates ) {
                activeChild.OnEvent (_event);
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void TestTransitions ( ref List<Transition> _validTransitions, Event _event ) {
            foreach ( State activeChild in currentStates ) {
                // NOTE: if parent transition triggerred, the child should always execute onExit transition
                bool hasTranstion = false;
                foreach ( Transition transition in activeChild.transitionList ) {
                    if ( transition.TestEvent (_event) ) {
                        _validTransitions.Add (transition);
                        hasTranstion = true;
                        break;
                    }
                }
                if ( !hasTranstion ) {
                    activeChild.TestTransitions ( ref _validTransitions, _event );
                }
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void EnterStates ( Event _event, State _toEnter, State _toExit ) {
            currentStates.Add (_toEnter);
            if ( machine != null && machine.logDebugInfo ) 
                Debug.Log( "FSM Debug: Enter State - " + _toEnter.name + " at " + Time.time );
            _toEnter.OnEnter ( _toExit, _toEnter, _event );

            if ( _toEnter.children.Count != 0 ) {
                if ( _toEnter.mode == State.Mode.Exclusive ) {
                    if ( _toEnter.initState != null ) {
                        _toEnter.EnterStates( _event, _toEnter.initState, _toExit );
                    }
                    else {
                        Debug.LogError( "FSM error: can't find initial state in " + _toEnter.name );
                    }
                }
                else { // if ( _toEnter.mode == State.Mode.Parallel )
                    foreach ( State child in _toEnter.children ) {
                        _toEnter.EnterStates( _event, child, _toExit );
                    }
                }
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void ExitStates ( Event _event, State _toEnter, State _toExit ) {
            _toExit.ExitAllStates ( _event, _toEnter );
            if ( machine != null && machine.logDebugInfo ) 
                Debug.Log( "FSM Debug: Exit State - " + _toExit.name + " at " + Time.time );
            _toExit.OnExit ( _toExit, _toEnter, _event );
            currentStates.Remove (_toExit);
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void ExitAllStates ( Event _event, State _toEnter ) {
            foreach ( State activeChild in currentStates ) {
                activeChild.ExitAllStates ( _event, _toEnter );
                activeChild.OnExit ( activeChild, _toEnter, _event );
                if ( machine != null && machine.logDebugInfo ) 
                    Debug.Log( "FSM Debug: Exit State - " + activeChild.name + " at " + Time.time );
            }
            currentStates.Clear();
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void OnEnter ( State _from, State _to, Event _event ) { 
            if ( onEnter != null ) { 
                onEnter ( _from, _to, _event );  
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void OnExit ( State _from, State _to, Event _event ) { 
            if ( onExit != null ) { 
                onExit ( _from, _to, _event );    
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public int TotalStates () {
            int count = 1;
            foreach ( State s in children ) {
                count += s.TotalStates();
            }
            return count;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void ShowDebugInfo ( int _level, bool _active, GUIStyle _textStyle ) {
            _textStyle.normal.textColor = _active ? Color.green : new Color( 0.5f, 0.5f, 0.5f );
            GUILayout.BeginHorizontal ();
                GUILayout.Space(5);
                GUILayout.Label ( new string('\t',_level) + name, _textStyle, new GUILayoutOption[] {} );
            GUILayout.EndHorizontal ();

            foreach ( State s in children ) {
                s.ShowDebugInfo ( _level + 1, currentStates.IndexOf(s) != -1, _textStyle );
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    // FinalState
    ///////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    public class FinalState : State {

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public FinalState ( string _name, State _parent = null ) 
            : base ( _name, _parent )
        {
            onEnter += OnFinished;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        void OnFinished ( State _from, State _to, Event _event ) {
            Machine stateMachine = machine;
            if ( stateMachine != null ) {
                stateMachine.Send ( Event.FINISHED );
            }
        }
    }
}

