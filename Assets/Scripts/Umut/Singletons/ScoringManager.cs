using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringManager : MonoSingleton<ScoringManager>
{
    private int currentGoldAmount = 0;
    
    
    public int getCurrentGoldAmount()
    {
        return currentGoldAmount;
    }
    
    public int addGold(int amount)
    {
        currentGoldAmount += amount;
        return currentGoldAmount;
    }
    
    public int removeGold(int amount)
    {
        currentGoldAmount -= amount;
        return currentGoldAmount;
    }
    
    public bool canAfford(int amount)
    {
        return currentGoldAmount >= amount;
    }
    
    
    
}
 