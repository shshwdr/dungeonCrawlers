using Doozy.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    public override int getCost { get { return cost[Inventory.Instance.statusLevel[itemId] + 1] *Utils.currencyScale; } }
    public int getLevelValue { get { return levelValue[Inventory.Instance.statusLevel[itemId]]; } }
    public int getNextLevelValue { get { return levelValue[Inventory.Instance.statusLevel[itemId] + 1]; } }
}

[Serializable]
public class PurchaseInventory : PurchaseItem
{
    public int cost;
    public override int getCost
    {
        get { return cost * Utils.currencyScale; }
    }

    public override string getItemName
    {
        get { return Inventory.Instance.itemInfoDict[itemId].actionName; }
    }
}
[Serializable]
public class PurchaseHeal : PurchaseItem
{
    public float cost;
    public override int getCost
    {
        get { return (int)(cost * Utils.currencyScale); }
    }
}
[Serializable]
public class PurchaseItem
{
    public string itemId;
    public string itemName;
    public virtual string getItemName
    {
        get { return itemName; }
    }
    public virtual int getCost
    {
        get { return 0; }
    }
}


    [Serializable]
    public class AllShopItems
    {

    public PlayerPurchaseStatus[] PlayerStatus;
    public PurchaseInventory[] Items;
    public PurchaseHeal[] ImmediateEffect;
}
public class ShopManager : Singleton<ShopManager>
{
    [SerializeField]
    TextAsset jsonFile;
    [SerializeField]
    Transform buttonsParent;
    public TMP_Text detailLabel;
    public TMP_Text coinLabel;

    public bool firstOpen = true;
    public bool isInShop;
    AllShopItems allitems;
    List<ShopItem> itemButtons = new List<ShopItem>();
    // Start is called before the first frame update


    public void updateCoin()
    {
        if (coinLabel)
        {
            coinLabel.text = "Coin: " + Inventory.Instance.currentCurrency;
        }
    }
    void Start()
    {
        foreach (Transform buffTransform in buttonsParent)
        {
            itemButtons.Add(buffTransform.GetComponent<ShopItem>());
        }


        allitems = JsonUtility.FromJson<AllShopItems>(jsonFile.text);
        Inventory.Instance.playerStatus = allitems.PlayerStatus.ToDictionary(x => x.itemId, x => x);
        foreach (var actionInfo in allitems.PlayerStatus)
        {
            Inventory.Instance.statusLevel[actionInfo.itemId] = 0;
        }
        updateShop();
    }

    public void updateShop()
    {
        int i = 0;
        foreach (var actionInfo in allitems.PlayerStatus)
        {
            if (!Inventory.Instance.isStatusAtMaxLevel(actionInfo))
            {
                itemButtons[i].gameObject.SetActive(true);
                itemButtons[i].Init(actionInfo);
                i++;
            }
        }
        foreach (var actionInfo in allitems.Items)
        {
            itemButtons[i].gameObject.SetActive(true);
            itemButtons[i].Init(actionInfo);
            i++;
        }
        foreach (var actionInfo in allitems.ImmediateEffect)
        {
            itemButtons[i].gameObject.SetActive(true);
            itemButtons[i].Init(actionInfo);
            i++;
        }
        for (; i < itemButtons.Count; i++)
        {
            itemButtons[i].gameObject.SetActive(false);
        }
    }

    public void showShopMenu()
    {
        if (firstOpen)
        {
            firstOpen = false;
        }
        else
        {
            ShopManager.Instance.updateCoin();
        }
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
