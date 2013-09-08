// ======================================================================================
// File         : exPlaneBuilderEditor.cs
// Author       : Wu Jie 
// Last Change  : 08/06/2011 | 22:05:41 PM | Saturday,August
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

[CustomEditor(typeof(exPlaneBuilder))]
public class exPlaneBuilderEditor : Editor {

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
        exPlaneBuilder editTarget = target as exPlaneBuilder;

        // create new mesh
        Mesh newMesh = new Mesh();
        newMesh.name = "builtin-plane";

        // init essential variables
        float cellWidth = editTarget.size.x / (float)editTarget.col;
        float cellHeight = editTarget.size.y / (float)editTarget.row;
        float halfWidth = (float)editTarget.size.x / (float)2.0f;
        float halfHeight = (float)editTarget.size.y / (float)2.0f;
        float offsetX = 0.0f;
        float offsetY = 0.0f;
        int numVerts = (editTarget.col+1) * (editTarget.row+1);

        //
        Vector3[] verts = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uvs = new Vector2[numVerts];
        Color[] colors = new Color[numVerts];

        // calculate anchor offset
        switch ( editTarget.anchor ) {
        case exPlaneBuilder.Anchor.TopLeft:  
            offsetX = -halfWidth;
            offsetY = -halfHeight;
            break;
        case exPlaneBuilder.Anchor.TopCenter:
            offsetX = 0.0f;
            offsetY = -halfHeight;
            break;
        case exPlaneBuilder.Anchor.TopRight: 
            offsetX = halfWidth;
            offsetY = -halfHeight;
            break;
        case exPlaneBuilder.Anchor.MidLeft:
            offsetX = -halfWidth;
            offsetY = 0.0f;
            break;
        case exPlaneBuilder.Anchor.MidCenter:
            offsetX = 0.0f;
            offsetY = 0.0f;
            break;
        case exPlaneBuilder.Anchor.MidRight: 
            offsetX = halfWidth;
            offsetY = 0.0f;
            break;
        case exPlaneBuilder.Anchor.BotLeft:
            offsetX = -halfWidth;
            offsetY = halfHeight;
            break;
        case exPlaneBuilder.Anchor.BotCenter:
            offsetX = 0.0f;
            offsetY = halfHeight;
            break;
        case exPlaneBuilder.Anchor.BotRight:
            offsetX = halfWidth;
            offsetY = halfHeight;
            break;
        }

        // build vertices
        for ( int r = 0; r < (editTarget.row+1); ++r ) {
            for ( int c = 0; c < (editTarget.col+1); ++c ) {
                int i = r * (editTarget.col+1) + c;
                float x = -halfWidth + c * cellWidth;
                float y = halfHeight - r * cellHeight;

                // build verts, normals, and uvs
                switch ( editTarget.planeType ) {
                case exPlaneBuilder.Plane.XY:
                    verts[i] = new Vector3( x - offsetX, y + offsetY, 0.0f );
                    normals[i] = new Vector3( 0.0f, 0.0f, -1.0f );
                    break;
                case exPlaneBuilder.Plane.XZ:
                    verts[i] = new Vector3( x - offsetX, 0.0f, y + offsetY );
                    normals[i] = new Vector3( 0.0f, 1.0f, 0.0f );
                    break;
                case exPlaneBuilder.Plane.ZY:
                    verts[i] = new Vector3( 0.0f, y + offsetY, x - offsetX );
                    normals[i] = new Vector3( 1.0f, 0.0f, 0.0f );
                    break;
                }

                // build uvs and colors
                uvs[i] = new Vector2 ( (x+halfWidth)/(float)editTarget.uvSize.x, (y+halfHeight)/(float)editTarget.uvSize.y ); 
                colors[i] = Color.white;
            }
        }
        newMesh.vertices = verts;
        newMesh.uv = uvs;
        newMesh.colors = colors;
        newMesh.normals = normals;

        // build indices  
        int numIndices = (editTarget.row) * (editTarget.col) * 6;
        int[] indices = new int[numIndices];
        for ( int r = 0; r < editTarget.row; ++r ) {
            for ( int c = 0; c < editTarget.col; ++c ) {
                int i = (r * editTarget.col + c) * 6;

                indices[i]   = r     * (editTarget.col+1) + c;
                indices[i+1] = r     * (editTarget.col+1) + (c+1);
                indices[i+2] = (r+1) * (editTarget.col+1) + c;
                indices[i+3] = (r+1) * (editTarget.col+1) + c;
                indices[i+4] = r     * (editTarget.col+1) + (c+1);
                indices[i+5] = (r+1) * (editTarget.col+1) + (c+1);
            }
        }
        newMesh.triangles = indices; 
        
