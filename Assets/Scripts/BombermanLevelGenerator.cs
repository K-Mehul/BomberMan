using UnityEngine;
using UnityEngine.Tilemaps;

public class BombermanLevelGenerator : MonoBehaviour
{
    public int width = 15;
    public int height = 13;

    public Tilemap indestructibleTilemap;  // Tilemap for indestructible tiles
    public Tilemap destructibleTilemap;    // Tilemap for destructible tiles

    public TileBase groundTile;
    public TileBase wallTile;
    public TileBase destructibleTile;

    [Range(0, 1)]
    public float destructibleSpawnChance = 0.7f;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        indestructibleTilemap.ClearAllTiles();
        destructibleTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                // Borders (indestructible walls)
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    indestructibleTilemap.SetTile(position, wallTile);  // Set wall on indestructible Tilemap
                }
                // Indestructible pillars every 2 tiles
                else if (x % 2 == 0 && y % 2 == 0)
                {
                    indestructibleTilemap.SetTile(position, wallTile);  // Set wall on indestructible Tilemap
                }
                else
                {
                    // Empty ground (on both Tilemaps, but ground is not visible)
                    indestructibleTilemap.SetTile(position, groundTile); // Set ground on indestructible Tilemap
                    destructibleTilemap.SetTile(position, groundTile);  // Set ground on destructible Tilemap

                    // Random chance to place destructible walls on destructible Tilemap
                    if (Random.value < destructibleSpawnChance && !IsPlayerSpawnArea(x, y))
                    {
                        destructibleTilemap.SetTile(position, destructibleTile);  // Set destructible wall
                    }
                }
            }
        }
    }

    private bool IsPlayerSpawnArea(int x, int y)
    {
        return
            (x <= 2 && y <= 2) || // Bottom-left corner
            (x >= width - 3 && y <= 2) || // Bottom-right corner
            (x <= 2 && y >= height - 3) || // Top-left corner
            (x >= width - 3 && y >= height - 3); // Top-right corner
    }

    // Call this method when the tile should be destroyed
    public void DestroyTile(Vector3Int position)
    {
        // Check if the tile is destructible
        if (destructibleTilemap.GetTile(position) == destructibleTile)
        {
            destructibleTilemap.SetTile(position, groundTile);  // Replace destructible with ground
        }
    }
}
