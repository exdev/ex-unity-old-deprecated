// ======================================================================================
// File         : CameraMng.cs
// Author       : Wu Jie 
// Last Change  : 11/27/2012 | 10:08:27 AM | Tuesday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// \class 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class CameraMng : MonoBehaviour {

    public static int MAX_LAYER = 10;

    class BlendState {
        public CameraCtrl target;
        public float duration;
        public float timer;
        public float srcWeight;
        public float destWeight;
        // TODO: curve

        public BlendState ( CameraCtrl _target, float _weight, float _duration ) {
            target = _target;
            duration = _duration;
            timer = 0.0f;
            srcWeight = _target.weight;
            destWeight = _weight;
        }

        public bool Step () {
            timer += Time.deltaTime;
            float ratio = timer/duration;

            // target.weight = Mathf.SmoothStep( srcWeight, destWeight, ratio );
            ratio = exEase.Smooth(ratio);
            target.weight = Mathf.Lerp( srcWeight, destWeight, ratio );

            if ( timer >= duration ) {
                target.weight = destWeight;
                return true;
            }

            return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    // serialize
    ///////////////////////////////////////////////////////////////////////////////

    public Camera renderCamera;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////
    
    // NOTE: we support only 10 layer
    List<CameraCtrl>[] layerToCameraCtrls = new List<CameraCtrl>[MAX_LAYER]; 
    List<BlendState>[] layerToBlendStates = new List<BlendState>[MAX_LAYER]; 

    //
    List<BlendState> blendRequestList = new List<BlendState>();

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake () {
        //
        if ( renderCamera == null )
            renderCamera = Camera.main;

        //
        for ( int i = 0; i < MAX_LAYER; ++i ) {
            layerToCameraCtrls[i] = new List<CameraCtrl>();
            layerToBlendStates[i] = new List<BlendState>();
        }

        // TODO: add free move camera for protection
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        for ( int l = MAX_LAYER-1; l >= 0; --l ) {
            List<BlendState> blendStates = layerToBlendStates[l];
            for ( int i = blendStates.Count-1; i >= 0; --i ) {
                BlendState blendState = blendStates[i];
                if ( blendState.Step() ) {
                    blendStates.RemoveAt(i);
                }
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void LateUpdate () {

        int startLayer = -1;
        int endLayer = MAX_LAYER-1;

        //
        for ( int l = MAX_LAYER-1; l >= 0; --l ) {
            List<CameraCtrl> cameraCtrls = layerToCameraCtrls[l];
            float layerWeight = 0.0f;

            //
            for ( int i = cameraCtrls.Count-1; i >= 0; --i ) {
                CameraCtrl cameraCtrl = cameraCtrls[i];

                // remove camera ctrl have zero weight
                if ( cameraCtrl.weight == 0.0f ) {
                    cameraCtrls.RemoveAt(i);
                    continue;
                }

                layerWeight += cameraCtrl.weight;
            }

            //
            if ( layerWeight != 0.0f ) {
                endLayer = l;
            }

            //
            if ( startLayer == -1 && layerWeight >= 1.0f ) {
                startLayer = l;
                // NOTE: we can't break here, otherwise, some camera will not pop up
            }
        }
        if ( startLayer == -1 ) 
            startLayer = 0;

        // DEBUG { 
        exDebugHelper.ScreenPrint("startLayer = " + startLayer + " endLayer = " + endLayer);
        for ( int l = startLayer; l < endLayer+1; ++l ) {
            List<CameraCtrl> cameraCtrls = layerToCameraCtrls[l];
            if ( cameraCtrls.Count == 0 )
                continue;

            // blend camera ctrls in same layer
            for ( int i = 0; i < cameraCtrls.Count; ++i ) {
                CameraCtrl ctrl = cameraCtrls[i];
                exDebugHelper.ScreenPrint( string.Format( "ctrl[{0}] name: {1}, weight = {2}", l, ctrl.cameraAnchor.name, ctrl.weight ) );
            }
        }
        // } DEBUG end 

        //
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        float totalWeight = 0.0f;

        for ( int l = startLayer; l < endLayer+1; ++l ) {
            List<CameraCtrl> cameraCtrls = layerToCameraCtrls[l];
            if ( cameraCtrls.Count == 0 )
                continue;

            Vector3 layerPosition = cameraCtrls[0].cameraAnchor.position;
            Quaternion layerRotation = cameraCtrls[0].cameraAnchor.rotation;
            float layerWeight = cameraCtrls[0].weight;

            // blend camera ctrls in same layer
            for ( int i = 1; i < cameraCtrls.Count; ++i ) {
                CameraCtrl nextCtrl = cameraCtrls[i];

                layerWeight += nextCtrl.weight;
                float ratio = nextCtrl.weight/layerWeight;

                layerPosition = Vector3.Lerp ( layerPosition, nextCtrl.cameraAnchor.position, ratio );
                layerRotation = Quaternion.Slerp ( layerRotation, nextCtrl.cameraAnchor.rotation, ratio );
            }
            totalWeight += layerWeight;

            // blend with prev layer
            if ( l != startLayer ) {
                float ratio = layerWeight/totalWeight;
                position = Vector3.Lerp ( position, layerPosition, ratio );
                rotation = Quaternion.Slerp ( rotation, layerRotation, ratio );
            }
            else {
                position = layerPosition;
                rotation = layerRotation;
            }
        }

        //
        if ( renderCamera != null ) {
            renderCamera.transform.position = position;
            renderCamera.transform.rotation = rotation;
        }

        //
        HandleBlendRequest();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void PushCameraCtrlIfNotExist ( CameraCtrl _cameraCtrl ) {
        // find if the camera ctrl is already in layerToCameraCtrls list, if not, add it
        if ( layerToCameraCtrls[_cameraCtrl.layer].IndexOf(_cameraCtrl) == -1 ) {
            _cameraCtrl.weight = 0.0f;
            layerToCameraCtrls[_cameraCtrl.layer].Add(_cameraCtrl);
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void HandleBlendRequest () {
        for ( int i = 0; i < blendRequestList.Count; ++i ) {
            BlendState blendState = blendRequestList[i];
            CameraCtrl cameraCtrl = blendState.target;
            PushCameraCtrlIfNotExist (cameraCtrl);

            // reset blend state
            blendState.timer = 0.0f;
            blendState.srcWeight = blendState.target.weight;

            layerToBlendStates[cameraCtrl.layer].Add (blendState);
        }
        blendRequestList.Clear();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void PushRequest ( CameraCtrl _cameraCtrl, float _weight, float _duration ) {
        BlendState blendState = null;
        for ( int i = 0; i < blendRequestList.Count; ++i ) {
            BlendState curBlendState = blendRequestList[i];
            if ( curBlendState.target == _cameraCtrl ) {
                blendState = curBlendState;
                break;
            }
        }

        if ( blendState == null ) {
            blendState = new BlendState( _cameraCtrl, _weight, _duration );
            blendRequestList.Add(blendState);
        }
        else {
            blendState.destWeight = _weight;
            blendState.duration = _duration;
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void CrossFade ( CameraCtrl _cameraCtrl, float _duration = 0.3f ) {
        if ( _cameraCtrl.layer >= MAX_LAYER ) {
            Debug.LogError ( string.Format( "Failed to CrossFade CameraCtrl: {0}, camera layer ({1}) out of range. MAX_LAYER is {2}.", 
                                            _cameraCtrl.name,
                                            _cameraCtrl.layer, 
                                            MAX_LAYER ) );
            return;
        }

        // DISABLE { 
        // if ( layerToCameraCtrls[_cameraCtrl.layer].IndexOf(_cameraCtrl) != -1 )
        // {
        //     layerToCameraCtrls[_cameraCtrl.layer].Remove(_cameraCtrl);
        // }
        // layerToCameraCtrls[_cameraCtrl.layer].Add(_cameraCtrl);
        // } DISABLE end 

        PushRequest( _cameraCtrl, 1.0f, _duration );

        foreach ( CameraCtrl cameraCtrl in layerToCameraCtrls[_cameraCtrl.layer] ) {
            if ( cameraCtrl == _cameraCtrl )
                continue;

            PushRequest( cameraCtrl, 0.0f, _duration );
        }
        layerToBlendStates[_cameraCtrl.layer].Clear();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Blend ( CameraCtrl _cameraCtrl, float _weight, float _duration = 0.3f ) {
        if ( _cameraCtrl.layer >= MAX_LAYER ) {
            Debug.LogError ( string.Format( "Failed to CrossFade CameraCtrl: {0}, camera layer ({1}) out of range. MAX_LAYER is {2}.", 
                                            _cameraCtrl.name,
                                            _cameraCtrl.layer, 
                                            MAX_LAYER ) );
            return;
        }

        PushRequest( _cameraCtrl, _weight, _duration );
        // layerToBlendStates[_cameraCtrl.layer].Clear();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void FadeOut ( int _layer, float _duration = 0.3f ) {
        // TODO:
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void FadeOut ( CameraCtrl _cameraCtrl, float _duration = 0.3f ) {
        // TODO:
    }

}

