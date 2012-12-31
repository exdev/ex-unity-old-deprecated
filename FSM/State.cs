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

        ///////////////////////////////////////////////////////////////////////////////
        // properties
        ///////////////////////////////////////////////////////////////////////////////

        public string name = "";
        public Mode mode = Mode.Exclusive;

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

        protected List<Transition> transitionList = new List<Transition>();
        protected List<State> children = new List<State>();

        protected Transition currentTransition = null;
        protected List<State> currentStates = new List<State>();

        ///////////////////////////////////////////////////////////////////////////////
        // event handles
        ///////////////////////////////////////////////////////////////////////////////

        public System.Action<State,State> onEnter = null;
        public System.Action<State,State> onExit = null;
        public System.Action<State> onAction = null;

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
            for ( int i = 0; i < children.Count; ++i ) {
                children[i].ClearCurrentStatesRecursively ();
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: add transition
        // ------------------------------------------------------------------ 

        public T Add<T> ( State _targetState, System.Func<bool> _onCheck = null, System.Func<bool> _onTransition = null ) where T : Transition, new() {
            T newTranstion = new T ();
            newTranstion.source = this;
            newTranstion.target = _targetState;
            if ( _onCheck != null ) newTranstion.onCheck = _onCheck;
            if ( _onTransition != null ) newTranstion.onTransition = _onTransition;

            transitionList.Add ( newTranstion );
            return newTranstion;
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void OnAction () { 
            if ( onAction != null ) onAction ( this );         

            for ( int i = 0; i < currentStates.Count; ++i ) {
                currentStates[i].OnAction ();

                // DISABLE { 
                // if ( machine != null && machine.logDebugInfo ) 
                //     Debug.Log( "FSM Debug: On Action - " + currentStates[i].name + " at " + Time.time );
                // } DISABLE end 
            }
        } 

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void CheckConditions () {
            // if we are in transtion, don't do anything
            if ( currentTransition != null )
                return;

            //
            for ( int i = 0; i < currentStates.Count; ++i ) {
                State activeChild = currentStates[i];

                // NOTE: if parent transition triggerred, the child should always execute onExit transition
                for ( int j = 0; j < activeChild.transitionList.Count; ++j ) {
                    Transition transition = activeChild.transitionList[j];
                    if ( transition.onCheck() ) {

                        // exit states
                        transition.source.parent.ExitStates ( transition.target, transition.source );

                        // set current transition
                        currentTransition = transition;

                        break;
                    }
                }

                if ( currentTransition == null ) {
                    activeChild.CheckConditions ();
                }
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void UpdateTransitions () {
            if ( currentTransition != null ) {
                // update transition
                if ( currentTransition.onTransition() ) {

                    // enter states
                    State targetState = currentTransition.target;
                    if ( targetState == null )
                        targetState = currentTransition.source;
                    targetState.parent.EnterStates ( targetState, currentTransition.source );

                    //
                    currentTransition = null;
                }
            }
            else {
                for ( int i = 0; i < currentStates.Count; ++i ) {
                    State activeChild = currentStates[i];
                    activeChild.UpdateTransitions();
                }
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void EnterStates ( State _toEnter, State _toExit ) {
            currentStates.Add (_toEnter);

            if ( machine != null && machine.logDebugInfo ) 
                Debug.Log( "FSM Debug: Enter State - " + _toEnter.name + " at " + Time.time );

            if ( _toEnter.onEnter != null )
                _toEnter.onEnter ( _toExit, _toEnter );

            if ( _toEnter.children.Count != 0 ) {
                if ( _toEnter.mode == State.Mode.Exclusive ) {
                    if ( _toEnter.initState != null ) {
                        _toEnter.EnterStates( _toEnter.initState, _toExit );
                    }
                    else {
                        Debug.LogError( "FSM error: can't find initial state in " + _toEnter.name );
                    }
                }
                else { // if ( _toEnter.mode == State.Mode.Parallel )
                    for ( int i = 0; i < _toEnter.children.Count; ++i ) {
                        _toEnter.EnterStates( _toEnter.children[i], _toExit );
                    }
                }
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void ExitStates ( State _toEnter, State _toExit ) {
            _toExit.ExitAllStates ( _toEnter );

            if ( machine != null && machine.logDebugInfo ) 
                Debug.Log( "FSM Debug: Exit State - " + _toExit.name + " at " + Time.time );

            if ( _toExit.onExit != null )
                _toExit.onExit ( _toExit, _toEnter );

            currentStates.Remove (_toExit);
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void ExitAllStates ( State _toEnter ) {
            for ( int i = 0; i < currentStates.Count; ++i ) {

                State activeChild = currentStates[i];
                activeChild.ExitAllStates ( _toEnter );

                if ( activeChild.onExit != null )
                    activeChild.onExit ( activeChild, _toEnter );

                if ( machine != null && machine.logDebugInfo ) 
                    Debug.Log( "FSM Debug: Exit State - " + activeChild.name + " at " + Time.time );
            }
            currentStates.Clear();
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public int TotalStates () {
            int count = 1;
            for ( int i = 0; i < children.Count; ++i ) {
                count += children[i].TotalStates();
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

            for ( int i = 0; i < children.Count; ++i ) {
                State s = children[i];
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

        void OnFinished ( State _from, State _to ) {
            // TODO { 
            // Machine stateMachine = machine;
            // if ( stateMachine != null ) {
            //     stateMachine.Send ( Event.FINISHED );
            // }
            // } TODO end 
        }
    }
}

