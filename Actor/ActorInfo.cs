// ======================================================================================
// File         : ActorInfo.cs
// Author       : Wu Jie 
// Last Change  : 11/28/2012 | 14:04:09 PM | Wednesday,November
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// \class ActorInfo 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class ActorInfo : ScriptableObject {
    public string actorName = "";
    public float height = 1.0f;
    public float radius = 0.2f;
    public float maxMoveSpeed = 1.0f;
}
