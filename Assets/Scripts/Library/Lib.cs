using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lib
{
    public static float GetAngleBetweenTranfsorm(Vector2 from, Vector2 to)
    {
        Vector2 difference = to - from;
        float sign = (to.y < from.y) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector3.right , difference) * sign;
        return angle - 90f;
    }

    public static Vector2 Vector3ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector2 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector2(x, y);

    }
}