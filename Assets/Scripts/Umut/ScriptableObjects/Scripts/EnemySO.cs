using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject", order = 2)]
public class EnemySO : ScriptableObject
{   
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    
    [Header("Base Values")]
    public int enemyID;
    public int health;
    public int goldReward;
    public float speed;
    public int size;
    public int damage;
    
}
