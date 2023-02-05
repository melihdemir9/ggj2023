using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range;
    public GameObject projectilePrefab;
    public float chargeTime = 3f;
    public bool charging;
    public float projectileSpeed = 10f;
    public TowerSO towerSo;
    public EffectType effectType;
    public float damage;

    public void Init()
    {
        chargeTime = 1 / towerSo.attackSpeed;
        projectilePrefab = towerSo.projectilePrefab;
        projectileSpeed = towerSo.projectileSpeed;
        range = towerSo.range * (float)Math.Pow(Screen.height, -0.3f);
        damage = towerSo.damage;
        effectType = towerSo.effectType;
    }
    
    void Update()
    {
        var availableTargets = GameManager.Instance.SpawnedEnemies.Where(
            x => GetScreenDistance(x.transform.position, transform.position) < range);
        if (availableTargets.Any())
        {
            ShootTarget(availableTargets.FirstOrDefault());
        }
    }

    private float GetScreenDistance(Vector3 worldPos1, Vector3 worldPos2)
    {
        var pos1 = Camera.main.WorldToScreenPoint(worldPos1);
        var pos2 = Camera.main.WorldToScreenPoint(worldPos2);
        var distVector = (pos1 - pos2);
        distVector.Scale(new Vector3((float)Math.Pow(Screen.width, -1), (float)Math.Pow(Screen.height, -1), 0));
        return distVector.magnitude;
    }

    private void ShootTarget(Enemy target)
    {
        if (charging) return;
        charging = true;
        DOTween.Sequence().AppendInterval(chargeTime).OnComplete(() => charging = false);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, target.transform);
        projectile.transform.DOLocalMove(Vector3.zero, projectileSpeed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(projectile);
            switch (effectType)
            {
                case EffectType.Slow:
                    target.SlowDownMovement();
                    target.TakeDamage(damage);
                    break;
                default:
                    target.TakeDamage(damage);
                    break;
            }
        });
    }
}
