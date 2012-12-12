// ======================================================================================
// File         : TestStage.cs
// Author       : Wu Jie 
// Last Change  : 11/27/2012 | 18:52:42 PM | Tuesday,November
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

public class TestStage : MonoBehaviour {

    public GameObject player;
    public GameObject OrbitFollowCameraCtrlPrefab;

    public CameraCtrl camCtrl1;
    public CameraCtrl camCtrl2;
    public CameraCtrl camCtrl3;

    void Start () {
        CameraMng cameraMng = Game.instance.GetComponent<CameraMng>();
        if ( cameraMng ) {
            GameObject ctrlGO = Instantiate( OrbitFollowCameraCtrlPrefab, 
                                             Vector3.zero, 
                                             Quaternion.identity ) as GameObject;
            if ( ctrlGO ) {
                OrbitFollowCameraCtrl orbitFollow = ctrlGO.GetComponent<OrbitFollowCameraCtrl>();
                if ( orbitFollow ) {
                    orbitFollow.traceTarget = player.transform;
                    orbitFollow.MoveTo ( player.transform.position 
                                         - player.transform.forward * 10.0f 
                                         + player.transform.up * 10.0f );
                    orbitFollow.Apply ();

                    camCtrl1 = orbitFollow;
                    cameraMng.CrossFade(camCtrl1);
                }
            }
        }
    }

    void Update () {
        if ( Input.GetKeyDown(KeyCode.Alpha1) ) {
            CameraMng cameraMng = Game.instance.GetComponent<CameraMng>();
            cameraMng.CrossFade( camCtrl1, 0.8f );
        }
        if ( Input.GetKeyDown(KeyCode.Alpha2) ) {
            CameraMng cameraMng = Game.instance.GetComponent<CameraMng>();
            cameraMng.CrossFade( camCtrl2, 0.8f );
        }
        if ( Input.GetKeyDown(KeyCode.Alpha3) ) {
            CameraMng cameraMng = Game.instance.GetComponent<CameraMng>();
            cameraMng.CrossFade( camCtrl3, 0.8f );
        }
    }
}

