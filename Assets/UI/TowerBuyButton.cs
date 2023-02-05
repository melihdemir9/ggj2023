using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerBuyButton : MonoBehaviour
{
    //test
    public GameObject towerPrefab;
    public TowerRing towerRing;

    private void OnMouseDown()
    {
        Vector3 ground = new Vector3(transform.position.x, 0, transform.position.z);
        towerRing.InitBuy(ground, () => Destroy(gameObject));
        towerRing.ToggleOn();
    }

    private void BuildTower()
    {
        //todo use this to spawn different towers
        var newTower = Instantiate(towerPrefab, transform.position, Quaternion.identity).GetComponent<Tower>();
        newTower.Init();
        Destroy(gameObject);
    }
}
