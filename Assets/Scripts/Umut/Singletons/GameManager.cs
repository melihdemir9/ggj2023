using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GridCoord[] Path;
    
    public float BaseEnemySpeed = 1f;

    public GameObject[] EnemyPrefabs;

    public List<Enemy> SpawnedEnemies;

    public void Spawn(int EnemyType)
    {
        var newEnemy = Instantiate(EnemyPrefabs[EnemyType],
            GridBuildingSystem.Instance.grid.GetWorldPosition(Path[0].x, Path[0].z), 
            Quaternion.identity)
            .GetComponent<Enemy>();
        SpawnedEnemies.Add(newEnemy);
        newEnemy.Init();
    }
    
    private void OnGUI()
    {
        for (int i = 0; i < EnemyPrefabs.Length; i++)
        {
            if (GUI.Button(new Rect(10, 10 + (i*60), 50, 50), "Spawn " + i))
            {
                Spawn(i);
            }
        }

    }
}

[Serializable]
public class GridCoord
{
    public int x;
    public int z;

    public GridCoord(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
