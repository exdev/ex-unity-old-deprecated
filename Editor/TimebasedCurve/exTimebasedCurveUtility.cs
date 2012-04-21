// ======================================================================================
// File         : exTimebasedCurveUtility.cs
// Author       : Wu Jie 
// Last Change  : 08/30/2011 | 10:28:56 AM | Tuesday,August
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

public static class exTimebasedCurveUtility {

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    [MenuItem ("Assets/Create/ex2D Curve Info (Timebased)")]
    public static void Create () {
        exTimebasedCurveInfo newCurve = Create ( exEditorHelper.GetCurrentDirectory(), "New CurveInfo" );
        EditorGUIUtility.PingObject(newCurve);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static exTimebasedCurveInfo Create ( string _path, string _name ) {
        //
        if ( new DirectoryInfo(_path).Exists == false ) {
            Debug.LogError ( "can't create asset, path not found" );
            return null;
        }
        if ( string.IsNullOrEmpty(_name) ) {
            Debug.LogError ( "can't create asset, the name is empty" );
            return null;
        }
        string assetPath = Path.Combine( _path, _name + ".asset" );

        // check if create the asset
        FileInfo fileInfo = new FileInfo(assetPath);
        if ( fileInfo.Exists ) {
            if ( EditorUtility.DisplayDialog( _name + " already exists.",
                                              "Do you want to overwrite the old one?",
                                              "Yes", 
                                              "No" ) == false )
            {
                return null;
            }
        }

        //
        exTimebasedCurveInfo newCurve = ScriptableObject.CreateInstance<exTimebasedCurveInfo>();
        AssetDatabase.CreateAsset(newCurve, assetPath);
        Selection.activeObject = newCurve;

        return newCurve;
    }
}
