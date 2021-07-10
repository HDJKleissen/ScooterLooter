using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugHelper
{
    public static void DrawBox2D(Vector3 center, Vector2 size, Color color, float duration)
    {
        Vector3 topleft = center + new Vector3(-0.5f * size.x, 0.5f * size.y);
        Vector3 topright = center + new Vector3(0.5f * size.x, 0.5f * size.y);
        Vector3 bottomleft = center + new Vector3(-0.5f * size.x, -0.5f * size.y);
        Vector3 bottomright = center + new Vector3(0.5f * size.x, -0.5f * size.y);
        Debug.DrawLine(topleft, topright, color, duration);
        Debug.DrawLine(topright, bottomright, color, duration);
        Debug.DrawLine(bottomright, bottomleft, color, duration);
        Debug.DrawLine(bottomleft, topleft, color, duration);
    }
}
