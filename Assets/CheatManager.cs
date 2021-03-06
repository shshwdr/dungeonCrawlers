using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : Singleton<CheatManager>
{
    public bool defeiniteAbsorb;
    public bool dontEncounter;

    public void toggleDefiniteAbsorb(bool isOn)
    {
        defeiniteAbsorb = isOn;
    }
    public void toggleDontEncounter(bool isOn)
    {
        dontEncounter = isOn;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addMoney()
    {
        Inventory.Instance.addCurrency(10000);
        if (ShopManager.Instance.isInShop)
        {

            ShopManager.Instance.updateCoin();
        }

    }

    public void unlockAllAbility()
    {
        foreach(var ability in AbilityManager.Instance.abilityDict)
        {
            if(AbilityManager.Instance.abilityLevel.ContainsKey(ability.Key))
            {

                AbilityManager.Instance.abilityButtons[ability.Key].gameObject.SetActive(true);
                AbilityManager.Instance.abilityLevel[ability.Key] = Mathf.Max(0, AbilityManager.Instance.abilityLevel[ability.Key]);
            }

        }
    }

    public void heal()
    {
        if (BattleSystem.Instance.player)
        {
            BattleSystem.Instance.player.heal(1000);
            BattleSystem.Instance.player.restoreMana(1000);
        }
    }
    public void killMonster()
    {
        if (BattleSystem.Instance.monster)
        {
            BattleSystem.Instance.monster.takeDamage(1000);
            BattleSystem.Instance. checkDeath();
        }
    }

    public void suicide()
    {
        if (BattleSystem.Instance.player)
        {
            BattleSystem.Instance.player.takeDamage(1000);
            BattleSystem.Instance.playerDead();
        }
    }
}
