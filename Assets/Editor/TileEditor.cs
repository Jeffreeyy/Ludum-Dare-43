using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Still draw default inspector
        DrawDefaultInspector();

        // Get a reference to the HexGrid
        Tile tile = (Tile)target;

        // Add spacing
        for (int i = 0; i < 4; i++)
            EditorGUILayout.Space();

        // Add a Button to create the Grid
        if (GUILayout.Button("Generate"))
            tile.Generate();

        if (GUILayout.Button("Clear"))
            tile.Clear();
    }
}