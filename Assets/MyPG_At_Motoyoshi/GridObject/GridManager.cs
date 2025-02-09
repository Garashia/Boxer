using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField, HideInInspector]
    public bool isOpen { get; set; }

    [SerializeField]
    private Vector3 m_gridScale; // gridScale

    public Vector3 GridScale
    {
        get => m_gridScale;
        set => m_gridScale = value;
    }

    [SerializeField]
    private List<GridObject> m_grids; // grids

    public List<GridObject> Grids
    {
        get => m_grids;
        set => m_grids = value;
    }

    // private Dictionary<Vector2Int, GridObject> m_gridObjects = new Dictionary<Vector2Int, GridObject>();

    [SerializeField, HideInInspector]
    private GridObject m_firstGrid = null; // firstGrid

    public GridObject FirstGrid
    {
        get
        {
            m_firstGrid ??= Grids[0];
            return m_firstGrid;
        }
        set => m_firstGrid = value;
    }

    [SerializeField]
    private TileMapData m_mazeTableObject; // mazeTableObject

    public TileMapData MazeObject
    {
        get => m_mazeTableObject;
        set => m_mazeTableObject = value;
    }

    [SerializeField]
    private GameObject m_floor; // floor

    public GameObject Floor
    {
        get => m_floor;
        set => m_floor = value;
    }

    [SerializeField]
    private GameObject m_wall; // wall

    public GameObject Wall
    {
        get => m_wall;
        set => m_wall = value;
    }

    [SerializeField]
    private GameObject m_corner; // corner

    public GameObject Corner
    {
        get => m_corner;
        set => m_corner = value;
    }

    [SerializeField]
    private GameObject m_textObject; // corner

    public GameObject TextObject
    {
        get => m_textObject;
        set => m_textObject = value;
    }

    private readonly Vector2Int[] m_orientations = new[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };

    //private Vector2Int[,] m_cornerOrientations = new Vector2Int[4, 3]
    //{
    //    {
    //        new Vector2Int(0, 1),
    //        new Vector2Int(1, 1),
    //        new Vector2Int(1, 0)
    //    },
    //    {
    //        new Vector2Int(0, 1),
    //        new Vector2Int(-1, 1),
    //        new Vector2Int(-1, 0)
    //    },
    //    {
    //        new Vector2Int(0, -1),
    //        new Vector2Int(1, -1),
    //        new Vector2Int(1, 0)
    //    },
    //    {
    //        new Vector2Int(0, -1),
    //        new Vector2Int(-1, -1),
    //        new Vector2Int(-1, 0)
    //    }
    //};

    private Vector2Int[,] m_cornerOrientations = new Vector2Int[4, 3]
{
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
        },
        {
            new Vector2Int(0, 1),
                        new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
        },
        {
            new Vector2Int(0, -1),
                        new Vector2Int(1, 0)
,
            new Vector2Int(1, -1),
        },
        {
            new Vector2Int(0, -1),            new Vector2Int(-1, 0)
,
            new Vector2Int(-1, -1),
        }
};


    private static readonly uint Corner_Up_Right = (1 << 0);
    private static readonly uint Corner_Up_Left = (1 << 1);
    private static readonly uint Corner_Down_Right = (1 << 2);
    private static readonly uint Corner_Down_Left = (1 << 3);

    private static readonly uint Wall_Up = (1 << 0);
    private static readonly uint Wall_Down = (1 << 1);
    private static readonly uint Wall_Right = (1 << 2);
    private static readonly uint Wall_Left = (1 << 3);

    private readonly uint[] m_wallOrientations = new[]
    {
        Wall_Up,
        Wall_Down,
        Wall_Right,
        Wall_Left
    };

    private readonly uint[] m_cornerOrientationFlags = new[]
    {
        Corner_Up_Right,
        Corner_Up_Left,
        Corner_Down_Right,
        Corner_Down_Left
    };

    private struct Distraction
    {
        public uint Corner { get; set; }
        public uint Wall { get; set; }
    }

    private const float m_baseScale = 2.25f; // by
    private readonly float m_wallHeight = 0.275f * m_baseScale; // high
    private readonly float m_wallOffset = 0.44f * m_baseScale; // wallInvert
    private readonly float m_cornerOffset = 0.44f * m_baseScale; // cornerInvert
    //private readonly float m_cornerOffset = 0.425f * m_baseScale; // cornerInvert

    [SerializeField, HideInInspector]
    private List<GameObject> m_spawnObjects; // spawnObjects

    [SerializeField]
    private Text m_textGUI;
    public Text TextGUI
    {
        get { return m_textGUI; }
        set { m_textGUI = value; }
    }

    [SerializeField]
    private GameObject m_activeObject;
    public GameObject ActiveObject
    {
        get { return m_activeObject; }
        set { m_activeObject = value; }
    }


    public bool IsAdjacent(int x, int y, Vector2Int direction)
    {
        return IsAdjacent(new Vector2Int(x, y), direction);
    }

    public bool IsAdjacent(Vector2Int point, Vector2Int direction)
    {
        Vector2Int targetPoint = point + direction;
        foreach (GridObject obj in m_grids)
        {
            if (obj.GridPoint == targetPoint)
            {
                return true;
            }
        }
        return false;
    }

    public GridObject GetGridObject(Vector2Int point, Vector2Int direction)
    {
        Vector2Int targetPoint = point + direction;
        foreach (GridObject obj in m_grids)
        {
            if (obj.GridPoint == targetPoint)
            {
                return obj;
            }
        }
        return null;
    }

    private void Start()
    {




    }
    public void DestroyObject(GameObject obj)
    {
        for (int i = 0; i < m_grids.Count; i++)
        {
            if (m_grids[i].gameObject.GetInstanceID() == obj.GetInstanceID())
            {
                DestroyImmediate(m_grids[i].gameObject);
                m_grids.RemoveAt(i);
                break;
            }
        }
    }

    public void PushObjectSpawn()
    {
        m_spawnObjects ??= new List<GameObject>();
        foreach (GameObject spawn in m_spawnObjects)
        {
            if (spawn != null) DestroyImmediate(spawn);
        }
        m_spawnObjects.Clear();
        var tiles = m_mazeTableObject.Tiles;
        foreach (GridObject obj in m_grids)
        {
            Transform trans = obj.transform;
            GameObject floorInstance = Instantiate(m_floor, trans);
            floorInstance.transform.localPosition = Vector3.zero;
            floorInstance.transform.localScale *= m_baseScale;
            m_spawnObjects.Add(floorInstance);
            Vector2Int point = obj.GridPoint;
            Distraction distraction = DistractionArea(point);
            if (m_wall != null)
                SpawnWall(distraction.Wall, m_spawnObjects, Vector3.zero, trans);
            if (m_corner != null)
                SpawnCorner(distraction.Corner, m_spawnObjects, Vector3.zero, trans);
            if (!tiles.ContainsKey(point)) continue;
            if (tiles[point] == TileType.Wall || tiles[point] == TileType.None || tiles[point] == TileType.Area) continue;
            SpawnText(tiles[point], m_spawnObjects, Vector3.up * 0.5f, trans);

        }
    }

    private Distraction DistractionArea(Vector2Int point)
    {
        Distraction distraction = new();
        for (int i = 0; i < m_orientations.Length; i++)
        {
            if (!IsAdjacent(point, m_orientations[i]))
            {
                distraction.Wall |= m_wallOrientations[i];
            }
        }

        for (int i = 0; i < m_cornerOrientations.GetLength(0); i++)
        {
            if (IsAdjacent(point, m_cornerOrientations[i, 0]) &&
                IsAdjacent(point, m_cornerOrientations[i, 1]) &&
                !IsAdjacent(point, m_cornerOrientations[i, 2]))
            {
                distraction.Corner |= m_cornerOrientationFlags[i];
            }
        }

        return distraction;
    }

    private void SpawnWall(uint walls, List<GameObject> gameObjects, Vector3 position, Transform parent)
    {
        if ((walls & Wall_Up) != 0)
        {
            SpawnObject(m_wall, new Vector3(0.0f, m_wallHeight, m_wallOffset), parent, gameObjects);
        }
        if ((walls & Wall_Down) != 0)
        {
            SpawnObject(m_wall, new Vector3(0.0f, m_wallHeight, -m_wallOffset), parent, gameObjects);
        }
        if ((walls & Wall_Right) != 0)
        {
            GameObject obj = SpawnObject(m_wall, new Vector3(m_wallOffset, m_wallHeight, 0.0f), parent, gameObjects);
            obj.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        if ((walls & Wall_Left) != 0)
        {
            GameObject obj = SpawnObject(m_wall, new Vector3(-m_wallOffset, m_wallHeight, 0.0f), parent, gameObjects);
            obj.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void SpawnCorner(uint corners, List<GameObject> gameObjects, Vector3 position, Transform parent)
    {
        if ((corners & Corner_Up_Right) != 0)
        {
            SpawnObject(m_corner, new Vector3(m_cornerOffset, m_wallHeight, m_cornerOffset), parent, gameObjects);
        }
        if ((corners & Corner_Up_Left) != 0)
        {
            SpawnObject(m_corner, new Vector3(-m_cornerOffset, m_wallHeight, m_cornerOffset), parent, gameObjects);
        }
        if ((corners & Corner_Down_Right) != 0)
        {
            SpawnObject(m_corner, new Vector3(m_cornerOffset, m_wallHeight, -m_cornerOffset), parent, gameObjects);
        }
        if ((corners & Corner_Down_Left) != 0)
        {
            SpawnObject(m_corner, new Vector3(-m_cornerOffset, m_wallHeight, -m_cornerOffset), parent, gameObjects);
        }
    }

    private void SpawnText(TileType type, List<GameObject> gameObjects, Vector3 position, Transform parent)
    {
        var spawn = SpawnObject(m_textObject, position, parent, gameObjects, false);
        if (spawn.TryGetComponent<TMP_Text>(out TMP_Text text))
        {
            text.text = type.ToString();
        }
    }


    private GameObject SpawnObject(GameObject prefab, Vector3 position, Transform parent, List<GameObject> gameObjects, bool isScaled = true)
    {
        GameObject instance = Instantiate(prefab, parent);
        instance.transform.localPosition = position;
        instance.transform.localScale = isScaled ? instance.transform.localScale * m_baseScale : instance.transform.localScale;
        gameObjects.Add(instance);
        return instance;
    }


}