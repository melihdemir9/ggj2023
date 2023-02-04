using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/TowerScriptableObject", order = 1)]
public class TowerSO : ScriptableObject
{   public int towerID;
    public float range;
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
    public float chargeTime = 3f;
    public bool charging;
}
