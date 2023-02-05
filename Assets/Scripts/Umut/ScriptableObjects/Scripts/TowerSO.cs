using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/TowerScriptableObject", order = 1)]
public class TowerSO : ScriptableObject
{   public int towerID;
    
    [Header("Prefabs")]
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
    
    [Header("Charging")]
    public float chargeTime;
    public bool charging;
    
    [Header("Base Values")]
    public float range;
    public float damage;
    public float attackSpeed;
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
    RapidPiercing,
    Slow,
    Chain,
    Aoe
}

