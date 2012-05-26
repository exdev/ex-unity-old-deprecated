// ======================================================================================
// File         : exTimebasedCurveInfo.cs
// Author       : Wu Jie 
// Last Change  : 08/06/2011 | 21:39:08 PM | Saturday,August
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// exTimebasedCurveInfo
///////////////////////////////////////////////////////////////////////////////

public class exTimebasedCurveInfo : ScriptableObject {

    public enum WrapMode {
        Once,
        Loop,
        PingPong,
    } 

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    public WrapMode wrapMode = WrapMode.Once;
    public float length = 1.0f;
    public bool useRealTime = false;
    public bool useEaseCurve = true;
    public exEase.Type easeCurveType = exEase.Type.Linear;
    public AnimationCurve animationCurve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public float WrapSeconds ( float _seconds, WrapMode _wrapMode ) {
        float t = Mathf.Abs(_seconds);
        if ( _wrapMode == WrapMode.Loop ) {
            t %= length;
        }
        else if ( _wrapMode == WrapMode.PingPong ) {
            int cnt = (int)(t/length);
            t %= length;
            if ( cnt % 2 == 1 ) {
                t = length - t;
            }
        }
        else {
            t = Mathf.Clamp( t, 0.0f, length );
        }
        return t;
    }
}

///////////////////////////////////////////////////////////////////////////////
//
///////////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class exTimebasedCurve {

    public exTimebasedCurveInfo data;

    private bool inverse = false;
    private exEase.easeCallback callback;
    private float time = 0.0f; 
    private float lastTime = 0.0f; 
    private bool timeup = false;
    private bool started = false;
    private float duration = 0.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Start ( bool _rewind = false, float _duration = -1.0f ) {
        duration = (_duration <= 0.0f) ? data.length : _duration;

        callback = data.useEaseCurve ? exEase.TypeToFunction(data.easeCurveType) : data.animationCurve.Evaluate;
        lastTime = data.useRealTime ? Time.realtimeSinceStartup : Time.time;
        if ( _rewind || started == false ) {
            if ( inverse )
                time = duration;
            else
                time = 0.0f;
        }
        timeup = false;
        started = true;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Inverse ( bool _enable ) {
        inverse = _enable;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public bool IsTimeUp () {
        return timeup;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public bool IsPlaying () { return started == true; }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public float Step () {
        if ( timeup ) {
            if ( inverse )
                return 0.0f;
            else
                return 1.0f;
        }

        if ( started == false ) {
            Debug.LogWarning( "the curve didn't started, please call curve.Start() first" );
            if ( inverse )
                return 1.0f;
            else
                return 0.0f;
        }

        float curTime = data.useRealTime ? Time.realtimeSinceStartup : Time.time;
        float deltaTime = curTime - lastTime;
        lastTime = curTime;

        //
        if ( inverse )
            deltaTime = -deltaTime;
        time += deltaTime;

        //
        float wrappedTime = data.WrapSeconds(time, data.wrapMode);

        // check if stop
        if ( data.wrapMode == exTimebasedCurveInfo.WrapMode.Once ) {
            if ( (!inverse && time >= duration) ||
                 (inverse && time <= 0.0f) )
            {
                timeup = true;
                started = false;
                if ( inverse ) {
                    time = 0.0f;
                    return 0.0f;
                }
                else {
                    time = duration;
                    return 1.0f;
                }
            }
        }

        //
        float ratio = Mathf.Clamp ( wrappedTime/duration, 0.0f, 1.0f );
        return callback(ratio);
    }
}
