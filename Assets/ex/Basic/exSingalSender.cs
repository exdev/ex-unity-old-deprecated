// ======================================================================================
// File         : exSingalSender.cs
// Author       : Wu Jie 
// Last Change  : 02/19/2012 | 21:21:58 PM | Sunday,February
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

///////////////////////////////////////////////////////////////////////////////
// exSingalSender
///////////////////////////////////////////////////////////////////////////////

public class exSingalSender {

    ///////////////////////////////////////////////////////////////////////////////
    // ReceiverInfo
    ///////////////////////////////////////////////////////////////////////////////

    [System.Serializable]
    public class ReceiverInfo {

        ///////////////////////////////////////////////////////////////////////////////
        // properties
        ///////////////////////////////////////////////////////////////////////////////

        public GameObject receiver;
        public string action = "OnSignal";
        public float delay = 0.0f;

        ///////////////////////////////////////////////////////////////////////////////
        // functions
        ///////////////////////////////////////////////////////////////////////////////

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        public void Send ( MonoBehaviour _sender ) {
            if ( delay <= Mathf.Epsilon ) {
                SendMessage (_sender);
            }
            else {
                _sender.StartCoroutine ( SendMessageDelay(_sender) );
            }
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected IEnumerator SendMessageDelay ( MonoBehaviour _sender ) {
            yield return new WaitForSeconds ( delay );
            SendMessage (_sender);
        }

        // ------------------------------------------------------------------ 
        // Desc: 
        // ------------------------------------------------------------------ 

        protected void SendMessage ( MonoBehaviour _sender ) {
            if ( receiver ) {
                receiver.SendMessage(action);
            }
            else 
                Debug.LogWarning ( "No receiver of signal \"" + action 
                                   + "\" on object " + _sender.name 
                                   + " (" + _sender.GetType().Name + ")", 
                                   _sender );
        }
    }

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    public List<ReceiverInfo> receiverInfoList = new List<ReceiverInfo>(); 

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    public void Emit ( MonoBehaviour _sender ) {
        foreach ( ReceiverInfo ri in receiverInfoList ) {
            ri.Send(_sender);
        }
    } 

}