        //
        editTarget.GetComponent<MeshFilter>().sharedMesh = newMesh; 

        // if we have mesh collider, update it.
        MeshCollider meshCol = editTarget.GetComponent<MeshCollider>();
        if ( meshCol )
            meshCol.sharedMesh = newMesh;

        // if we have box collider, update it.
        BoxCollider boxCol = editTarget.GetComponent<BoxCollider>();
        if ( boxCol ) {
            boxCol.center = Vector3.zero;
            switch ( editTarget.planeType ) {
            case exPlaneBuilder.Plane.XY:
                boxCol.size = new Vector3( editTarget.size.x, editTarget.size.y, 0.2f );
                break;
            case exPlaneBuilder.Plane.XZ:
                boxCol.size = new Vector3( editTarget.size.x, 0.2f, editTarget.size.y );
                break;
            case exPlaneBuilder.Plane.ZY:
                boxCol.size = new Vector3( 0.2f, editTarget.size.y, editTarget.size.x );
                break;
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public override void OnInspectorGUI () {
        exPlaneBuilder editTarget = target as exPlaneBuilder;
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
        Vector2 newSize = EditorGUILayout.Vector2Field( "Size", editTarget.size );
        if ( newSize != editTarget.size ) {
            if ( newSize.x < 1.0f )
                newSize.x = 1.0f;
            if ( newSize.y < 1.0f )
                newSize.y = 1.0f;
            editTarget.size = newSize;
            isDirty = true;
        }

        EditorGUIUtility.LookLikeInspector ();
        EditorGUI.indentLevel = 1;

        // col
        // EditorGUILayout.BeginHorizontal();
            int newCol = EditorGUILayout.IntField( "Col", editTarget.col );
            if ( newCol != editTarget.col ) {
                if ( newCol < 1 )
                    newCol = 1;
                editTarget.col = newCol;
                isDirty = true;
            }

            // row
            int newRow = EditorGUILayout.IntField( "Row", editTarget.row );
            if ( newRow != editTarget.row ) {
                if ( newRow < 1 )
                    newRow = 1;
                editTarget.row = newRow;
                isDirty = true;
            }
        // EditorGUILayout.EndHorizontal();

        // custom uv
        editTarget.customUV = EditorGUILayout.Toggle( "Customize UV", editTarget.customUV );
        if ( editTarget.customUV ) {
            // uv width height
            Vector2 newUVSize = EditorGUILayout.Vector2Field( "UV", editTarget.uvSize );
            if ( newUVSize != editTarget.uvSize ) {
                editTarget.uvSize = newUVSize;
                isDirty = true;
            }
        }
        else if ( editTarget.uvSize != editTarget.size ) {
            editTarget.uvSize = editTarget.size;
            isDirty = true;
        }

        // plane type
        exPlaneBuilder.Plane newType = (exPlaneBuilder.Plane)EditorGUILayout.EnumPopup( "exPlaneBuilder Plane", editTarget.planeType );
        if ( newType != editTarget.planeType ) {
            editTarget.planeType = newType;
            isDirty = true;
        }

        // anchor
        exPlaneBuilder.Anchor newAnchor = (exPlaneBuilder.Anchor)EditorGUILayout.EnumPopup( "Anchor", editTarget.anchor );
        if ( newAnchor != editTarget.anchor ) {
            editTarget.anchor = newAnchor;
            isDirty = true;
        }

        // add mesh collider button
        GUI.enabled = editTarget.GetComponent<Collider>() == null;
        if ( GUILayout.Button("Add Mesh Collider", GUILayout.Width(120) ) ) {
            editTarget.gameObject.AddComponent<MeshCollider>();
            isDirty = true;
        }
        GUI.enabled = true;

        // add box collider button
        GUI.enabled = editTarget.GetComponent<Collider>() == null;
        if ( GUILayout.Button("Add Box Collider", GUILayout.Width(120) ) ) {
            editTarget.gameObject.AddComponent<BoxCollider>();
            isDirty = true;
        }
        GUI.enabled = true;

        // if dirty, build it.
        // DISABLE { 
        // GUIStyle styleBuildBtn = GUI.skin.GetStyle("Button");
        // Color old = styleBuildBtn.normal.textColor;
        // styleBuildBtn.normal.textColor = isDirty ? Color.black : Color.gray;
        // if ( GUILayout.Button("Build", GUILayout.Width(100) ) && isDirty ) {
        //     Build();
        // }
        // styleBuildBtn.normal.textColor = old;
        // } DISABLE end 
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
