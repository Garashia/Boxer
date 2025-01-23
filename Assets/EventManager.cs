using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private GridManager m_gridManager;
    private Dictionary<Vector2Int, GridObject> m_gridObjects = new Dictionary<Vector2Int, GridObject>();
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


    [SerializeField]
    private Fade fade;

    [SerializeField]
    private ShoppingList m_shoppingGUI;

    [SerializeField]
    private Canvas m_canvas;

    private void CreateMap()
    {
        var gridData = m_gridManager.MazeObject;
        var itemList = gridData.ShoppingList;

        var grids = m_gridManager.Grids;
        int count = grids.Count;
        for (int i = 0; i < count; i++)
        {
            GridObject obj = grids[i];
            obj.Id = i;
            ConnectAdjacentGrids(obj, Vector2Int.right);
            ConnectAdjacentGrids(obj, Vector2Int.left);
            ConnectAdjacentGrids(obj, Vector2Int.up);
            ConnectAdjacentGrids(obj, Vector2Int.down);
            var point = obj.GridPoint;
            m_gridObjects.Add(point, obj);
            TileMapData mazeTile = m_gridManager.MazeObject;
            if (mazeTile.Tiles.ContainsKey(point))
            {
                var tileData = mazeTile.Tiles[point];

                switch (tileData)
                {
                    case TileType.Start:
                        m_gridManager.FirstGrid = obj;
                        break;
                    case TileType.Shop:

                        var list = itemList[point];

                        obj.EventCommands = () =>
                        {
                            return new List<MicroCommander.Command>()
                            {
                                new EnableCommand(m_activeObject, false),
                                new SpawnEventCommand(TextGUI),
                                new ShoppingEventCommand(TextGUI, m_shoppingGUI, m_canvas, list),
                                new DeleteEventCommand(TextGUI),
                                new EnableCommand(m_activeObject, true),

                            };

                        };
                        break;
                    case TileType.Goal:
                        obj.EncounterCommands = () =>
                        {
                            return new
                            List<MicroCommander.Command>()
                            {
                                new SceneChangeCommand("Result", fade),
                            };
                        };

                        break;

                    default:
                        break;
                }
            }
        }
        m_gridManager.Grids = grids;
    }

    private void ConnectAdjacentGrids(GridObject grid, Vector2Int direction)
    {
        GridObject adjacentGrid = m_gridManager.GetGridObject(grid.GridPoint, direction);
        if (adjacentGrid == null) return;

        if (direction == Vector2Int.right)
        {
            grid.A_Grid.Right ??= adjacentGrid;
            adjacentGrid.A_Grid.Left ??= grid;
        }
        else if (direction == Vector2Int.left)
        {
            grid.A_Grid.Left ??= adjacentGrid;
            adjacentGrid.A_Grid.Right ??= grid;
        }
        else if (direction == Vector2Int.up)
        {
            grid.A_Grid.Front ??= adjacentGrid;
            adjacentGrid.A_Grid.Back ??= grid;
        }
        else if (direction == Vector2Int.down)
        {
            grid.A_Grid.Back ??= adjacentGrid;
            adjacentGrid.A_Grid.Front ??= grid;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
