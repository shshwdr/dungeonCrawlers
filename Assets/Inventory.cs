using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class ItemInfo:ActionInfo
{
    public int param;
}
[Serializable]

public class Inventory : Singleton<Inventory>
{
    Dictionary<string, int> currentItemDict = new Dictionary<string, int>();
    Dictionary<string, ActionButton> itemButtonDict = new Dictionary<string, ActionButton>();
    public Dictionary<string, ItemInfo> itemInfoDict;
    public Dictionary<string, int> statusLevel = new Dictionary<string, int>();
    public Dictionary<string, PlayerPurchaseStatus> playerStatus = new Dictionary<string, PlayerPurchaseStatus>();

    [SerializeField]
    TextAsset jsonFile;

    [SerializeField]
    GameObject itemButtonPrefab;

    [SerializeField]
    Transform buttonsParent;

    public int currentCurrency = 0;
    // Start is called before the first frame update
    void Awake()
    {
        AllActionInfo allItemInfo = JsonUtility.FromJson<AllActionInfo>(jsonFile.text);
        itemInfoDict = allItemInfo.itemInfos.ToDictionary(x => x.actionId, x => x);

        foreach (var itemInfo in allItemInfo.itemInfos)
        {
            //don't show currency in action
            if(itemInfo.descriptionType == "currency")
            {
                continue;
            }
            //itemInfoDict[itemInfo.actionName] = itemInfo;
            currentItemDict[itemInfo.actionId] = 0;
            GameObject button = Instantiate(itemButtonPrefab, buttonsParent);
            ActionButton actionButton = button.GetComponent<ActionButton>();
            actionButton.Init(itemInfo);
            itemButtonDict[itemInfo.actionId] = actionButton;
            updateItemButton(itemInfo.actionId);

        }
        foreach (var actionInfo in allItemInfo.topBattleInfos)
        {
            if (actionInfo.actionId == "back")
            {
                GameObject button = Instantiate(itemButtonPrefab, buttonsParent);
                ActionButton actionButton = button.GetComponent<ActionButton>();
                actionButton.Init(actionInfo);
                itemButtonDict[actionInfo.actionId] = actionButton;
            }
        }

    }
    void updateItemButton(string itemInfo)
    {
        if (currentItemDict[itemInfo] > 0)
        {
            itemButtonDict[itemInfo].gameObject.SetActive(true);
        }
        else
        {
            itemButtonDict[itemInfo].gameObject.SetActive(false);
        }
    }
    public void useItem(string item, int value = 1)
    {
        currentItemDict[item] -= value;
        updateItemButton(item);
    }

    public void addItem(string item, int value = 1)
    {
        if (currentItemDict.ContainsKey(item))
        {
            currentItemDict[item]+=value;
        }
        else
        {
            currentItemDict[item] = value;
        }
        updateItemButton(item);
    }

    public void addCurrency(int value)
    {
        currentCurrency += value;
        //ShopMenu.Instance. updateCoin();
    }

    public void lossCurrency(float value)
    {
        currentCurrency =( int)(currentCurrency*(1-value));
        //ShopMenu.Instance.updateCoin();
    }
    public bool canPurchase(PurchaseItem item)
    {

        var cost = item.getCost;
        return currentCurrency >= cost;
    }
    public void purchase(PurchaseItem item)
    {
        var cost = item.getCost;

        currentCurrency -= cost;
        if(item is PlayerPurchaseStatus)
        {
            updateStatusLevel((PlayerPurchaseStatus)item);
        }else if(item is PurchaseInventory)
        {
            addItem(item.itemId);
        }else if(item is PurchaseHeal)
        {
            if (item.itemId == "hp")
            {
                BattleSystem.Instance.player.heal(5);
            }
            else
            {
                BattleSystem.Instance.player.restoreMana(5);
            }
        }
        //ShopMenu.Instance.updateCoin();
    }
    public bool isStatusAtMaxLevel(PlayerPurchaseStatus status)
    {
        return statusLevel[status.itemId] == status.cost.Length-1;
    }

    public void updateStatusLevel(PlayerPurchaseStatus status)
    {
        if (BattleSystem.Instance.player)
        {
            if (status.itemId == "hp")
        {

                //hacky
                BattleSystem.Instance.player.heal(10);
            }else if(status.itemId == "mana")
            {
                BattleSystem.Instance.player.restoreMana(5);
            }
        }
        statusLevel[status.itemId]++;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
