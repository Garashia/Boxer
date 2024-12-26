using UnityEngine;

public enum TileType
{
    Wall = 0, // NoneÇ‚WallÇÕé´èëÇ…ìoò^ÇµÇ»Ç¢
    Area = 1,
    Shop,
    Start,
    Goal,
    Move,
    Warp,
    Boss,
    Chara,
    Item,
    None = 0
}


[System.Serializable]
public class TileMapData : ScriptableObject
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private SerializedDictionary<Vector2Int, TileType> tiles = new SerializedDictionary<Vector2Int, TileType>();
    [SerializeField]
    private Vector2Int? startedTile = null;

    [SerializeField]
    private SerializedDictionary<Vector2Int, int> warpTile = new SerializedDictionary<Vector2Int, int>();
    public int Width
    {
        get => width;
        set => width = value;
    }
    public int Height
    {
        get => height;
        set => height = value;
    }

    public SerializedDictionary<Vector2Int, TileType> Tiles
    {
        get => tiles;
        set => tiles = value;
    }

    public Vector2Int? StartedTile
    {
        get => startedTile;
        set => startedTile = value;
    }

    public SerializedDictionary<Vector2Int, int> WarpList
    {
        get => warpTile;
        set => warpTile = value;
    }

    public void Initialize(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles.Clear();
    }

    public TileType GetTile(Vector2Int position)
    {
        if (tiles.TryGetValue(position, out TileType tile))
        {
            return tile;
        }
        return TileType.None;
    }

    public void SetTile(Vector2Int position, TileType tileType)
    {
        if (tileType != TileType.Warp && warpTile.ContainsKey(position))
        {
            warpTile.Remove(position);
        }
        else if (tileType != TileType.Start && StartedTile == position)
        {
            StartedTile = null;
        }
        if (tileType == TileType.None || tileType == TileType.Wall)
        {
            tiles.Remove(position);
        }
        else
        {
            tiles[position] = tileType;
        }
    }

    public void SetWarp(Vector2Int position, TileType tileType)
    {
        warpTile.TryAdd(position, 0);
        Debug.Log(position.ToString());
        SetTile(position, tileType);
    }

}
