using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class AbilityInfo : ActionInfo
{
    public int[] mana;
    public int[] damage;
    public string abilityType;
    public int effectRound;
    public int[] effectValue;
    public string effectType;
    public bool wouldExtend;

    public bool applyOnSelf;
    public int getMana { get { return mana[level]; } }
    public int getDamage { get { return damage[level]; } }
    public int getEffectValue { get { return effectValue[level]; } }

}
public class AbilityManager : Singleton<AbilityManager>
{
    [SerializeField]
    TextAsset jsonFile;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Transform buttonsParent;
    Dictionary<string, AbilityInfo> actionDictionary = new Dictionary<string, AbilityInfo>();

    int[] upgradeExp = new int[] { 0,100, 150 };

    Dictionary<string, int> abilityLevel = new Dictionary<string, int>();
    Dictionary<string, int> abilityExp = new Dictionary<string, int>();
    Dictionary<string, ActionButton> abilityButtons = new Dictionary<string, ActionButton>();

    // Start is called before the first frame update
    void Start()
    {
        AllActionInfo allActionInfoList = JsonUtility.FromJson<AllActionInfo>(jsonFile.text);
        actionDictionary = allActionInfoList.abilityInfos.ToDictionary(x => x.actionId, x => x);
        //this might change for each battle
        foreach (var actionInfo in allActionInfoList.abilityInfos)
        {
            if (actionInfo.actionId == "Attack")
            {
                continue;
            }
            GameObject button = Instantiate(buttonPrefab, buttonsParent);
            ActionButton actionButton = button.GetComponent<ActionButton>();
            actionButton.Init(actionInfo);
            abilityButtons[actionInfo.actionId] = actionButton;
            button.SetActive(false);
            abilityLevel[actionInfo.actionId] = 0;
            abilityExp[actionInfo.actionId] = 0;
        }
    }

    public bool isAbilityUnlocked(string abilityId)
    {
        return abilityLevel[abilityId] > 0;
    }

    public string addExp(string abilityId, int exp)
    {
        var info = actionDictionary[abilityId];
        if (abilityLevel[abilityId] == 0)
        {
            abilityLevel[abilityId] = 1;
            abilityButtons[abilityId].gameObject.SetActive(true);
            return string.Format(Dialogs.unlockAbility, info.actionName);
        }
        else if(abilityLevel[abilityId] == upgradeExp.Length)
        {

            return string.Format(Dialogs.abilityMax, info.actionName);
        }
        else
        {
            abilityExp[abilityId] += exp;
            string res = string.Format(Dialogs.abilityExpAdd, info.actionName, exp);
            if (upgradeExp[abilityLevel[abilityId]] <= abilityExp[abilityId])
            {
                abilityExp[abilityId] -= upgradeExp[abilityLevel[abilityId]];
                abilityLevel[abilityId] += 1;
                res += string.Format(Dialogs.abilityLevelUp, info.actionName);
            }
            return res;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
