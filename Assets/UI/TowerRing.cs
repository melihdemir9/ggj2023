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

    public void InitBuy(Action buySuccess)
    {
        _buySuccess = buySuccess;
        TopRightButton.onClick.AddListener(BuildRedTower);
        BottomLeftButton.onClick.AddListener(BuildBlueTower);
    }
    
    private void BuildRedTower()
    {
        var newTower = Instantiate(redTowerPrefab, transform.position, Quaternion.identity).GetComponent<Tower>();
        newTower.Init();        
        ToggleOff();
        _buySuccess();
    }
    
    private void BuildBlueTower()
    {
        var newTower = Instantiate(blueTowerPrefab, transform.position, Quaternion.identity).GetComponent<Tower>();
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
