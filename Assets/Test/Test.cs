// ======================================================================================
// File         : Test.cs
// Author       : Wu Jie 
// Last Change  : 11/19/2012 | 17:47:26 PM | Monday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// \class Test
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class Test : MonoBehaviour {
    public float blend_1 = 0.0f;
    public float blend_2 = 0.0f;
    public float blend_3 = 0.0f;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        if ( Input.GetKeyDown( KeyCode.I ) ) {
            animation.Blend( "Camera1", 1.0f );
            animation.Blend( "Camera2", 0.0f );
            animation.Blend( "Camera3", 0.0f );
        }
        if ( Input.GetKeyDown( KeyCode.O ) ) {
            animation.Blend( "Camera1", 0.0f );
            animation.Blend( "Camera2", 1.0f );
            animation.Blend( "Camera3", 0.0f );
        }
        if ( Input.GetKeyDown( KeyCode.P ) ) {
            animation.Blend( "Camera1", 0.0f );
            animation.Blend( "Camera2", 0.0f );
            animation.Blend( "Camera3", 1.0f );
        }
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnGUI () {
    }
}
