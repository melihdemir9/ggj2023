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
    
    public void StartMoving()
    {
        if (!grid) return;
        path = grid.GetPath();
        MoveRecursive(grid.spawnLocation, path[path.IndexOf(grid.spawnLocation) + 1]);
    }

    public void MoveRecursive(Square start, Square finish)
    {
        transform.position = start.transform.position;
        transform.DOMove(finish.transform.position, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.SetParent(finish.transform);
            if (finish.isDestination) return;
            var next = path[path.IndexOf(finish) + 1];
            MoveRecursive(finish, next);
        });
    }
}
