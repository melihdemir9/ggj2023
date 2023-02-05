using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/TowerScriptableObject", order = 1)]
public class TowerSO : ScriptableObject
{   public int towerID;
    
    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
    
    [Header("Base Values")]
    public float range;
    public float damage;
    public float attackSpeed;
    public float projectileSpeed;
    public EffectType effectType;
    
    [Header("Costs")]
    public int buyCost;
    public int upgradeCost;

    [Header("Upgraded Values")]
    public float rangeUpgradedValue;
    public float damageUpgradedValue;
    public float chargingUpgradedValue;
    
}
public enum EffectType
{
    None,
    RapidPiercing,
    Slow,
    Chain,
    Aoe
}

