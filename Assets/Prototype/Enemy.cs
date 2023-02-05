using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float currentSpeed;
    
    [NonSerialized] public bool isSlowed;
    private GridCoord currentFrom, currentTo;
    private int stepCount = 1;
    [SerializeField] private EnemySO enemySo;
    public float currentHealth;

    public void Init()
    {
        currentHealth = enemySo.health;
        currentSpeed = enemySo.speed > 0 ? enemySo.speed : GameManager.Instance.BaseEnemySpeed;
        StartMoving();
    }
    
    public void StartMoving()
    {
        currentFrom = GameManager.Instance.Path[0];
        currentTo = GameManager.Instance.Path[1];
        MoveRecursive(currentFrom, currentTo);
    }

    public void MoveRecursive(GridCoord start, GridCoord finish)
    {
        transform.position = GridBuildingSystem.Instance.grid.GetWorldPosition(start.x, start.z);
        stepCount++;
        transform.DOMove(GridBuildingSystem.Instance.grid.GetWorldPosition(finish.x, finish.z), currentSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            if (GameManager.Instance.Path.Last() == finish)
            {
                //damage
                transform.DOKill();
                GameManager.Instance.SpawnedEnemies.Remove(this);
                Destroy(gameObject);
                return;
            }
            currentFrom = currentTo;
            currentTo = GameManager.Instance.Path[stepCount];
            MoveRecursive(currentFrom, currentTo);
        });
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0) onDeath();
    }

    public void SlowDownMovement()
    {
        if (isSlowed) return;
        isSlowed = true;
        StartCoroutine(slowTimer());
        currentSpeed /= 2f;
        transform.DOKill();
        transform.DOMove(GridBuildingSystem.Instance.grid.GetWorldPosition(currentTo.x, currentTo.z), currentSpeed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(
            () =>
            {
                if (GameManager.Instance.Path.Last() == currentTo)
                {
                    //damage
                    return;
                }
                currentFrom = currentTo;
                currentTo = GameManager.Instance.Path[stepCount];
                MoveRecursive(currentFrom, currentTo);
            });
    }

    private IEnumerator slowTimer()
    {
        yield return new WaitForSeconds(2f);
        currentSpeed = GameManager.Instance.BaseEnemySpeed;
        isSlowed = false;
    }

    private void onDeath()
    {
        transform.DOKill();
        ScoringManager.Instance.addGold(enemySo.goldReward);
        GameManager.Instance.SpawnedEnemies.Remove(this);
        Destroy(gameObject);
    }
}
