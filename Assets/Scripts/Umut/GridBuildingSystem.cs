using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using CodeMonkey.Utils;
public class GridBuildingSystem  : MonoSingleton<GridBuildingSystem>
{
    public GridXZ<GridCell> grid;
    [SerializeField] private Transform originPosition;
    [SerializeField] private GameObject gridGroundPrefab;
    [SerializeField] private GameObject gridGroundPrefabEnemyBorderless;

    [SerializeField] private TowerSO[] _unitSO;
    
    private void Awake()
    {  
        int gridWidth = 8;
        int gridHeight = 8;
        float cellSizeX = 10f;
        float cellSizeZ = 10f;
        grid = new GridXZ<GridCell>(gridWidth, gridHeight, cellSizeX,cellSizeZ, originPosition.position, (GridXZ<GridCell> grid, int x, int z) => new GridCell(grid, x, z));
        
        GameObject gridContainer = GameObject.Find("GridContainer");
        if (gridContainer == null)
            gridContainer = new GameObject("GridContainer");

        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight()/2; j++)
            {   GameObject spawnedTile = Instantiate(gridGroundPrefab, grid.GetWorldPositionCenterOfGrid(i, j) + new Vector3(0,-0.91f,0), Quaternion.identity);
                spawnedTile.tag = "GridCellFake";
                spawnedTile.name = $"GridCell [{i}, {j}]";
                spawnedTile.transform.SetParent(gridContainer.transform);
            }

            for (int j = grid.GetHeight()/2; j < gridHeight; j++)
            {
                GameObject spawnedTile = Instantiate(gridGroundPrefabEnemyBorderless, grid.GetWorldPositionCenterOfGrid(i, j) + new Vector3(0,-0.91f,0), Quaternion.identity);
                //spawnedTile.tag = "Grid";
                spawnedTile.name = $"GridCellEnemy [{i}, {j}]";
                spawnedTile.transform.SetParent(gridContainer.transform);
            }
        }
       
    }
    
}