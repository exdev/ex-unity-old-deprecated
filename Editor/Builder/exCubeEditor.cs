// ======================================================================================
// File         : exCubeEditor.cs
// Author       : Wu Jie 
// Last Change  : 06/02/2011 | 15:32:46 PM | Thursday,June
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

[CustomEditor(typeof(exCubeBuilder))]
public class exCubeEditor : Editor {

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    bool isDirty = false; 

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Build () {
        isDirty = false;
        exCubeBuilder editTarget = target as exCubeBuilder;

        // create new mesh
        Mesh newMesh = new Mesh();
        newMesh.name = "builtin-cube";

        Vector3 center = editTarget.center;
        Vector3 halfSize = editTarget.size / 2.0f;

        /* 
         *          4---------5
         *         / |       /|
         *        /  |      / |
         *       /   |     /  |
         *      0---------1   |
         *      |    |    |   |
         *      |    7----|---6
         *      |   /     |  /
         *      |  /      | /
         *      | /       |/ 
         *      3---------2
         */

        // build vertices
        newMesh.vertices = new Vector3[] {
            // front face
            center + new Vector3(  halfSize.x,  halfSize.y,  halfSize.z ), // 0
            center + new Vector3( -halfSize.x,  halfSize.y,  halfSize.z ), // 1
            center + new Vector3( -halfSize.x, -halfSize.y,  halfSize.z ), // 2
            center + new Vector3(  halfSize.x, -halfSize.y,  halfSize.z ), // 3

            // top face
            center + new Vector3(  halfSize.x,  halfSize.y, -halfSize.z ), // 4
            center + new Vector3( -halfSize.x,  halfSize.y, -halfSize.z ), // 5
            center + new Vector3( -halfSize.x,  halfSize.y,  halfSize.z ), // 6
            center + new Vector3(  halfSize.x,  halfSize.y,  halfSize.z ), // 7

            // rear face
            center + new Vector3( -halfSize.x,  halfSize.y, -halfSize.z ), // 8 
            center + new Vector3(  halfSize.x,  halfSize.y, -halfSize.z ), // 9 
            center + new Vector3(  halfSize.x, -halfSize.y, -halfSize.z ), // 10
            center + new Vector3( -halfSize.x, -halfSize.y, -halfSize.z ), // 11

            // bottom face
            center + new Vector3(  halfSize.x, -halfSize.y,  halfSize.z ), // 12
            center + new Vector3( -halfSize.x, -halfSize.y,  halfSize.z ), // 13
            center + new Vector3( -halfSize.x, -halfSize.y, -halfSize.z ), // 14
            center + new Vector3(  halfSize.x, -halfSize.y, -halfSize.z ), // 15

            // left face
            center + new Vector3( -halfSize.x,  halfSize.y,  halfSize.z ), // 16
            center + new Vector3( -halfSize.x,  halfSize.y, -halfSize.z ), // 17
            center + new Vector3( -halfSize.x, -halfSize.y, -halfSize.z ), // 18
            center + new Vector3( -halfSize.x, -halfSize.y,  halfSize.z ), // 19

            // right face
            center + new Vector3(  halfSize.x,  halfSize.y, -halfSize.z ), // 20
            center + new Vector3(  halfSize.x,  halfSize.y,  halfSize.z ), // 21
            center + new Vector3(  halfSize.x, -halfSize.y,  halfSize.z ), // 22
            center + new Vector3(  halfSize.x, -halfSize.y, -halfSize.z ), // 23
        };

        // build uvs
        newMesh.uv = new Vector2[] {
            // front face
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 1.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 0.0f, 0.0f ),

            // top face
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 1.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 0.0f, 0.0f ),

