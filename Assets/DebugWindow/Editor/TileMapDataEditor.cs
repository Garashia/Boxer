using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileMapData))]
public class TileMapDataEditor : Editor
{
    private Vector2 _scrollPosition = Vector2.zero;
    private TileMapData obj;

    private void OnEnable()
    {
        // 有効になった時に対象を確保しておく
        obj = target as TileMapData;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (obj == null)
        {
            EditorGUILayout.HelpBox("TileMapData is null.", MessageType.Error);
            return;
        }

        // タイルマップ情報を視覚化
        EditorGUILayout.LabelField("Tile Map Visualization", EditorStyles.boldLabel);
        int gridSize = 10; // 固定サイズで視覚化
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        for (int y = 0; y < obj.Height; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < obj.Width; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                TileType tile = obj.GetTile(position);

                Color tileColor = tile switch
                {
                    TileType.Area => Color.green,
                    TileType.Shop => Color.blue,
                    TileType.Start => Color.cyan,
                    TileType.Goal => Color.magenta,
                    TileType.Move => Color.yellow,
                    TileType.Warp => Color.gray,
                    TileType.Boss => new Color(0.5f, 0, 0),
                    TileType.Chara => new Color(0.5f, 0.5f, 0),
                    TileType.Wall => Color.black,
                    TileType.Item => new Color(1.0f, 192.0f / 255.0f, 203.0f),
                    _ => Color.white,
                };

                GUIStyle style = new GUIStyle(GUI.skin.box);
                style.normal.background = Texture2D.whiteTexture;
                GUI.backgroundColor = tileColor;

                GUILayout.Box("", style, GUILayout.Width(gridSize), GUILayout.Height(gridSize));
                GUI.backgroundColor = Color.white; // Reset color
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // タイルマップエディターの呼び出しボタン
        if (GUILayout.Button("Open in TileMap Editor"))
        {
            TileMapEditor editor = (TileMapEditor)EditorWindow.GetWindow(typeof(TileMapEditor));
            editor.titleContent = new GUIContent("Tile Map Editor");
            editor.Focus();
            editor.LoadTileMapData(obj);
        }

        EditorUtility.SetDirty(obj);
        serializedObject.ApplyModifiedProperties();

    }
}

