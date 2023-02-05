using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerRing : MonoBehaviour
{
    public Button TopRightButton;
    public Button BottomLeftButton;

    public GameObject redTowerPrefab;
    public GameObject blueTowerPrefab;

    private Action _buySuccess;
    private Vector3 _spawnPos;

    public void InitBuy(Vector3 spawnPos, Action buySuccess)
    {
        _spawnPos = spawnPos;
        _buySuccess = buySuccess;
        TopRightButton.onClick.AddListener(BuildRedTower);
        BottomLeftButton.onClick.AddListener(BuildBlueTower);
    }
    
    private void BuildRedTower()
    {
        var newTower = Instantiate(redTowerPrefab, _spawnPos, Quaternion.identity).GetComponent<Tower>();
        newTower.Init();        
        ToggleOff();
        _buySuccess();
    }
    
    private void BuildBlueTower()
    {
        var newTower = Instantiate(blueTowerPrefab, _spawnPos, Quaternion.identity).GetComponent<Tower>();
        newTower.Init();
        ToggleOff();
        _buySuccess();
    }

    public void ToggleOn()
    {
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        gameObject.SetActive(false);
    }
}
