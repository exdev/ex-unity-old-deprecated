// ======================================================================================
// File         : exDebugHelper.cs
// Author       : Wu Jie 
// Last Change  : 06/05/2011 | 11:08:21 AM | Sunday,June
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

// ------------------------------------------------------------------ 
// Desc: 
// ------------------------------------------------------------------ 

[ExecuteInEditMode]
public class exDebugHelper : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // static
    ///////////////////////////////////////////////////////////////////////////////

    // static instance
    private static exDebugHelper instance = null;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void ScreenPrint ( string _text ) {
        if ( instance.showScreenPrint_ ) {
#if EX2D
            instance.txtPrint.text = instance.txtPrint.text + _text + "\n"; 
#else
            instance.txtPrint = instance.txtPrint + _text + "\n"; 
#endif
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void ScreenPrint ( Vector2 _pos, string _text ) {
        if ( instance.showScreenDebugText ) {
#if EX2D
            exSpriteFont debugText = instance.debugTextPool.Request<exSpriteFont>();

            Vector2 screenPos = debugText.renderCamera.WorldToScreenPoint(_pos);
            exScreenPosition screenPosition = debugText.GetComponent<exScreenPosition>();
            screenPosition.x = screenPos.x;
            screenPosition.y = Screen.height - screenPos.y;

            debugText.text = _text;
            debugText.enabled = true;
#else
            TextInfo info = new TextInfo( _pos, _text ); 
            instance.debugTextPool.Add(info);
#endif
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void ScreenLog ( string _text ) {
        instance.logs.Add(_text);
        if ( instance.logs.Count > instance.logCount ) {
            instance.logs.RemoveAt(0);
        }
        instance.updateLogText = true;
    }

    ///////////////////////////////////////////////////////////////////////////////
    // serialized
    ///////////////////////////////////////////////////////////////////////////////

#if EX2D
    public exSpriteFont txtPrint;
    public exSpriteFont txtFPS;
    public exSpriteFont txtLog;
    public exGameObjectPool debugTextPool = new exGameObjectPool();
#else 
    protected string txtPrint = "screen print: ";
    protected string txtFPS = "fps: ";
    protected string txtLog = "log: ";

    public class TextInfo {
        public Vector2 screenPos = Vector2.zero;
        public string text;

        public TextInfo ( Vector2 _screenPos, string _text ) {
            screenPos = _screenPos;
            text = _text;
        }
    }
    protected List<TextInfo> debugTextPool = new List<TextInfo>();
#endif

    [SerializeField] protected bool showFps_ = true;
    public bool showFps {
        get { return showFps_; }
        set {
            if ( showFps_ != value ) {
                showFps_ = value;
#if EX2D
                if ( txtFPS != null )
                    txtFPS.enabled = showFps_;
#endif
            }
        }
    }

    [SerializeField] protected bool showScreenPrint_ = true;
    public bool showScreenPrint {
        get { return showScreenPrint_; }
        set {
            if ( showScreenPrint_ != value ) {
                showScreenPrint_ = value;
#if EX2D
                if ( txtPrint != null )
                    txtPrint.enabled = showScreenPrint_;
#endif
            }
        }
    }

    [SerializeField] protected bool showScreenLog_ = true;
    public bool showScreenLog {
        get { return showScreenLog_; }
        set {
            if ( showScreenLog_ != value ) {
                showScreenLog_ = value;
#if EX2D
                if ( txtLog != null ) 
                    txtLog.enabled = showScreenLog_;
#endif
            }
        }
    }

    public bool showScreenDebugText = false;
    public int logCount = 10;

    ///////////////////////////////////////////////////////////////////////////////
    // non-serialized
    ///////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public List<string> logs = new List<string>();
    [System.NonSerialized] public bool updateLogText = false; 
    private int frames = 0;
    private float fps = 0.0f;
    private float lastInterval = 0.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake () {
        if ( instance == null )
            instance = this;

#if EX2D
        txtPrint.text = "";
        txtFPS.text = "";
        txtLog.text = "";

        if ( showScreenDebugText ) {
            debugTextPool.Init();
            for ( int i = 0; i < debugTextPool.initData.Length; ++i ) {
                GameObject textGO = debugTextPool.initData[i];
                textGO.transform.parent = transform;
                textGO.transform.localPosition = Vector3.zero;
                textGO.GetComponent<exSpriteFont>().enabled = false;
            }
        }
#else
        txtPrint = "";
        txtFPS = "";
        txtLog = "";

        if ( showScreenDebugText ) {
            debugTextPool.Clear();
        }
#endif
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start () {
        InvokeRepeating("UpdateFPS", 0.0f, 1.0f );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        // count fps
        ++frames;

        // update log
        UpdateLog ();

        // NOTE: the OnGUI call multiple times in one frame, so we just clear text here.
        StartCoroutine ( CleanDebugText() );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

#if !EX2D
    void OnGUI () {
        GUIContent content = null;
        Vector2 size = Vector2.zero;
        float curX = 10.0f;
        float curY = 10.0f;

        if ( showFps ) {
            content = new GUIContent(txtFPS);
            size = GUI.skin.label.CalcSize(content);
            GUI.Label ( new Rect( curX, curY, size.x, size.y ), txtFPS );
            curY += size.y;
        }
        if ( showScreenPrint ) {
            content = new GUIContent(txtPrint);
            size = GUI.skin.label.CalcSize(content);
            GUI.Label ( new Rect( curX, curY, size.x, size.y ), txtPrint );
        }
        if ( showScreenLog ) {
            content = new GUIContent(txtLog);
            size = GUI.skin.label.CalcSize(content);
            GUI.Label ( new Rect( Screen.width - 10.0f - size.x, Screen.height - 10.0f - size.y, size.x, size.y ), txtLog );
        }
        if ( showScreenDebugText ) {
            for ( int i = 0; i < debugTextPool.Count; ++i ) {
                TextInfo info = debugTextPool[i];
                content = new GUIContent(info.text);
                size = GUI.skin.label.CalcSize(content);

                Vector2 pos = new Vector2( info.screenPos.x, Screen.height - info.screenPos.y ) - size * 0.5f; 
                GUI.Label ( new Rect( pos.x, pos.y, size.x, size.y ), info.text );
            }
        }
    }
#endif

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateFPS () {
        float timeNow = Time.realtimeSinceStartup;
        fps = frames / (timeNow - lastInterval);
        frames = 0;
        lastInterval = timeNow;
#if EX2D
        txtFPS.text = "fps: " + fps.ToString("f2");
#else
        txtFPS = "fps: " + fps.ToString("f2");
#endif
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    IEnumerator CleanDebugText () {
        yield return new WaitForEndOfFrame();
#if EX2D
        txtPrint.text = "";

        if ( showScreenDebugText ) {
            debugTextPool.Reset();
            for ( int i = 0; i < debugTextPool.initData.Length; ++i ) {
                GameObject textGO = debugTextPool.initData[i];
                textGO.GetComponent<exSpriteFont>().enabled = false;
            }
        }
#else
        txtPrint = "";

        if ( showScreenDebugText ) {
            debugTextPool.Clear();
        }
#endif
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void UpdateLog () {
        if ( updateLogText ) {
            string text = "";
            foreach ( string l in logs ) {
                text = text + l + "\n";
            }
#if EX2D 
            txtLog.text = text;
#else
            txtLog = text;
#endif
            updateLogText = false;
        }
    }
}
