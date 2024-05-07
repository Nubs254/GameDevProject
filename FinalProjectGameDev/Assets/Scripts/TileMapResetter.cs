using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapResetter : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        ResetTilemap();
    }

    void ResetTilemap()
    {
        // Clear all tiles on the tilemap
        tilemap.ClearAllTiles();

        // You can also set specific tiles if needed
        // For example:
        // tilemap.SetTile(new Vector3Int(0, 0, 0), null);
        // This sets the tile at position (0, 0, 0) to null (empty)
    }
}
