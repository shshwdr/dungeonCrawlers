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
public class AllItemInfo
{
    public List<ItemInfo> itemInfos;
}
public class Inventory : Singleton<Inventory>
{
    Dictionary<string, int> currentItemDict = new Dictionary<string, int>();
    Dictionary<string, ActionButton> itemButtonDict = new Dictionary<string, ActionButton>();
    Dictionary<string, ItemInfo> itemInfoDict;
    [SerializeField]
    TextAsset jsonFile;

    [SerializeField]
    GameObject itemButtonPrefab;

    [SerializeField]
    Transform buttonsParent;

    int currentCurrency = 0;
    // Start is called before the first frame update
    void Awake()
    {
        AllItemInfo allItemInfo = JsonUtility.FromJson<AllItemInfo>(jsonFile.text);
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

    public void addItem(string item, int value)
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
