using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public SquareClass Class;
    public int x;
    public int y;
    public bool isStart;
    public bool isDestination;

    private void OnDrawGizmos()
    {
        switch (Class)
        {
            case SquareClass.Empty:
                Gizmos.color = Color.green;
                break;
            case SquareClass.Obstacle:
                Gizmos.color = Color.red;
                break;
            case SquareClass.Path:
                Gizmos.color = Color.yellow;
                break;
        }
        Gizmos.DrawSphere(transform.position, 0.15f);
    }
}

public enum SquareClass
{
    Empty,
    Obstacle,
    Path,
}
