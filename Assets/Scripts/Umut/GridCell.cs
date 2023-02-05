using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{   
    private GridXZ<GridCell> grid;
    public int x;
    public int z;
    public PlacedUnit placedUnit;
    
    public Transform transform;

    public CellType cellType;


    public GridCell(GridXZ<GridCell> grid, int x, int z)
    {   
            
        this.grid = grid;
        this.x = x; 
        this.z = z;
    }
    
    
    public void SetPlacedUnit(PlacedUnit placedUnit)
    {
        this.placedUnit = placedUnit;
        
        grid.TriggerGridObjectChanged(x,z);
    }
        
    public void ClearPlacedUnit()
    {
        this.placedUnit = null;
        
        grid.TriggerGridObjectChanged(x,z);
    }

    public bool isEmpty()
    {
        return placedUnit == null;
    }
        
    public PlacedUnit GetPlacedUnit()
    {
        return placedUnit;
    }

    public int GetIDPlacedUnit()
    {
        return placedUnit.placedUnitSO.towerID;
    }
        
    public void MovePlacedObject(Vector3 position)
    {
        placedUnit.transform.position = position;
        grid.TriggerGridObjectChanged(x,z);
    }
    
    public void ChangeTransform(GridXZ<GridCell> grid,  int oldX, int oldZ, int newX, int newZ, PlacedUnit placedUnit)
    {
        GridCell oldCell = grid.GetGridObject(oldX, oldZ);
        oldCell.ClearPlacedUnit();
        GridCell newCell = grid.GetGridObject(newX, newZ);
        newCell.SetPlacedUnit(placedUnit);
        grid.TriggerGridObjectChanged(oldX, oldZ);
        grid.TriggerGridObjectChanged(newX, newZ);
    }
    
    public void ChangeTransform(GridXZ<GridCell> grid,  GridCell oldGridCell, GridCell newGridCell, PlacedUnit placedUnit)
    {
        oldGridCell.ClearPlacedUnit();
        newGridCell.SetPlacedUnit(placedUnit);
        grid.TriggerGridObjectChanged(oldGridCell.x, oldGridCell.z);
        grid.TriggerGridObjectChanged(newGridCell.x, newGridCell.z);
        
    }

    public bool canMerge(GridCell oldGridCell, GridCell newGridCell)
    {
        return oldGridCell.GetPlacedUnit() == newGridCell.GetPlacedUnit();
    }
    
        
    public override string ToString()
    {
        return x + "," + z + "\n"+ transform + "\n" + placedUnit;
    }
    
    
    
        
}