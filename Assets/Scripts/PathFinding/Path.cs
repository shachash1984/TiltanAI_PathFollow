using System.Collections;
using UnityEngine;

public class Path : MonoBehaviour
{
    public bool bDebug = true;
    public float Radius = 2.0f;
    public Vector3[] points;



    public float Length { get { return points.Length; } }

    public Vector3 GetPoint(int index)
    {
        return points[index];
    }

    void OnDrawGizmos()
    {
        if (!bDebug)
        {
            return;
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (i + 1 < points.Length)
            {
                Debug.DrawLine(points[i], points[i + 1], Color.red);
            }
        }
    }
}
