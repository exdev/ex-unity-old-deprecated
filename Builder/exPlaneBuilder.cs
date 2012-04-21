// ======================================================================================
// File         : exPlaneBuilder.cs
// Author       : Wu Jie 
// Last Change  : 06/02/2011 | 08:41:56 AM | Thursday,June
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]
public class exPlaneBuilder : MonoBehaviour {

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public enum Plane {
        XY,
        XZ,
        ZY
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public enum Anchor {
		TopLeft,
		TopCenter,
		TopRight,
		MidLeft,
		MidCenter,
		MidRight,
		BotLeft,
		BotCenter,
		BotRight,
    }

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    public int row = 1;
    public int col = 1;
    public Vector2 size;
    public Plane planeType = Plane.XY;
    public Anchor anchor = Anchor.MidCenter;
    public bool customUV = false;
    public Vector2 uvSize = Vector2.one;
}
