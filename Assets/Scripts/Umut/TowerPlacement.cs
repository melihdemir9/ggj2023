using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public TowerSO[] towerTypes;
    
    public Camera camera;
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   Debug.Log("Mouse Clicked");
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {   Debug.Log("Raycast Hit");
                Vector3 mouseWorldPosition = hit.point;
                
                GridBuildingSystem.Instance.grid.GetXZ(mouseWorldPosition, out int x, out int z);
                Debug.Log(x + " " + z );
                // Do something with the mouse world position, such as creating a tower
                CreateTower(0, mouseWorldPosition);
            }
        }
    }

    public void CreateTower(int towerID, Vector3 position)
    {
        // Make sure the tower index is within bounds
        if (towerID < 0 || towerID >= towerTypes.Length)
        {
            Debug.LogError("Invalid tower index");
            return;
        }

        // Get the selected tower scriptable object
        TowerSO towerType = towerTypes[towerID];

        // Create the tower at the desired position
        PlacedUnit tower = PlacedUnit.Create(GridBuildingSystem.Instance.grid.GetWorldPositionCenterOfGrid((int) position.x,(int)position.z) ,new Vector2Int((int)position.x, (int)position.z),
            towerType);
            //Instantiate(towerType.towerPrefab, GridBuildingSystem.Instance.grid.GetWorldPositionCenterOfGrid((int)position.x, (int)position.z), Quaternion.identity);
        //GridBuildingSystem.Instance.grid.GetGridObject((int)position.x, (int)position.z).cellType = CellType.Tower;
        tower.transform.position = GridBuildingSystem.Instance.grid.GetWorldPositionCenterOfGrid((int)position.x, (int)position.z);
        
    }

    public bool IsBuildLocationValid(Vector3 position)
    {
        // Do your validation checks here, for example checking if the position is in a valid build area
        // and if there is no other tower already built at that position
        // ...

        return true;
    }
}
