// ======================================================================================
// File         : exStaticDebugger.cs
// Author       : Wu Jie 
// Last Change  : 02/19/2012 | 21:20:42 PM | Sunday,February
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

public static class exStaticDebugger {

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void Assert ( bool _condition, string _msg, bool _break = false ) {
        if ( _condition == false ) {
            Debug.LogError ( "Assert Failed: " + _msg );
            if ( _break )
                Debug.Break ();
        }
    } 

    // ------------------------------------------------------------------ 
    // Desc: DrawCircle
    // ------------------------------------------------------------------ 

    // DrawCircleX
    public static void DrawCircleX ( Vector3 _center, float _radius, Color _color, float _duration = 0.0f, bool _depthTets = true ) {
        DrawCircle ( Quaternion.Euler(0.0f,0.0f,90.0f), _center, _radius, _color, _duration, _depthTets );
    }

    // DrawCircleY
    public static void DrawCircleY ( Vector3 _center, float _radius, Color _color, float _duration = 0.0f, bool _depthTets = true ) {
        DrawCircle ( Quaternion.identity, _center, _radius, _color, _duration, _depthTets );
    }

    // DrawCircleZ
    public static void DrawCircleZ ( Vector3 _center, float _radius, Color _color, float _duration = 0.0f, bool _depthTets = true ) {
        DrawCircle ( Quaternion.Euler(90.0f,0.0f,0.0f), _center, _radius, _color, _duration, _depthTets );
    }

    // 
    public static void DrawCircle ( Quaternion _rot, Vector3 _center, float _radius, Color  _color, float _duration = 0.0f, bool _depthTets = true ) {
        //
        float two_pi = 2.0f * Mathf.PI;
        float segments = 32.0f;
        float step = two_pi / segments;
        float theta = 0.0f;

        //
        Vector3 last = _center + _rot * ( _radius * new Vector3( Mathf.Cos(theta), 0.0f, Mathf.Sin(theta) ) );
        theta += step;

        //
        for ( int i = 1; i <= segments; ++i ) {
            Vector3 cur = _center + _rot * ( _radius * new Vector3( Mathf.Cos(theta), 0.0f, Mathf.Sin(theta) ) );
            Debug.DrawLine ( last, cur, _color, _duration, _depthTets );
            last = cur;
            theta += step;
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void DrawBall ( Vector3 _center, float _radius, Color _color, float _duration = 0.0f, bool _depthTets = true  ) {
        DrawCircleX ( _center, _radius, _color, _duration, _depthTets );
        DrawCircleY ( _center, _radius, _color, _duration, _depthTets );
        DrawCircleZ ( _center, _radius, _color, _duration, _depthTets );
    }

    // ------------------------------------------------------------------ 
    // Desc: GizmosDrawCircle
    // ------------------------------------------------------------------ 

    // DrawCircleX
    public static void GizmosDrawCircleX ( Vector3 _center, float _radius, Color _color ) {
        GizmosDrawCircle ( Quaternion.Euler(0.0f,0.0f,90.0f), _center, _radius, _color );
    }

    // DrawCircleY
    public static void GizmosDrawCircleY ( Vector3 _center, float _radius, Color _color ) {
        GizmosDrawCircle ( Quaternion.identity, _center, _radius, _color );
    }

    // DrawCircleZ
    public static void GizmosDrawCircleZ ( Vector3 _center, float _radius, Color _color ) {
        GizmosDrawCircle ( Quaternion.Euler(90.0f,0.0f,0.0f), _center, _radius, _color );
    }

    // 
    public static void GizmosDrawCircle ( Quaternion _rot, Vector3 _center, float _radius, Color _color ) {
        //
        float two_pi = 2.0f * Mathf.PI;
        float segments = 32.0f;
        float step = two_pi / segments;
        float theta = 0.0f;

        //
        Vector3 last = _center + _rot * ( _radius * new Vector3( Mathf.Cos(theta), 0.0f, Mathf.Sin(theta) ) );
        theta += step;

        //
        for ( int i = 1; i <= segments; ++i ) {
            Vector3 cur = _center + _rot * ( _radius * new Vector3( Mathf.Cos(theta), 0.0f, Mathf.Sin(theta) ) );
            Gizmos.color = _color;
            Gizmos.DrawLine ( last, cur );
            last = cur;
            theta += step;
        }
    }
}

