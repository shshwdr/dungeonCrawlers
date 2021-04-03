using Doozy.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerPurchaseStatus : PurchaseItem
{
    /*"statusId": "hp",
        "statusName":"HP",
        "levelValue":[25,	35,	45,	55,	65,	75,	85,	95,	105,	115,	125],
        "cost":[0,	30,	35,	45,	50,	55,	60,	65,	70,	85,	90]*/
    
    public int[] levelValue;
    public int[] cost;


}
[Serializable]
public class PurchaseItem
{
    public string itemId;
    public string itemName;
}


[Serializable]
public class AllShopItems
{

    public PlayerPurchaseStatus[] PlayerStatus;
}
public class ShopManager : Singleton<ShopManager>
{
    [SerializeField]
    TextAsset jsonFile;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Transform buttonsParent;


    public bool isInShop;
    // Start is called before the first frame update
    void Start()
    {
        AllShopItems allitems = JsonUtility.FromJson<AllShopItems>(jsonFile.text);

        foreach (var actionInfo in allitems.PlayerStatus)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsParent);
            ShopItem actionButton = button.GetComponent<ShopItem>();
            actionButton.Init(actionInfo);
        }
    }

    public void showShopMenu()
    {
        isInShop = true;
        GameEventMessage.SendEvent("Shop");
    }

    public void hideShopMenu()
    {
        isInShop = false;
        GameEventMessage.SendEvent("ExitShop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
