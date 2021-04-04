using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatusAbility
{
    public string abilityName;
    public int freq;
}
[Serializable]
public class StatusReward
{
    public string itemName;
    public float itemRate;
    public int currencyMin;
    public int currencyMax;
}

[Serializable]
public class BattleCharacterStatus
{
    public string playerName;
    public int hp;
    public int atk;
    public int def;
    public int mag;
    public int magdef;
}
[Serializable]
public class BattlePlayerStatus : BattleCharacterStatus {
    public int mana;
}
[Serializable]
public class MonsterStatus : BattleCharacterStatus
{
    /*"absorbCost":5,
            "ability":[{"punch":1},{"strongPunch":1}],
            "reward":[{"potion":1}]*/
    public int absorbCost;
    public float absorbRate;
    public int absorbExp;

    public float runRate;
    public int level;
    public StatusReward reward;
    public StatusAbility[] ability;



}
[Serializable]
public class MonsterAbsorb
{
    public string playerName;
    public int color;
    public string absorbAbility;
}
[Serializable]
public class AllCharacterStatus
{

    public BattlePlayerStatus[] PlayerStatus;
    public MonsterStatus[] MonsterStatus;
    public MonsterAbsorb[] MonsterAbsorb;
}
public class BattleCharacters : Singleton<BattleCharacters>
{
    [SerializeField]
    TextAsset jsonFile;

    public BattlePlayerStatus playerStatus;
    public Dictionary<string, MonsterStatus> monsterStatusDict;
    public Dictionary<string, MonsterAbsorb> monsterAbsorbDict;

    // Start is called before the first frame update
    void Awake()
    {

        AllCharacterStatus allcharacters = JsonUtility.FromJson<AllCharacterStatus>(jsonFile.text);
        foreach (BattlePlayerStatus ps in allcharacters.PlayerStatus)
        {
            playerStatus = ps;

        }


        monsterStatusDict = allcharacters.MonsterStatus.ToDictionary(x => x.playerName + x.level, x => x);
        monsterAbsorbDict = allcharacters.MonsterAbsorb.ToDictionary(x => x.playerName + x.color, x => x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
