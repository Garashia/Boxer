using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonFactory
{
    private readonly Vector2Int[] DIRECT_POINT = new Vector2Int[4]
    {
        new(0, 1),
        new(0, -1),
        new(1, 0),
        new(-1, 0),
    };

    private enum MapType2
    {
        AREA,
        ROAD,
        WALL
    };

    private int[,] m_maze;

    // private int[,] m_dungeon;

    private MapType2[,] m_mapType2;
    private int Width { get; set; }
    private int Height { get; set; }

    private int Room { get; set; }

    public DungeonFactory(int width, int height, int room)
    {
        int mazeWidth = width / 3;
        int mazeHeight = height / 3;

        if (mazeWidth % 2 == 0)
        {
            mazeWidth--;
        }
        if (mazeHeight % 2 == 0)
        {
            mazeHeight--;
        }

        // 5ñ¢ñûÇÃÉTÉCÉYÇ‚ãÙêîÇ≈ÇÕê∂ê¨Ç≈Ç´Ç»Ç¢
        if (mazeWidth < 5 || mazeHeight < 5) throw new ArgumentOutOfRangeException();
        // ñ¿òHèÓïÒÇèâä˙âª
        this.Width = mazeWidth;
        this.Height = mazeHeight;
        MazeFactory mazeFactory = new MazeFactory(mazeWidth, mazeHeight);
        m_maze = mazeFactory.CreateMaze();
        m_mapType2 = new MapType2[mazeWidth, mazeHeight];
        mazeFactory = null;
        this.Room = room;
    }

    private struct DungeonMap
    {
        public int s_x, s_y;

        public Vector2Int Point
        {
            get { return new(s_x, s_y); }
        }

        public bool s_up, s_down, s_right, s_left;

        //new (0, 1),
        //new (0, -1),
        //new (1, 0),
        //new (-1, 0),
        public void SetBool(int index)
        {
            switch (index)
            {
                case 0:
                    s_up = true;
                    break;

                case 1:
                    s_down = true;
                    break;

                case 2:
                    s_right = true;
                    break;

                case 3:
                    s_left = true;
                    break;

                default:
                    break;
            }
        }

        public DungeonMap(int x, int y, bool up = false, bool down = false, bool right = false, bool left = false)
        {
            s_x = x;
            s_y = y;
            s_up = up;
            s_down = down;
            s_right = right;
            s_left = left;
        }
    }

    public int[,] CreateDungeon()
    {
        List<DungeonMap> values = new List<DungeonMap>();
        SerializedDictionary<Vector2Int, bool> num = new SerializedDictionary<Vector2Int, bool>();
        for (int x = 0; x < this.Width; ++x)
        {
            for (int y = 0; y < this.Height; ++y)
            {
                if (m_maze[x, y] == 0)
                {
                    m_mapType2[x, y] = MapType2.WALL;
                    continue;
                }
                m_mapType2[x, y] = MapType2.ROAD;
                values.Add(new(x, y));
                num.Add(new(x, y), true);
            }
        }
        values = values.OrderBy(a => Guid.NewGuid()).ToList();
        var d_list = values.Select((item, i) =>
        {
            if (i < this.Room)
            {
                m_mapType2[item.s_x, item.s_y] = MapType2.AREA;
            }
            for (int j = 0; j < 4; ++j)
            {
                if (num.ContainsKey(DIRECT_POINT[j] + item.Point) == true)
                {
                    item.SetBool(j);
                }
            }

            return item;
        });

        int DungeonWidth = this.Width * 3;
        int DungeonHeight = this.Height * 3;

        int[,] m_dungeon = new int[DungeonWidth + 5, DungeonHeight + 5];

        foreach (var d in d_list)
        {
            (int dungeon_x, int dungeon_y) = (d.s_x * 3, d.s_y * 3);
            if (m_mapType2[d.s_x, d.s_y] == MapType2.WALL)
                continue;
            else if (m_mapType2[d.s_x, d.s_y] == MapType2.AREA)
            {
                m_dungeon[dungeon_x, dungeon_y] = 1;
                m_dungeon[dungeon_x + 1, dungeon_y] = 1;
                m_dungeon[dungeon_x + 2, dungeon_y] = 1;
                m_dungeon[dungeon_x, dungeon_y + 1] = 1;
                m_dungeon[dungeon_x + 1, dungeon_y + 1] = 1;
                m_dungeon[dungeon_x + 2, dungeon_y + 1] = 1;
                m_dungeon[dungeon_x, dungeon_y + 2] = 1;
                m_dungeon[dungeon_x + 1, dungeon_y + 2] = 1;
                m_dungeon[dungeon_x + 2, dungeon_y + 2] = 1;
            }
            else if (m_mapType2[d.s_x, d.s_y] == MapType2.ROAD)
            {
                m_dungeon[dungeon_x + 1, dungeon_y + 1] = 1;
                if (d.s_up)
                {
                    m_dungeon[dungeon_x + 1, dungeon_y + 2] = 1;
                }
                if (d.s_down)
                {
                    m_dungeon[dungeon_x + 1, dungeon_y] = 1;
                }
                if (d.s_left)
                {
                    m_dungeon[dungeon_x, dungeon_y + 1] = 1;
                }
                if (d.s_right)
                {
                    m_dungeon[dungeon_x + 2, dungeon_y + 1] = 1;
                }
            }
        }

        return m_dungeon;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}

//static class D
//{
//    public static void DebugList<T>(this T[,] ints)
//    {
//        var len = ints;
//        for (int i = 0; i < len.GetLength(0); i++)
//        {
//            string str = "";
//            for (int j = 0; j < len.GetLength(1); j++)
//            {
//                str = str + len[i, j].ToString() + ", ";
//            }
//            Debug.Log(str);
//        }
//    }

//}