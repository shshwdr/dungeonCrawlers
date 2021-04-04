using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : Singleton<CheatManager>
{
    public bool defeiniteAbsorb;
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
        ShopMenu.Instance.updateCoin();

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
}
