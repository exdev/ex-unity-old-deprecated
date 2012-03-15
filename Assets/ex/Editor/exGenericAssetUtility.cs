// ======================================================================================
// File         : exGenericAssetUtility.cs
// Author       : Wu Jie 
// Last Change  : 02/19/2012 | 21:22:54 PM | Sunday,February
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

public static class exGenericAssetUtility<T> where T : ScriptableObject {

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static T Create ( string _path, string _name ) {
        // check if the asset is valid to create
        if ( new DirectoryInfo(_path).Exists == false ) {
            Debug.LogError ( "can't create asset, path not found" );
            return null;
        }
        if ( string.IsNullOrEmpty(_name) ) {
            Debug.LogError ( "can't create asset, the name is empty" );
            return null;
        }
        string assetPath = Path.Combine( _path, _name + ".asset" );

        //
        T newAsset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(newAsset, assetPath);
        Selection.activeObject = newAsset;
        return newAsset;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static void CreateInCurrentDirectory ( string _assetName ) {
        // get current selected directory
        string assetPath = "Assets";
        if ( Selection.activeObject ) {
            assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if ( Path.GetExtension(assetPath) != "" ) {
                assetPath = Path.GetDirectoryName(assetPath);
            }
        }

        //
        bool doCreate = true;
        string path = Path.Combine( assetPath, _assetName + ".asset" );
        FileInfo fileInfo = new FileInfo(path);
        if ( fileInfo.Exists ) {
            doCreate = EditorUtility.DisplayDialog( _assetName + " already exists.",
                                                    "Do you want to overwrite the old one?",
                                                    "Yes", "No" );
        }
        if ( doCreate ) {
            T newAsset = Create ( assetPath, _assetName );
            Selection.activeObject = newAsset;
            // EditorGUIUtility.PingObject(border);
        }
    }
}
