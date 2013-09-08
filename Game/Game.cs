// ======================================================================================
// File         : Game.cs
// Author       : Wu Jie 
// Last Change  : 11/23/2012 | 11:40:19 AM | Friday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// \class 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class Game : FSMBase {

    public enum EventType {
        LoadStage = fsm.Event.USER_FIELD + 1,
        ShowStage,
        QuitApp,
    }

    public class LoadStageEvent : fsm.Event {
        public string scene;

        public LoadStageEvent ( string _sceneName ) 
            : base ( (int)EventType.LoadStage )
        {
            scene = _sceneName;
        }
    } 

    ///////////////////////////////////////////////////////////////////////////////
    // statics
    ///////////////////////////////////////////////////////////////////////////////

    public static Game instance = null;
    public static LayerMng layerMng { get { return instance.layerMng_; } }
    public static CameraMng cameraMng { get { return instance.cameraMng_; } }

    // TEST & DEBUG { 
    public static bool isUnitTest = false;
    // } TEST & DEBUG end 

    ///////////////////////////////////////////////////////////////////////////////
    // serialized
    ///////////////////////////////////////////////////////////////////////////////

    // submodules
    public LayerMng layerMng_ = null;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // submodules
    protected CameraMng cameraMng_;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void UnitTestCreate () {
        isUnitTest = true;
        GameObject gamePrefab = Resources.Load ( "prefab/Game", typeof(GameObject) ) as GameObject;
        if ( gamePrefab ) {
            GameObject gameGO = GameObject.Instantiate ( gamePrefab, Vector3.zero, Quaternion.identity ) as GameObject;
            instance = gameGO.GetComponent<Game>();
        }
        else {
            Debug.LogError ( "Can not find Game prefab at Resources/prefab/Game" );
        }
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected new void Awake () {
        if ( instance != null )
            return;

        instance = this;
        DontDestroyOnLoad (gameObject);

        base.Awake ();

        //
        InitSubModules ();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start () {
        stateMachine.Start();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        stateMachine.Update();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected override void InitStateMachine () {

        // ======================================================== 
        // init states 
        // ======================================================== 

        fsm.State launch = new fsm.State( "launch", stateMachine );
        launch.onEnter += EnterLaunchState;
        launch.onExit += ExitLaunchState;

        fsm.State loading = new fsm.State( "loading", stateMachine );
        loading.onEnter += EnterLoadingState;
        loading.onExit += ExitLoadingState;

        fsm.State stage = new fsm.State( "stage", stateMachine );
        stage.onEnter += EnterStageState;
        stage.onExit += ExitStageState;

        // ======================================================== 
        // setup transitions 
        // ======================================================== 

        launch.Add<fsm.EventTransition> ( stage, (int)EventType.ShowStage );

        loading.Add<fsm.EventTransition> ( stage, (int)EventType.ShowStage );

        stage.Add<fsm.EventTransition> ( loading, (int)EventType.LoadStage );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void InitSubModules () {
        cameraMng_ = GetComponent<CameraMng>();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void LoadStage ( string _sceneName ) {
        if ( isUnitTest ) {
            Debug.LogWarning ( "You can not change stage in test stage environment" );
        }
        else {
            stateMachine.Send( new LoadStageEvent(_sceneName) );
        }
    } 

    ///////////////////////////////////////////////////////////////////////////////
    // launch state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterLaunchState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        if ( isUnitTest == false ) {
            StartCoroutine ( "Launch" );
        }
        else {
            stateMachine.Send( (int)EventType.ShowStage );
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ExitLaunchState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    IEnumerator Launch () {
        // Application.LoadLevel( "MainMenu" );

        // NOTE: one frame later to make all gameObject in the scene/stage Awake and available
        yield return 0;

        // TODO { 
        // loadingScene.ResetCamera( Stage.mainCamera );
        // debug.ResetCamera( Stage.mainCamera );
        // } TODO end 

        stateMachine.Send( (int)EventType.ShowStage );
    }

    ///////////////////////////////////////////////////////////////////////////////
    // loading state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterLoadingState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
        LoadStageEvent e = _event as LoadStageEvent;
        if ( e != null ) {
            StartCoroutine( StartLoading( e.scene ) );
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ExitLoadingState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    IEnumerator StartLoading( string _sceneName ) {
        // // fade in loading scene
        // loadingScene.Show();
        // yield return new WaitForSeconds(loadingScene.fadeInDuration);

        // // Application.LoadLevel(_sceneName); // DISABLE
        // AsyncOperation async = Application.LoadLevelAsync(_sceneName);
        // yield return async;

        // one more frame for loading scene init
        yield return 0;

        // loadingScene.ResetCamera( Stage.mainCamera );
        // debug.ResetCamera( Stage.mainCamera );

        // // fade out loading scene
        // loadingScene.Hide();
        // yield return new WaitForSeconds( loadingScene.fadeOutDuration );

        stateMachine.Send( (int)EventType.ShowStage );
    }

    ///////////////////////////////////////////////////////////////////////////////
    // stage state
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void EnterStageState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ExitStageState ( fsm.State _from, fsm.State _to, fsm.Event _event ) {
    }
}
