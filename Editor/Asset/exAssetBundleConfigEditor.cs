// ======================================================================================
// File         : exAssetBundleConfigEditor.cs
// Author       : Wu Jie 
// Last Change  : 06/18/2012 | 19:58:04 PM | Monday,June
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
// defines
///////////////////////////////////////////////////////////////////////////////

[CustomEditor(typeof(exAssetBundleConfig))]
[CanEditMultipleObjects]
public class exAssetBundleConfigEditor : Editor {

    SerializedProperty autoVersionProp;
    SerializedProperty versionProp;
    SerializedProperty objInfoListProp;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    [MenuItem ("Assets/Create/ex/AssetBundle Config File")]
    public static void CreateAsset () {
        exGenericAssetUtility<exAssetBundleConfig>.CreateInCurrentDirectory("New AssetBundle Config");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnEnable () {
        autoVersionProp = serializedObject.FindProperty ("autoVersion");
        versionProp = serializedObject.FindProperty ("version");
        objInfoListProp = serializedObject.FindProperty ("objInfoList");
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public override void OnInspectorGUI () {
        EditorGUIUtility.LookLikeInspector ();

        EditorGUILayout.PropertyField (autoVersionProp);
        EditorGUILayout.PropertyField (versionProp);

        EditorGUILayout.PropertyField (objInfoListProp);

        // 
        serializedObject.ApplyModifiedProperties ();
    }

}
