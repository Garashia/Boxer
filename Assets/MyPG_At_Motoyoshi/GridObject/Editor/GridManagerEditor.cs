using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private GridManager obj;
    private ReorderableList gridList;
    private bool isString = false;

    private void OnEnable()
    {
        obj = (GridManager)target;

        gridList = new ReorderableList(obj.Grids, typeof(GridObject), true, true, false, false)
        {
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Grid List");
            },

            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                // SerializedProperty element = gridList.serializedProperty.GetArrayElementAtIndex(index);
                GridObject grid = obj.Grids[index];

                // 基本的な高さと幅の計算
                float lineHeight = EditorGUIUtility.singleLineHeight + 4;
                Rect fieldRect = new Rect(rect.x, rect.y + 2, rect.width, lineHeight);

                // フォールドアウト
                grid.Open = EditorGUI.Foldout(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), grid.Open, $"Grid {index + 1}");

                if (grid.Open)
                {
                    EditorGUI.indentLevel++;

                    // Position
                    fieldRect.y += lineHeight;
                    grid.transform.localPosition = EditorGUI.Vector3Field(fieldRect, "Position", grid.transform.localPosition);

                    // Rotation
                    fieldRect.y += lineHeight;
                    grid.transform.localEulerAngles = EditorGUI.Vector3Field(fieldRect, "Rotation", grid.transform.localEulerAngles);

                    // ボタン群（Front, Back, Right, Left）
                    DrawDirectionalButtons(grid, ref fieldRect, lineHeight);

                    // Destroy Button
                    fieldRect.y += lineHeight;
                    if (GUI.Button(new Rect(fieldRect.x, fieldRect.y, fieldRect.width / 2 - 5, lineHeight), "Destroy"))
                    {
                        DestroyImmediate(grid.gameObject);
                        obj.Grids.RemoveAt(index);
                        return;
                    }

                    // GridDown Button
                    if (GUI.Button(new Rect(fieldRect.x + fieldRect.width / 2 + 5, fieldRect.y, fieldRect.width / 2 - 5, lineHeight), "Grid Down"))
                    {
                        grid.GridDown();
                    }
                    // fieldRect.y += lineHeight;

                    EditorGUI.indentLevel--;
                }
            },

            elementHeightCallback = index =>
            {
                GridObject grid = obj.Grids[index];
                return grid.Open ? EditorGUIUtility.singleLineHeight * 6 + 30 : EditorGUIUtility.singleLineHeight + 10;
            },

            onAddCallback = list =>
            {
                CreateGrid();
            },

            onRemoveCallback = list =>
            {
                if (list.index >= 0 && list.index < obj.Grids.Count)
                {
                    DestroyImmediate(obj.Grids[list.index].gameObject);
                    obj.Grids.RemoveAt(list.index);
                }
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        isString = EditorGUILayout.Toggle("DebugString", isString);

        obj.GridScale = EditorGUILayout.Vector3Field("Scale:", obj.GridScale);

        if (GUILayout.Button("Delete All Grids"))
        {
            GridDestroy(obj.Grids);
        }

        gridList.DoLayoutList();

        if (GUILayout.Button("Create Grid"))
        {
            CreateGrid();
        }

        obj.MazeObject = (TileMapData)EditorGUILayout.ObjectField("Maze", obj.MazeObject, typeof(TileMapData), true);
        if (obj.MazeObject != null && GUILayout.Button("Spawn Maze"))
        {
            SpawnMaze();
        }

        if (obj.Grids.Count > 0)
        {
            obj.Floor = (GameObject)EditorGUILayout.ObjectField("Floor", obj.Floor, typeof(GameObject), true);
            obj.Wall = (GameObject)EditorGUILayout.ObjectField("Wall", obj.Wall, typeof(GameObject), true);
            obj.Corner = (GameObject)EditorGUILayout.ObjectField("Corner", obj.Corner, typeof(GameObject), true);
            obj.TextObject = (GameObject)EditorGUILayout.ObjectField("Text", obj.TextObject, typeof(GameObject), true);
            if (GUILayout.Button("Spawn Objects"))
            {
                obj.PushObjectSpawn();
            }
        }

        EditorUtility.SetDirty(obj);
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDirectionalButtons(GridObject grid, ref Rect fieldRect, float lineHeight)
    {
        fieldRect.y += lineHeight;

        EditorGUI.LabelField(fieldRect, "Add Neighbor:");
        fieldRect.y += lineHeight;

        float buttonWidth = fieldRect.width / 4 - 5;
        #region 方向
        using (new EditorGUI.DisabledScope(obj.IsAdjacent(grid.GridPoint, Vector2Int.up)))
        {
            if (GUI.Button(new Rect(fieldRect.x, fieldRect.y, buttonWidth, lineHeight), "Front"))
            {
                AddObject(obj.Grids, grid.GridPoint, Vector2Int.up, Vector3.forward, grid.transform);
            }
        }

        using (new EditorGUI.DisabledScope(obj.IsAdjacent(grid.GridPoint, Vector2Int.down)))
        {
            if (GUI.Button(new Rect(fieldRect.x + buttonWidth + 5, fieldRect.y, buttonWidth, lineHeight), "Back"))
            {
                AddObject(obj.Grids, grid.GridPoint, Vector2Int.down, Vector3.back, grid.transform);
            }
        }

        using (new EditorGUI.DisabledScope(obj.IsAdjacent(grid.GridPoint, Vector2Int.right)))
        {
            if (GUI.Button(new Rect(fieldRect.x + 2 * (buttonWidth + 5), fieldRect.y, buttonWidth, lineHeight), "Right"))
            {
                AddObject(obj.Grids, grid.GridPoint, Vector2Int.right, Vector3.right, grid.transform);
            }
        }

        using (new EditorGUI.DisabledScope(obj.IsAdjacent(grid.GridPoint, Vector2Int.left)))
        {
            if (GUI.Button(new Rect(fieldRect.x + 3 * (buttonWidth + 5), fieldRect.y, buttonWidth, lineHeight), "Left"))
            {
                AddObject(obj.Grids, grid.GridPoint, Vector2Int.left, Vector3.left, grid.transform);
            }
        }
        #endregion
    }

    private void CreateGrid()
    {
        GameObject newGridObject = new GameObject("Grid");
        newGridObject.transform.position = Vector3.zero;
        newGridObject.transform.localScale = obj.GridScale;

        GridObject newGrid = newGridObject.AddComponent<GridObject>();
        obj.Grids.Add(newGrid);
        newGridObject.transform.parent = obj.transform;

        newGrid.GridPoint = Vector2Int.zero;
        newGrid.SetGridManager = obj;
    }

    private void SpawnMaze()
    {
        GridDestroy(obj.Grids);

        foreach (var tile in obj.MazeObject.Tiles)
        {
            GameObject newGridObject = new GameObject();
            newGridObject.transform.localScale = obj.GridScale;
            newGridObject.transform.localPosition = new Vector3(
                tile.Key.x * obj.GridScale.x * 2.0f,
                0.0f,
                tile.Key.y * obj.GridScale.z * 2.0f);

            GridObject newGrid = newGridObject.AddComponent<GridObject>();
            obj.Grids.Add(newGrid);
            newGridObject.transform.parent = obj.transform;
            newGridObject.name = tile.Key.ToString();

            newGrid.GridPoint = tile.Key;
            newGrid.SetGridManager = obj;
        }
    }

    private void GridDestroy(List<GridObject> grids)
    {
        foreach (var grid in grids)
        {
            if (grid != null)
                DestroyImmediate(grid.gameObject);
        }
        grids.Clear();
    }

    private void AddObject(List<GridObject> grids, Vector2Int point, Vector2Int direction, Vector3 offset, Transform referenceTransform)
    {
        GameObject newGridObject = new GameObject();
        newGridObject.transform.localPosition = referenceTransform.localPosition + referenceTransform.rotation * (offset * 2.0f);
        newGridObject.transform.rotation = referenceTransform.rotation;
        newGridObject.transform.localScale = referenceTransform.localScale;

        GridObject newGrid = newGridObject.AddComponent<GridObject>();
        grids.Add(newGrid);
        newGridObject.transform.parent = obj.transform;
        newGrid.GridPoint = point + direction;
        newGridObject.name = newGrid.GridPoint.ToString();
        newGrid.SetGridManager = obj;
    }
}
