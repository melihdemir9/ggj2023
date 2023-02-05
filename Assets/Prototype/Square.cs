using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public CellType Class;
    public int x;
    public int y;
    public bool isStart;
    public bool isDestination;

    private void OnDrawGizmos()
    {
        switch (Class)
        {
            case CellType.Empty:
                Gizmos.color = Color.green;
                break;
            case CellType.Obstacle:
                Gizmos.color = Color.red;
                break;
            case CellType.Path:
                Gizmos.color = Color.yellow;
                break;
        }
        Gizmos.DrawSphere(transform.position, 0.15f);
    }
}

public enum CellType
{
    Empty,
    Obstacle,
    Path,
    Tower
}
