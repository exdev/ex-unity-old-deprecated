// ======================================================================================
// File         : OrbitFollowCameraCtrl.cs
// Author       : Wu Jie 
// Last Change  : 11/27/2012 | 17:29:08 PM | Tuesday,November
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

public class OrbitFollowCameraCtrl : CameraCtrl {

    ///////////////////////////////////////////////////////////////////////////////
    // serialize
    ///////////////////////////////////////////////////////////////////////////////

    public Transform traceTarget;
    public Vector3 offset = Vector3.zero;

    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;

    public float minCameraRotUp = -50.0f;
    public float maxCameraRotUp = 80.0f;

    public float moveDampingDuration = 0.1f;
    public float rotDampingDuration = 0.1f;
    public float zoomDampingDuration = 0.3f;

    ///////////////////////////////////////////////////////////////////////////////
    // non-serialize
    ///////////////////////////////////////////////////////////////////////////////

    float destDistance;
    float destCameraRotUp;
    float destCameraRotSide;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Start () {
        destDistance = -cameraAnchor.localPosition.z;
        destDistance = Mathf.Clamp(destDistance, minDistance, maxDistance);

        destCameraRotUp = transform.eulerAngles.x;
        destCameraRotUp = Mathf.Clamp(destCameraRotUp, minCameraRotUp, maxCameraRotUp);

        destCameraRotSide = transform.eulerAngles.y;
    }
    
    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        HandleInput ();
        UpdateTransform ();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void Apply () {
        if ( traceTarget ) {
            transform.position = GetLookAtPoint();
        }
        transform.rotation = Quaternion.Euler(destCameraRotUp, destCameraRotSide, 0);
        cameraAnchor.localPosition = -Vector3.forward * destDistance;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void HandleInput () {
        if ( acceptInput == false )
            return;

        if (Input.GetMouseButton(1)) {
            destCameraRotSide += Input.GetAxis("Mouse X")*5;
            destCameraRotUp -= Input.GetAxis("Mouse Y")*5;
        }

        destCameraRotUp = Mathf.Clamp(destCameraRotUp, minCameraRotUp, maxCameraRotUp);

        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if ( Mathf.Abs(zoomDelta) >= 0.01f ) {
            destDistance *= (1.0f - zoomDelta);
            destDistance = Mathf.Clamp(destDistance, minDistance, maxDistance);
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    float curCameraRotUpVel = 0.0f;
    float curCameraRotSideVel = 0.0f;
    Vector3 curVel = Vector3.zero;
    float curZoomVel = 0.0f;
    // ------------------------------------------------------------------ 

    void UpdateTransform () {
        Vector3 eulerAngles = transform.eulerAngles;
        // DISABLE { 
        // float rotDamping = 10.0f;
        // float newCameraRotSide = Mathf.LerpAngle(eulerAngles.y, destCameraRotSide, rotDamping * Time.deltaTime);
        // float newCameraRotUp = Mathf.Lerp(eulerAngles.x, destCameraRotUp, rotDamping * Time.deltaTime);
        // } DISABLE end 
        if ( float.IsNaN(curCameraRotUpVel) ) curCameraRotUpVel = 0.0f;
        if ( float.IsNaN(curCameraRotSideVel) ) curCameraRotSideVel = 0.0f;

        float newCameraRotUp = Mathf.SmoothDampAngle(eulerAngles.x, destCameraRotUp, ref curCameraRotUpVel, rotDampingDuration);
        float newCameraRotSide = Mathf.SmoothDampAngle(eulerAngles.y, destCameraRotSide, ref curCameraRotSideVel, rotDampingDuration);
        transform.rotation = Quaternion.Euler(newCameraRotUp, newCameraRotSide, 0);
        
        if ( traceTarget ) {
            Vector3 lookAtPoint = GetLookAtPoint();

            // DISABLE { 
            // float moveDamping = 10.0f;
            // transform.position = Vector3.Lerp(transform.position, lookAtPoint, moveDamping * Time.deltaTime);
            // } DISABLE end 
            transform.position = Vector3.SmoothDamp( transform.position, lookAtPoint, ref curVel, moveDampingDuration );
        }
        
        // DISABLE { 
        // float dist = Mathf.Lerp(-cameraAnchor.transform.localPosition.z, distance, curZoomDamping * Time.deltaTime);
        // } DISABLE end 
        float dist = Mathf.SmoothDamp( -cameraAnchor.transform.localPosition.z, destDistance, ref curZoomVel, zoomDampingDuration );
        cameraAnchor.localPosition = -Vector3.forward * dist;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Vector3 GetLookAtPoint () { return traceTarget.position + offset; }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void MoveTo ( Vector3 _pos ) {
        Vector3 lookAtPoint = GetLookAtPoint();
        Vector3 delta = lookAtPoint - _pos;

        //
        Vector3 dir = delta;
        dir.Normalize();

        Quaternion quat = Quaternion.identity;
        quat.SetLookRotation( dir );
        Vector3 eulerAngles = quat.eulerAngles;

        Set ( eulerAngles.x, eulerAngles.y, delta.magnitude );
    }

    // ------------------------------------------------------------------
    // Desc:
    // ------------------------------------------------------------------

    public void Set ( float _cameraRotUp, float _cameraRotSide, float _distance ) {
        destDistance = _distance;
        destDistance = Mathf.Clamp(destDistance, minDistance, maxDistance);

        destCameraRotUp = _cameraRotUp;
        destCameraRotUp = Mathf.Clamp(destCameraRotUp, minCameraRotUp, maxCameraRotUp);

        destCameraRotSide = _cameraRotSide;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void OnDrawGizmos () {
        Gizmos.color = new Color( 1.0f, 1.0f, 0.0f, 0.8f ); // Color.yellow;
        Gizmos.DrawSphere ( cameraAnchor.position, 0.2f );

        Gizmos.color = new Color( 1.0f, 0.0f, 0.0f, 0.8f ); // Color.yellow;
        Gizmos.DrawSphere ( transform.position, 0.2f );

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine ( cameraAnchor.position, transform.position );
    }
}
