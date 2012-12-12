// ======================================================================================
// File         : LayerMng.cs
// Author       : Wu Jie 
// Last Change  : 11/23/2012 | 11:32:02 AM | Friday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// \class LayerMng
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class LayerMng : ScriptableObject {

    [System.Serializable]
    public class LayerMaskInfo {
        public string name;
        public List<int> layerIDs = new List<int>();
    }

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    public List<LayerMaskInfo> layerMaskInfos = new List<LayerMaskInfo>();

    ///////////////////////////////////////////////////////////////////////////////
    // 
    ///////////////////////////////////////////////////////////////////////////////

    protected Dictionary<string,int> nameToLayerMask = new Dictionary<string,int>();

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnEnable () {
        for ( int i = 0; i < layerMaskInfos.Count; ++i ) {
            int layerMask = 0;
            LayerMaskInfo layerMaskInfo = layerMaskInfos[i];

            for ( int j = 0; j < layerMaskInfo.layerIDs.Count; ++j ) {
                int layerID = layerMaskInfo.layerIDs[j];
                layerMask |= 1 << layerID;
            }

            nameToLayerMask[layerMaskInfo.name] = layerMask;
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public int GetLayerMask ( string _name ) {
        if ( nameToLayerMask.ContainsKey(_name) )
            return nameToLayerMask[_name];
        return 0;
    }
}
