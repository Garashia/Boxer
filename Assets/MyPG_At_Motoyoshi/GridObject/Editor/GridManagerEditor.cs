using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;






#if UNITY_EDITOR
using UnityEditor;      //!< デプロイ時にEditorスクリプトが入るとエラーになるので UNITY_EDITOR で括ってね！
#endif // UNITY_EDITOR



#if UNITY_EDITOR
[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private GridManager obj;
    private bool isString = false;
    private ReorderableList _reorderableList;

    private void OnEnable()
    {
        // 有効になった時に対象を確保しておく
        obj = target as GridManager;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // 画面構成と処理
        // 標準的な表示をしたい場合は、baseを呼び出す
        // base.OnInspectorGUI();
        bool flag = false;
        bool flag2 = false;
        EditorGUI.BeginChangeCheck();
        {
            isString = EditorGUILayout.Toggle("DebugString", isString);
        }
        if (EditorGUI.EndChangeCheck())
        {
            flag2 = true;
        }
        EditorGUI.BeginChangeCheck();
        {
            EditorGUILayout.BeginHorizontal();
            {
                obj.GridScale = EditorGUILayout.Vector2Field("Scale:", obj.GridScale);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (EditorGUI.EndChangeCheck())
        {
            flag = true;
        }
        List<GridObject> grids = obj.Grids;
        int count = grids.Count;

        if (GUILayout.Button("Delete Grid"))
        {
            GridDestroy(grids, count);
        }
        if (_reorderableList == null)
        {
            _reorderableList = new ReorderableList(grids, typeof(GridObject));
            // 並び替え可能か
            _reorderableList.draggable = true;

            // タイトル描画時のコールバック
            // 上書きしてEditorGUIを使えばタイトル部分を自由にレイアウトできる
            _reorderableList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "推移条件");

            // 要素の描画時のコールバック
            // 上書きしてEditorGUIを使えば自由にレイアウトできる
            _reorderableList.drawElementCallback += DrawElement;
            void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                EditorGUI.indentLevel += 1;
                var height = EditorGUIUtility.singleLineHeight + 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y += 5;
                GridObject grid = grids[index];
                if (grid == null) return;
                Transform transform = grid.transform;
                Vector2Int point = grid.GridPoint;

                if (flag2)
                    grid.GridRender = isString;

                grid.Open = EditorGUILayout.Foldout(grid.Open, "grid" + (index + 1).ToString());
                if (grid.Open)
                {
                    rect.y += height;
                    EditorGUILayout.BeginHorizontal();
                    {
                        transform.localPosition = EditorGUILayout.Vector3Field("position:", transform.localPosition);
                    }
                    EditorGUILayout.EndHorizontal();
                    rect.y += height;

                    EditorGUILayout.BeginHorizontal();
                    {
                        transform.localEulerAngles = EditorGUILayout.Vector3Field("rotation:", transform.localEulerAngles);
                    }
                    EditorGUILayout.EndHorizontal();
                    rect.y += height;

                    EditorGUILayout.BeginHorizontal();
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.right)))
                    {
                        if (GUILayout.Button("Right"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.right,
                                Vector3.right,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.left)))
                    {
                        if (GUILayout.Button("Left"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.left,
                                Vector3.left,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.up)))
                    {
                        if (GUILayout.Button("Front"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.up,
                                Vector3.forward,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.down)))
                    {
                        if (GUILayout.Button("Back"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.down,
                                Vector3.back,
                                transform
                                );
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    rect.y += height;

                    if (GUILayout.Button("Destroy"))
                    {
                        DestroyImmediate(grid.gameObject);
                        grids.RemoveAt(index); //リストの削除
                        return;
                    }
                    rect.y += height;

                    if (GUILayout.Button("GridDown"))
                    {
                        grid.GridDown();
                    }


                }

                EditorGUI.indentLevel -= 1;
            }
        }
        count = grids.Count;
        if (count != 0)
        {

            for (int i = 0; i < count; ++i)
            {
                GridObject grid = grids[i];
                if (grid == null) continue;
                Transform transform = grid.transform;
                Vector2Int point = grid.GridPoint;
                if (flag2)
                    grid.GridRender = isString;
                grid.SetGridManager = obj;

                if (grid.Open = EditorGUILayout.Foldout(grid.Open, "grid" + (i + 1).ToString()))
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        transform.localPosition = EditorGUILayout.Vector3Field("position:", transform.localPosition);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        transform.localEulerAngles = EditorGUILayout.Vector3Field("rotation:", transform.localEulerAngles);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.right)))
                    {
                        if (GUILayout.Button("Right"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.right,
                                Vector3.right,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.left)))
                    {
                        if (GUILayout.Button("Left"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.left,
                                Vector3.left,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.up)))
                    {
                        if (GUILayout.Button("Front"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.up,
                                Vector3.forward,
                                transform
                                );
                        }
                    }
                    using (new EditorGUI.DisabledScope(obj.IsAdjacent(point, Vector2Int.down)))
                    {
                        if (GUILayout.Button("Back"))
                        {
                            AddObject
                                (
                                grids,
                                point,
                                Vector2Int.down,
                                Vector3.back,
                                transform
                                );
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("Destroy"))
                    {
                        DestroyImmediate(grid.gameObject);
                        grids.RemoveAt(i); //リストの削除
                        break;
                    }
                    if (GUILayout.Button("GridDown"))
                    {
                        grid.GridDown();
                    }
                }

                if (flag)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = obj.GridScale.x;
                    scale.z = obj.GridScale.y;
                    transform.localScale = scale;
                }

            }
            if (GUILayout.Button("GridAllDown"))
            {
                for (int i = 0; i < count; ++i)
                {
                    GridObject grid = grids[i];
                    grid.GridDown();
                }
            }

        }
        else
        {
            if (GUILayout.Button("Create"))
            {
                GameObject games = new GameObject();
                games.transform.position = Vector3.zero;
                Vector3 scale = games.transform.localScale;
                scale.x = obj.GridScale.x;
                scale.z = obj.GridScale.y;
                games.transform.localScale = scale;
                GridObject newGrid = games.AddComponent<GridObject>();
                grids.Add(newGrid);
                games.transform.parent = obj.transform;
                games.name = newGrid.GridPoint.ToString();

                newGrid.GridPoint = Vector2Int.zero;
                newGrid.SetGridManager = obj;
            }
        }
        obj.MazeObject = EditorGUILayout.ObjectField("Maze", obj.MazeObject, typeof(TileMapData), true) as TileMapData;
        if (obj.MazeObject != null)
        {
            if (GUILayout.Button("SpawnMaze"))
            {
                GridDestroy(grids);
                var mazeList = obj.MazeObject.Tiles;
                if (mazeList != null)
                    foreach (var maze in mazeList)
                    {
                        // if (maze.Value == TileType.None || maze.Value == TileType.Wall) continue;
                        GameObject games = new GameObject();

                        Vector3 scale = games.transform.localScale;
                        scale.x = obj.GridScale.x;
                        scale.z = obj.GridScale.y;
                        games.transform.localScale = scale;
                        games.transform.localPosition =
                            new((float)(maze.Key.x) * games.transform.localScale.x * 2.0f,
                            0.0f,
                           (float)(maze.Key.y) * games.transform.localScale.z * 2.0f);
                        GridObject newGrid = games.AddComponent<GridObject>();
                        grids.Add(newGrid);
                        games.transform.parent = obj.transform;
                        newGrid.GridPoint = maze.Key;
                        games.name = newGrid.GridPoint.ToString();

                        newGrid.SetGridManager = obj;
                    }
            }
        }
        if (count != 0)
        {

            obj.Floor = EditorGUILayout.ObjectField("Floor", obj.Floor, typeof(GameObject), true) as GameObject;
            obj.Wall = EditorGUILayout.ObjectField("Wall", obj.Wall, typeof(GameObject), true) as GameObject;
            obj.Corner = EditorGUILayout.ObjectField("Corner", obj.Corner, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("SpawnObject"))
            {
                obj.PushObjectSpawn();
            }
        }
        // Dirtyフラグを立てる
        EditorUtility.SetDirty(obj);
        serializedObject.ApplyModifiedProperties();
    }

    private void GridDestroy(List<GridObject> grids)
    {
        int count = grids.Count;

        if (count != 0)
        {
            foreach (GridObject grid in grids)
            {
                if (grid != null)
                    DestroyImmediate(grid.gameObject);
            }
        }
        // 押下時に実行したい処理
        grids.Clear();

    }

    private void GridDestroy(List<GridObject> grids, int count)
    {
        if (count != 0)
        {
            foreach (GridObject grid in grids)
            {
                if (grid != null)
                    DestroyImmediate(grid.gameObject);
            }
        }
        // 押下時に実行したい処理
        grids.Clear();

    }


    private void AddObject
    (
        List<GridObject> list,
        Vector2Int point,
        Vector2Int direct2,
        Vector3 direct3,
        Transform transform)
    {
        GameObject games = new GameObject();
        Vector3 position = transform.localPosition;
        Vector3 moving = new Vector3(
            direct3.x * transform.localScale.x,
            0.0f,
            direct3.z * transform.localScale.z
            );
        position += transform.rotation * (moving * 2.0f);

        games.transform.localPosition = position;
        games.transform.rotation = transform.rotation;
        GridObject newGrid = games.AddComponent<GridObject>();
        list.Add(newGrid);
        games.transform.localScale = transform.localScale;
        games.transform.parent = obj.transform;
        newGrid.GridPoint = point + direct2;
        games.name = newGrid.GridPoint.ToString();

        newGrid.SetGridManager = obj;
    }

}
#endif // UNITY_EDITOR