// ======================================================================================
// File         : PlayerInputListener.cs
// Author       : Wu Jie 
// Last Change  : 11/28/2012 | 15:55:13 PM | Wednesday,November
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

public class PlayerInputListener : InputListener {

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // TODO: load from configuration file, canWhat, canWhat, ...

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void HandleInput () {
        float h = 0.0f;
        float v = 0.0f;
        bool moveActor = false;

        //
        if ( Input.GetKey(KeyCode.W) ) {
            v += 1.0f;
            moveActor = true;
        }
        if ( Input.GetKey(KeyCode.S) ) {
            v -= 1.0f;
            moveActor = true;
        }

        //
        if ( Input.GetKey(KeyCode.D) ) {
            h += 1.0f;
            moveActor = true;
        }
        if ( Input.GetKey(KeyCode.A) ) {
            h -= 1.0f;
            moveActor = true;
        }

        //
        if ( moveActor ) {
            Transform camTrans = Game.cameraMng.renderCamera.transform;
            Vector3 moveDir = camTrans.forward * v + camTrans.right * h;
            moveDir.y = 0.0f;
            moveDir.Normalize();

            actor.MoveTowards(moveDir);
        }
        else {
            actor.Stop ();
        }

        //
        if ( Input.GetKeyDown(KeyCode.Space) ) {
            actor.Jump();
        }
    }
}


