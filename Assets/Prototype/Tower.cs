using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Range(1, 5)] public float range;
    public GameObject projectilePrefab;
    public float chargeTime = 3f;
    public bool charging;
    public float projectileSpeed = 1f;
    
    void Update()
    {
        // var enemies = FindObjectsOfType<Enemy>();
        // var target = enemies
        //     .Where(x => Vector3.Distance(x.transform.position, transform.position) < range)
        //     .OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
        // if (target)
        // {
        //     ShootTarget(target);
        // }
        Debug.Log(Camera.main.WorldToScreenPoint(transform.position).x / Screen.width);
    }

    private void ShootTarget(Enemy target)
    {
        if (charging) return;
        charging = true;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, target.transform);
        projectile.transform.DOLocalMove(Vector3.zero, 1 / projectileSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            DOTween.Sequence().AppendInterval(chargeTime).OnComplete(() => charging = false);
            Destroy(projectile);
            
            //Type 1: Damage (destroy)
            //Destroy(target.gameObject);
            
            //Type 2: Slow
            target.SlowDownMovement();
        });
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, range);
    }
}
