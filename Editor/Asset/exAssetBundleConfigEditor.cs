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
        serializedObject.Update ();

            EditorGUILayout.PropertyField (autoVersionProp);
            EditorGUILayout.PropertyField (versionProp);
            EditorGUILayout.PropertyField (objInfoListProp);
            if ( objInfoListProp.isExpanded ) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.IntField ( "Size", objInfoListProp.arraySize);
                for ( int i = 0; i < objInfoListProp.arraySize; ++i ) {
                    SerializedProperty elementProp = objInfoListProp.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField (elementProp);
                }
                EditorGUI.indentLevel = 0;
            }

        serializedObject.ApplyModifiedProperties ();
    }

}
