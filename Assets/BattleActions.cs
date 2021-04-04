using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class ActionInfo
{
    public string actionId;
    public string actionName;
    public string description;
    public string doActionDesc;
    public string descriptionType;
    public int[] cost;
    public int level
    {
        get
        {
            var lev = 0;
            if (this is AbilityInfo)
            {
                if(AbilityManager.Instance.abilityLevel.ContainsKey(actionId))
                {

                    return AbilityManager.Instance.abilityLevel[actionId];
                }
            }
            return lev;
        }
    }
    public int getCost { get {return cost[level]; } }
}

[Serializable]
public class AllActionInfo
{
    public List<ActionInfo> topBattleInfos;
    public List<AbilityInfo> abilityInfos;
}
public class BattleActions : Singleton<BattleActions>
{
    [SerializeField]
    TextAsset jsonFile;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Transform buttonsParent;
    Dictionary<string, ActionInfo> actionDictionary = new Dictionary<string, ActionInfo>();
    public Dictionary<string, ActionButton> actionButtonDictionary = new Dictionary<string, ActionButton>();
    // Start is called before the first frame update
    void Start()
    {
        AllActionInfo allActionInfoList = JsonUtility.FromJson<AllActionInfo>(jsonFile.text);
        //actionDictionary = actionInfoList.ToDictionary(x => x.actionId, x => x);
        foreach (var actionInfo in allActionInfoList.abilityInfos)
        {
            if(actionInfo.actionId == "Attack")
            {
                GameObject button = Instantiate(buttonPrefab, buttonsParent);
                ActionButton actionButton = button.GetComponent<ActionButton>();
                actionButton.Init(actionInfo);
                break;
            }
        }

        foreach (var actionInfo in allActionInfoList.topBattleInfos)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsParent);
            ActionButton actionButton = button.GetComponent<ActionButton>();
            actionButton.Init(actionInfo);
            actionButtonDictionary[actionInfo.actionId] = actionButton;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
