// ======================================================================================
// File         : exMenuItems.cs
// Author       : Wu Jie 
// Last Change  : 11/28/2012 | 14:09:49 PM | Wednesday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

///////////////////////////////////////////////////////////////////////////////
// \class 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public static class exMenuItems {

    [MenuItem("Assets/Create/ex/Layer Manager")]
    static void CreateLayerMng () { 
        exGenericAssetUtility<LayerMng>.CreateInCurrentDirectory("New LayerMng"); 
    }

    [MenuItem("Assets/Create/ex/ActorInfo")]
    static void CreateActorInfo () { 
        exGenericAssetUtility<ActorInfo>.CreateInCurrentDirectory("New ActorInfo"); 
    }

    [MenuItem("Assets/Create/ex/MovementState")]
    static void CreateMovementState () { 
        exGenericAssetUtility<MovementState>.CreateInCurrentDirectory("New MovementState"); 
    }

    [MenuItem("Assets/Create/ex/InputListener")]
    static void CreateInputListener () {
        exGenericAssetUtility<InputListener>.CreateInCurrentDirectory("New InputListener");
    }
}
