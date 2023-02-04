using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedUnit : MonoBehaviour
{   
    public static PlacedUnit Create(Vector3 worldPosition, Vector2Int origin, TowerSO placedUnitSo)
    {
        GameObject placedUnitTransform = Instantiate(placedUnitSo.projectilePrefab, worldPosition, Quaternion.identity);
        PlacedUnit placedUnit = placedUnitTransform.GetComponent<PlacedUnit>();
        placedUnit.origin = origin;
        placedUnit.placedUnitSO = placedUnitSo;
        
        return placedUnit;
    }
    public TowerSO placedUnitSO;
    private Vector2Int origin;
    
    public int GetUnitID()
    {
        return placedUnitSO.towerID;
    }
}