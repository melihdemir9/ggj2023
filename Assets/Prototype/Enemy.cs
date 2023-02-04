using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Grid grid;
    private List<Square> path;
    private float currentSpeed;
    [NonSerialized] public bool isSlowed;
    private Square currentFrom, currentTo;
    
    public void StartMoving()
    {
        currentSpeed = grid.baseEnemySpeed;
        if (!grid) return;
        path = grid.GetPath();
        currentFrom = grid.spawnLocation;
        currentTo = path[path.IndexOf(grid.spawnLocation) + 1];
        MoveRecursive(currentFrom, currentTo);
    }

    public void MoveRecursive(Square start, Square finish)
    {
        transform.position = start.transform.position;
        transform.DOMove(finish.transform.position, currentSpeed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.SetParent(finish.transform);
            if (finish.isDestination) return;
            currentFrom = currentTo;
            currentTo = path[path.IndexOf(finish) + 1];
            MoveRecursive(currentFrom, currentTo);
        });
    }

    public void SlowDownMovement()
    {
        if (isSlowed) return;
        isSlowed = true;
        StartCoroutine(slowTimer());
        currentSpeed /= 2f;
        transform.DOKill();
        transform.DOMove(currentTo.transform.position, currentSpeed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(
            () =>
            {
                transform.SetParent(currentTo.transform);
                if (currentTo.isDestination) return;
                currentFrom = currentTo;
                currentTo = path[path.IndexOf(currentTo) + 1];
                MoveRecursive(currentFrom, currentTo);
            });
    }

    private IEnumerator slowTimer()
    {
        yield return new WaitForSeconds(2f);
        currentSpeed = grid.baseEnemySpeed;
        isSlowed = false;
    }
}