            // rear face
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 0.0f, 0.0f ),
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 1.0f ),

            // bottom face
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 1.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 0.0f, 0.0f ),

            // left face
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 1.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 0.0f, 0.0f ),

            // right face
            new Vector2( 0.0f, 1.0f ),
            new Vector2( 1.0f, 1.0f ),
            new Vector2( 1.0f, 0.0f ),
            new Vector2( 0.0f, 0.0f ),
        };

        // build normals
        newMesh.normals = new Vector3[] {
            // front face
            new Vector3( 0.0f, 0.0f, 1.0f ),
            new Vector3( 0.0f, 0.0f, 1.0f ),
            new Vector3( 0.0f, 0.0f, 1.0f ),
            new Vector3( 0.0f, 0.0f, 1.0f ),

            // top face
            new Vector3( 0.0f, 1.0f, 0.0f ),
            new Vector3( 0.0f, 1.0f, 0.0f ),
            new Vector3( 0.0f, 1.0f, 0.0f ),
            new Vector3( 0.0f, 1.0f, 0.0f ),

            // rear face
            new Vector3( 0.0f, 0.0f, -1.0f ),
            new Vector3( 0.0f, 0.0f, -1.0f ),
            new Vector3( 0.0f, 0.0f, -1.0f ),
            new Vector3( 0.0f, 0.0f, -1.0f ),

            // bottom face
            new Vector3( 0.0f, -1.0f, 0.0f ),
            new Vector3( 0.0f, -1.0f, 0.0f ),
            new Vector3( 0.0f, -1.0f, 0.0f ),
            new Vector3( 0.0f, -1.0f, 0.0f ),

            // left face
            new Vector3( -1.0f, 0.0f, 0.0f ),
            new Vector3( -1.0f, 0.0f, 0.0f ),
            new Vector3( -1.0f, 0.0f, 0.0f ),
            new Vector3( -1.0f, 0.0f, 0.0f ),

            // right face
            new Vector3( 1.0f, 0.0f, 0.0f ),
            new Vector3( 1.0f, 0.0f, 0.0f ),
            new Vector3( 1.0f, 0.0f, 0.0f ),
            new Vector3( 1.0f, 0.0f, 0.0f ),
        };

        // build the colors
        newMesh.colors = new Color [] {
            Color.white, Color.white, Color.white, Color.white,
            Color.white, Color.white, Color.white, Color.white,
            Color.white, Color.white, Color.white, Color.white,
            Color.white, Color.white, Color.white, Color.white,
            Color.white, Color.white, Color.white, Color.white,
            Color.white, Color.white, Color.white, Color.white,
        };

        // build indices  
        newMesh.triangles = new int [] {
            0, 1, 3, 3, 1, 2, // front
            4, 5, 7, 7, 5, 6, // top
            8, 9, 11, 11, 9, 10, // rear
            12, 13, 15, 15, 13, 14, // bottom
            16, 17, 19, 19, 17, 18, // left
            20, 21, 23, 23, 21, 22, // right
        }; 

        //
        editTarget.GetComponent<MeshFilter>().sharedMesh = newMesh; 

        // if we have mesh collider, update it either
        BoxCollider boxCol = editTarget.GetComponent<BoxCollider>();
        if ( boxCol ) {
            boxCol.size = editTarget.size;
            boxCol.center = editTarget.center;
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public override void OnInspectorGUI () {
        exCubeBuilder editTarget = target as exCubeBuilder;
        EditorGUILayout.Space ();

        // check if we can build the mesh 
        if ( editTarget.GetComponent<MeshRenderer>() == null ||
             editTarget.GetComponent<MeshFilter>() == null ) 
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.red;
            GUILayout.Label( "Can't find MeshRenderer and MeshFilter in edit editTarget", style );
            return;
        }

        // first time build.
        if ( editTarget.GetComponent<MeshFilter>().sharedMesh == null ) {
            isDirty = true;
        }

        // size
        Vector3 newSize = EditorGUILayout.Vector3Field( "Size", editTarget.size );
        if ( newSize != editTarget.size ) {
            if ( newSize.x < 1.0f )
                newSize.x = 1.0f;
            if ( newSize.y < 1.0f )
                newSize.y = 1.0f;
            if ( newSize.z < 1.0f )
                newSize.z = 1.0f;
            editTarget.size = newSize;
            isDirty = true;
        }

        // center
        Vector3 newCenter = EditorGUILayout.Vector3Field( "Center", editTarget.center );
        if ( newCenter != editTarget.center ) {
            editTarget.center = newCenter;
            isDirty = true;
        }

        // add collider button
        GUI.enabled = editTarget.GetComponent<Collider>() == null;
        if ( GUILayout.Button("Add Box Collider", GUILayout.Width(150) ) ) {
            editTarget.gameObject.AddComponent<BoxCollider>();
            isDirty = true;
        }
        GUI.enabled = true;

        // if dirty, build it.
        if ( isDirty ) {
            Build();
        }
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void OnSceneGUI () {
        ProcessEvent();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void ProcessEvent () {
        // TODO:
    }

}

