using System;
using System.Collections.Generic;
public class DungeonFactory
{

    enum MapType2
    {
        AREA,
        ROAD,
        WALL
    };

    private int[,] m_maze;

    private int[,] m_dungeon;

    private MapType2[,] m_mapType2;
    private int Width { get; set; }
    private int Height { get; set; }

    private int Room { get; set; }



    public DungeonFactory(int width, int height, int room)
    {
        int mazeWidth = width / 3;
        int mazeHeight = height / 3;


        // 5–¢–‚ÌƒTƒCƒY‚â‹ô”‚Å‚Í¶¬‚Å‚«‚È‚¢
        if (mazeWidth < 5 || mazeHeight < 5) throw new ArgumentOutOfRangeException();
        if (mazeWidth % 2 == 0)
        {
            mazeWidth++;
        }
        if (mazeHeight % 2 == 0)
        {
            mazeHeight++;
        }
        // –À˜Hî•ñ‚ğ‰Šú‰»
        this.Width = mazeWidth;
        this.Height = mazeHeight;
        MazeFactory mazeFactory = new MazeFactory(mazeWidth, mazeHeight);
        m_maze = mazeFactory.CreateMaze();
        m_mapType2 = new MapType2[mazeWidth, mazeHeight];
        mazeFactory = null;
        this.Room = room;
    }

    public int[,] CreateDungeon()
    {
        List<(int x, int y)> values = new List<(int x, int y)>();
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
                values.Add((x, y));
            }
        }



        return m_dungeon;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
