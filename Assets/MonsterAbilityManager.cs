using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class AllMonsterAbilityInfo
{
    public List<AbilityInfo> ability;
}
public class MonsterAbilityManager : Singleton<MonsterAbilityManager>
{
    [SerializeField]
    TextAsset jsonFile;
    public Dictionary<string, AbilityInfo> abilityDictionary;
    // Start is called before the first frame update
    void Start()
    {

        AllMonsterAbilityInfo allActionInfoList = JsonUtility.FromJson<AllMonsterAbilityInfo>(jsonFile.text);
        abilityDictionary = allActionInfoList.ability.ToDictionary(x => x.actionId, x => x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
