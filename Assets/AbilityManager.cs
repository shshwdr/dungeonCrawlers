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
    public int getNextDamage { get { return damage[level+1]; } }
    public int getEffectValue { get { return effectValue[level]; } }
    public int getNextEffectValue { get { return effectValue[level+1]; } }
    public bool isUnlocked { get { return AbilityManager.Instance.isAbilityUnlocked(actionId); } }
    public string getAbilityDetails
    {
        get
        {
            string text = "";
            if (descriptionType == "attack")
            {

                text = string.Format(description, BattleSystem.Instance.getPlayerDamage(this));
            }
            else if (descriptionType == "attackEffect")
            {

                text = string.Format(description, BattleSystem.Instance.getPlayerDamage(this), getEffectValue);
            }
            else if (descriptionType == "effect")
            {

                text = string.Format(description, getEffectValue);
            }
            return text;
        }
    }

    public string getNextLevelAbilityDetails
    {
        get
        {
            string text = "";
            if (descriptionType == "attack")
            {

                text = string.Format(description, BattleSystem.Instance.getPlayerDamage(this,true));
            }
            else if (descriptionType == "attackEffect")
            {

                text = string.Format(description, BattleSystem.Instance.getPlayerDamage(this, true), getNextEffectValue);
            }
            else if (descriptionType == "effect")
            {

                text = string.Format(description, getNextEffectValue);
            }
            return text;
        }
    }

}
public class AbilityManager : Singleton<AbilityManager>
{
    [SerializeField]
    TextAsset jsonFile;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Transform buttonsParent;
    public Dictionary<string, AbilityInfo> abilityDict = new Dictionary<string, AbilityInfo>();

    public int[] upgradeExp = new int[] { 100, 150 };

    public Dictionary<string, int> abilityLevel = new Dictionary<string, int>();
    public Dictionary<string, int> abilityExp = new Dictionary<string, int>();
    Dictionary<string, ActionButton> abilityButtons = new Dictionary<string, ActionButton>();

    // Start is called before the first frame update
    void Start()
    {
        AllActionInfo allActionInfoList = JsonUtility.FromJson<AllActionInfo>(jsonFile.text);
        abilityDict = allActionInfoList.abilityInfos.ToDictionary(x => x.actionId, x => x);
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
            abilityLevel[actionInfo.actionId] = -1;

            //test
            //if(actionInfo.actionId == "Focus" || actionInfo.actionId == "Tiny Tornado" || actionInfo.actionId == "Heat Breath")
            //{

            //    abilityLevel[actionInfo.actionId] = 0;
            //}

            abilityExp[actionInfo.actionId] = 0;
        }
    }

    public bool isAbilityUnlocked(string abilityId)
    {
        return abilityLevel[abilityId] >= 0;
    }
    public bool isAbilityAtMaxLevel(string abilityId)
    {
        return abilityLevel[abilityId] == upgradeExp.Length;
    }
    public string addExp(string abilityId, int exp)
    {
        var info = abilityDict[abilityId];
        if (!isAbilityUnlocked(abilityId))
        {
            abilityLevel[abilityId] = 1;
            abilityButtons[abilityId].gameObject.SetActive(true);
            return string.Format(Dialogs.unlockAbility, info.actionName);
        }
        else if(isAbilityAtMaxLevel(abilityId))
        {

            return string.Format(Dialogs.abilityMax, info.actionName);
        }
        else
        {
            abilityExp[abilityId] += exp;
            string res = string.Format(Dialogs.abilityExpAdd, exp, info.actionName);
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
