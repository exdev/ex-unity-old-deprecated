// ======================================================================================
// File         : MovementState.cs
// Author       : Wu Jie 
// Last Change  : 11/28/2012 | 16:02:16 PM | Wednesday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// \class MovementState 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class MovementState : ScriptableObject {

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    public string layerMaskName = "";
    public bool applyGravity = true;
    public bool smoothPathFinding = true;
    public float moveSlowDownDistance = 1.0f;
    public float speedDamping = 10.0f;
    public float rotateDamping = 10.0f;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    protected Actor actor = null;
    protected CharacterController charCtrl = null;
    protected int layerMasks = 0;

    // moving towards
    protected Vector3 moveDir = Vector3.zero;
    protected float moveSpeed = 0.0f;
    protected Vector3 velocity = Vector3.zero;

    // moving to or path process
    protected int curPathIdx = 0;
    protected Vector3[] path = null;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( Actor _actor ) {
        //
        actor = _actor; 
        charCtrl = _actor.GetComponent<CharacterController>();
        layerMasks = Game.layerMng.GetLayerMask ( layerMaskName );
        moveDir = _actor.transform.forward;

        //
        moveSpeed = 0.0f;
        velocity = Vector3.zero;
        curPathIdx = 0;
        path = new Vector3[0];
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public bool CheckCapsule ( float _height, float _raidus ) {
        return Physics.CheckCapsule ( actor.transform.position,
                                      actor.transform.position + new Vector3 ( 0.0f, _height, 0.0f ),
                                      _raidus,
                                      layerMasks );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Vector3 GetMoveDir () { return moveDir; }
    public float GetMoveSpeed () { return moveSpeed; }
    public Vector3 GetVelocity () { return velocity; }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void MoveTo ( Vector3 _pos ) {
        moveSpeed = actor.actorInfo.maxMoveSpeed;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void MoveTowards ( Vector3 _dir ) {
        FaceTo (_dir);
        moveSpeed = actor.actorInfo.maxMoveSpeed;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public bool FaceTo ( Vector3 _dir ) {
        if ( _dir.x != 0.0f || _dir.z != 0.0f ) {
            moveDir = _dir;
            moveDir.y = 0.0f;
            moveDir.Normalize();
            return true;
        }
        return false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Stop () {
        moveSpeed = 0.0f;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void Step () {
        // handle path
        if ( path != null && path.Length > 0 ) {
            bool isLastPoint = false;

            if ( curPathIdx == path.Length - 1 )
                isLastPoint = true;

            if ( smoothPathFinding ) {
                // TODO:
            }

            //
            Vector3 vDistance = path[curPathIdx] - actor.transform.position;
            vDistance.y = 0.0f;
            moveDir = vDistance.normalized;

            //
            if ( isLastPoint ) {
                float distance = vDistance.magnitude;
                float ratio = ( distance / moveSlowDownDistance );
                moveSpeed = Mathf.SmoothStep ( 0.0f, actor.actorInfo.maxMoveSpeed, ratio );
            }
        }

        // update position
        Vector3 wantedVelocity = moveDir * moveSpeed;
        velocity = Vector3.Lerp ( velocity, wantedVelocity, speedDamping * Time.smoothDeltaTime );
        velocity.y = 0.0f;

        // apply gravity
        if ( applyGravity ) {
            float gravity = 1000.0f;
            Vector3 vGravity = new Vector3 ( 0.0f, -gravity * Time.smoothDeltaTime, 0.0f );
            velocity += vGravity;
        }

        //
        // if ( velocity.magnitude > Mathf.Epsilon ) {
        Vector3 nextStep = velocity * Time.smoothDeltaTime;

        if ( charCtrl != null )
            charCtrl.Move( nextStep );
        else 
            actor.transform.position += nextStep;
        // }

        // update rotation
        if ( moveDir != Vector3.zero ) {
            Quaternion quat = Quaternion.identity;
            quat.SetLookRotation (moveDir);
            actor.transform.rotation = Quaternion.Slerp ( actor.transform.rotation, quat, rotateDamping * Time.smoothDeltaTime );
        }
    }
}

