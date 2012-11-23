// ======================================================================================
// File         : Test.cs
// Author       : Wu Jie 
// Last Change  : 11/19/2012 | 17:47:26 PM | Monday,November
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

public class Test : MonoBehaviour {
    [Multiline]
    public string playerBiography = "Please enter your biography";

    // [Popup ("Warrior", "Mage", "Archer", "Ninja")]
    // public string @class = "Warrior";

    // [Popup ("Human/Local", "Human/Network", "AI/Easy", "AI/Normal", "AI/Hard")]
    // public string controller;

    [Range (0, 100)]
    public float health = 100;

    // [Regex (@"^(?:\d{1,3}\.){3}\d{1,3}$", "Invalid IP address!\nExample: '127.0.0.1'")]
    // public string serverAddress = "192.168.0.1";

    // [Compact]
    // public Vector3 forward = Vector3.forward;

    // [Compact]
    // public Vector3 target = new Vector3 (100, 200, 300);

    // [Angle]
    // public float turnRate = (Mathf.PI / 3) * 2;
}
