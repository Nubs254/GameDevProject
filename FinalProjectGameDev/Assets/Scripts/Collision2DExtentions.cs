using UnityEngine;

public static class Collision2DExtensions
{
    public static bool WasWithPlayer(this Collision2D collision)
    {
        return collision.gameObject.CompareTag("Player");
    }
}