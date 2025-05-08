using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utilities
{
    // Returns the distance between two points in the game
    public static float FindDistance(Vector2 a, Vector2 b) {
        float xDist = Mathf.Abs(a.x - b.x);
        float yDist = Mathf.Abs(a.y - b.y);
        return Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));
    }

    public static Vector2 FindNearestTileInSet(Vector2 position, Tilemap tilemap) {
        Vector2 nearestTile = Vector2.zero;
        float shortestDist = float.MaxValue;

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Debug.Log("Possible tile: " + pos);
            if (tilemap.HasTile(pos) && tilemap.GetTile(pos) != null)
            {
                Debug.Log("This tile exists!");
                // Get the world position of the tile
                Vector2 tileWorldPos = tilemap.GetCellCenterWorld(pos);
                float distance = FindDistance(position, tileWorldPos);

                if (distance < shortestDist)
                {
                    Debug.Log("This tile is closer!");
                    shortestDist = distance;
                    nearestTile = tileWorldPos;
                }
            }
        }

        return nearestTile;
    }
}
