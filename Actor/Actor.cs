// ======================================================================================
// File         : Actor.cs
// Author       : Wu Jie 
// Last Change  : 11/28/2012 | 14:00:02 PM | Wednesday,November
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

public class Actor : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // serialized
    ///////////////////////////////////////////////////////////////////////////////

    public ActorInfo actorInfo = null;
    public MovementState movementState = null;
    public InputListener inputListener = null;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    protected CharacterController charCtrl = null;
	protected Animator animator = null;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnDrawGizmos () {
        Vector3 drawPos = transform.position + transform.up * 0.1f;
        exStaticDebugger.GizmosDrawCircleY ( drawPos,
                                             actorInfo.radius,
                                             new Color( 1.0f, 0.0f, 1.0f ) );
        if ( movementState ) {
            Gizmos.color = new Color( 1.0f, 0.0f, 1.0f );
            Vector3 vel = movementState.GetVelocity();
            vel.y = 0.0f;
            Gizmos.DrawLine ( drawPos, drawPos + vel );
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start () {
        charCtrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if ( charCtrl && actorInfo ) {
            charCtrl.radius = actorInfo.radius;
            charCtrl.height = actorInfo.height;
        }
        if ( movementState )
            movementState.Init ( this );

        if ( inputListener )
            inputListener.Init ( this );
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        if ( inputListener ) {
            inputListener.HandleInput();
        }

        if ( movementState ) {
            movementState.Step ();
        }

        if ( animator ) {
            HandleAnimation ();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void HandleAnimation () {
        // NOTE: it is better performance to cache id at Awake/Start

        int id = Animator.StringToHash("Speed");
        animator.SetFloat ( id, movementState.GetMoveSpeed() );

        Vector3 curentDir = transform.forward;
        Vector3 wantedDir = movementState.GetMoveDir();
        float angle = Vector3.Angle ( curentDir, wantedDir );
        Vector3 up = Vector3.Cross ( curentDir, wantedDir );
        angle *= Mathf.Sign(up.y); 
        // exDebugHelper.ScreenPrint( "angle = " + angle );
        animator.SetFloat ( "Direction", angle/90.0f, 0.1f, Time.deltaTime );

        //
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
        if ( stateInfo.IsName("Base Layer.Jump") ) {
            animator.SetBool ( "Jump", false );
        }
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void MoveTo ( Vector3 _pos ) {
        // TODO: path finding here
        if ( movementState ) movementState.MoveTo ( _pos );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void MoveTowards ( Vector3 _dir ) {
        if ( movementState ) movementState.MoveTowards ( _dir );
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Stop () {
        if ( movementState ) movementState.Stop ();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Jump () {
        animator.SetBool ( "Jump", true );
    }
}
