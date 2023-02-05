using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using CodeMonkey.Utils;
public class GridBuildingSystem  : MonoSingleton<GridBuildingSystem>
{
    public GridXZ<GridCell> grid;
    [SerializeField] private Transform originPosition;
    public float cellSizeX = 10f;
    public float cellSizeZ = 10f;
    // [SerializeField] private GameObject gridGroundPrefab;
    // [SerializeField] private GameObject gridGroundPrefabEnemyBorderless;
    private void Awake()
    {  
        int gridWidth = 6;
        int gridHeight = 17;
        grid = new GridXZ<GridCell>(gridWidth, gridHeight, cellSizeX,cellSizeZ, originPosition.position, (GridXZ<GridCell> grid, int x, int z) => new GridCell(grid, x, z));
        
        

        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                if(GameManager.Instance.Path.Count(a => a.x == i && a.z == j) > 0)
                    grid.GetGridObject(i, j).cellType = CellType.Path;
            }
        }
       
    }
    
}