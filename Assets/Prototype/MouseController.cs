using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [NonSerialized] public Vector3 startPoint;
    [NonSerialized] public Vector3 endPoint;
    public LineRenderer line;
    public float swipeInterval = 1f;

    public bool isSwiping = false;

    private void OnMouseDown()
    {
        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPoint.z = 0;
        isSwiping = true;
        StartCoroutine(SwipeTimer());
    }

    public IEnumerator SwipeTimer()
    {
        yield return new WaitForSeconds(swipeInterval);
        isSwiping = false;
    }

    private void OnMouseDrag()
    {
        if (isSwiping)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 0;
        }
        else
        {
            ResetLinePoints();
        }
        DrawLine();
    }

    private void OnMouseUp()
    {
        CeaseSwipe();
        ResetLinePoints();
        DrawLine();
    }

    public void CeaseSwipe()
    {
        isSwiping = false;
    }

    public void DrawLine()
    {
        line.SetPositions(new []{startPoint, endPoint});
        Mesh mesh = new Mesh();
        line.BakeMesh(mesh, Camera.main);
        //SetPolygonCollider3D.UpdatePolygonCollider2D(mesh, line.GetComponent<PolygonCollider2D>());
    }

    public void ResetLinePoints()
    {
        startPoint = Vector3.zero;
        endPoint = Vector3.zero;
    }
}
