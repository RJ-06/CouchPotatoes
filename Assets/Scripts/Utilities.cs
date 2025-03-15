using UnityEngine;

public static class Utilities
{
    // Returns the distance between two points in the game
    public static float FindDistance(Vector2 a, Vector2 b) {
        float xDist = Mathf.Abs(a.x - b.x);
        float yDist = Mathf.Abs(a.y - b.y);
        return Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));
    }
}
