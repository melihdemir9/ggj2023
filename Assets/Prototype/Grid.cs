using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GridArray[] squares;
    public GameObject EnemyPrefab;

    public Square spawnLocation;

    public void Start()
    {
        spawnLocation = squares.SelectMany(x => x.array).FirstOrDefault(x => x.isStart);
        for (int i = 0; i < squares.Length; ++i)
        {
            for (int j = 0; j < squares[i].array.Length; ++j)
            {
                squares[i].array[j].x = i;
                squares[i].array[j].y = j;
            }
        }
    }
}

[Serializable]
public class GridArray
{
    public Square[] array;
}
