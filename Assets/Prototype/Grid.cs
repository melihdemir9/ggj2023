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

    public float baseEnemySpeed = 1f;

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

    //debug
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 50, 50), "Spawn"))
        {
            Spawn();
        }
    }


    public void Spawn()
    {
        var newEnemy = Instantiate(EnemyPrefab, spawnLocation.transform.position, 
            Quaternion.identity, spawnLocation.transform).GetComponent<Enemy>();
        newEnemy.grid = this;
        newEnemy.StartMoving();
    }

    public List<Square> GetPath()
    {
        var path = new List<Square>();
        path.Add(spawnLocation);
        bool notFoundDestination = true;
        var currentSquare = spawnLocation;

        while (notFoundDestination)
        {
            Square currentClosestSquare = squares
                .SelectMany(x => x.array)
                .FirstOrDefault(x => x.Class == CellType.Path 
                                     && !path.Contains(x)
                                     && Math.Abs(x.x - currentSquare.x) + Math.Abs(x.y - currentSquare.y) == 1);
            path.Add(currentClosestSquare);
            notFoundDestination = !currentClosestSquare.isDestination;
            currentSquare = currentClosestSquare;
        }

        return path;
    }
}

[Serializable]
public class GridArray
{
    public Square[] array;
}
