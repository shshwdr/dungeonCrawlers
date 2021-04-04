using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class ZoneMonsterInfo
{
    public string monsterId;
    public int rate;
    public int level;
    public int color;
}
    [Serializable]
public class ZoneInfo
{
    public string zoneId;
    public string zoneName;
    public List<ZoneMonsterInfo> zoneMonsters;

}
[Serializable]
public class AllZoneInfo
{
    public List<ZoneInfo> zoneInfos;
}



public class ZoneManager : Singleton<ZoneManager>
{
    [SerializeField]
    int meetMonterCounter = 5;
    int currentStepCounter = 0;
    Dictionary<string, ZoneInfo> zoneDict = new Dictionary<string, ZoneInfo>();
    [SerializeField]
    TextAsset jsonFile;
    void Awake()
    {
        AllZoneInfo allItemInfo = JsonUtility.FromJson<AllZoneInfo>(jsonFile.text);
        zoneDict = allItemInfo.zoneInfos.ToDictionary(x => x.zoneId, x => x);
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    public bool moveInDangerZone(bool canTriggerBattle)
    {
        currentStepCounter++;
        if (currentStepCounter >= meetMonterCounter)
        {
            if (canTriggerBattle && !CheatManager.Instance.dontEncounter)
            {

                //start popup battle
                currentStepCounter = 0;
                return true;
            }
            else
            {
                Debug.Log("can't trigger battle");
            }
        }
        else
        {

        }
        return false;
    }

    public void StartPopupBattle(string zoneID, BattlePlayer player, Vector3 monsterPosition, Quaternion monsterRotation)
    {
        Debug.Log("start battle " + zoneID);
        var zoneInfo = zoneDict[zoneID];
        List<ZoneMonsterInfo> zoneMonsterInfos = new List<ZoneMonsterInfo>();
        List<int> monsterRates = new List<int>();
        foreach (var zoneMonsterInfo in zoneInfo.zoneMonsters)
        {
            string monsterId = zoneMonsterInfo.monsterId;
            monsterRates.Add(zoneMonsterInfo.rate);
            zoneMonsterInfos.Add(zoneMonsterInfo);
            //var monsterInfo = BattleCharacters.Instance.monsterStatusDict[monsterId];
            //Debug.Log("battle monster " + monsterId);
            //BattleSystem.Instance.StartBattle(monsterId, player, monsterPosition, monsterRotation);
        }
        var randomId  = Utils.getRandomIdInDistribution(monsterRates);
        var selectedZoneMonster = zoneMonsterInfos[randomId];

        BattleSystem.Instance.StartBattle(selectedZoneMonster, player, monsterPosition, monsterRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
