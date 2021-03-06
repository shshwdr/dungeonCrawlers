using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{


    protected BattleCharacterStatus status;

    protected int currentHP;
    protected BattleStateUI stateUI;
    protected BattleStateUI stateUI2;

    public bool isDead;
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    protected virtual void Init(BattleCharacterStatus s)
    {
        status = s;
        currentHP = getMaxHp();
    }

    public void UpdateStatus()
    {

    }

    protected virtual void initStatusUI()
    {
        stateUI.updateHPObject(this);
        if (stateUI2) { stateUI2.updateHPObject(this); }
        stateUI.updateName(status.playerName).updateMaxHP(getMaxHp());
        if (stateUI2)
            stateUI2.updateName(status.playerName).updateMaxHP(getMaxHp());
        updateStatusUI();
    }

    public virtual int getMaxHp()
    {
        return status.hp;
    }

    public virtual void updateStatusUI()
    {
        stateUI.updateHP(currentHP);
        if (stateUI2)
            stateUI2.updateHP(currentHP);
    }

    public virtual void heal(int value)
    {
        currentHP += value;
        currentHP = Mathf.Clamp(currentHP, 0, getMaxHp());

        updateStatusUI();
    }

    public virtual void healPercent(int value)
    {

        currentHP += value*getMaxHp()/100;
        currentHP = Mathf.Clamp(currentHP, 0, getMaxHp());

        updateStatusUI();
    }


    public virtual void takeDamage(int damage )
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, getMaxHp());
        updateStatusUI();

        if (currentHP <= 0)
        {
            die();
        }
    }

    protected virtual void die()
    {
        isDead = true;
    }


    public virtual int getAttack()
    {
        return status.atk;
    }


    public virtual  int getDef()
    {
        return status.def;
    }

    public int getBattleDef()
    {
        return Mathf.Clamp(getDef() + BattleSystem.Instance.getBuffValue(this, "increaseBDef"),0,50);
    }
    public virtual int getMagic()
    {
        return status.mag;
    }
    public virtual int getMagDef()
    {
        return status.magdef;
    }
    public int getBattleMagDef()
    {
        return Mathf.Clamp(getMagDef() + BattleSystem.Instance.getBuffValue(this, "increaseBDef") - BattleSystem.Instance.getBuffValue(this, "removeMDef"), 0, 50);
    }



    public virtual void attack(HPObject attakee, int damage)
    {
            attakee.takeDamage(damage);
    }
    // Update is called once per frame

    public int getDamageValue(AbilityInfo info, HPObject attacker, HPObject attackee, bool isNextLevel = false)
    {
        //get info attack
        float abilityAttack = info.getDamage;
        if (isNextLevel)
        {
            abilityAttack = info.getNextDamage;
        }

        if (info.abilityType == "physical")
        {
            //apply attacker atk
            abilityAttack = (1 + attacker.getAttack() * 0.01f) * abilityAttack;
            //apply attackee def
            var attackeeDef = 0;
            if (attackee)
            {

                attackee.getDef();

                // check if player has increaseBDef buff on it
                attackeeDef += BattleSystem.Instance.getBuffValue(attackee, "increaseBDef");

                //if attack ignore def
                if (info.effectType == "ignoreDef")
                {
                    attackeeDef -= info.getEffectValue;
                }
                attackeeDef = Mathf.Clamp(attackeeDef, 0, 50);
            }
            abilityAttack = (1 - attackeeDef * 0.01f) * abilityAttack;
        }
        else if (info.abilityType == "magical")
        {
            //apply attacker atk
            abilityAttack = (1 + attacker.getMagic() * 0.01f) * abilityAttack;
            //apply attackee def
            var attackeeDef = 0;
            if (attackee)
            {
                attackee.getMagDef();

                attackeeDef -= BattleSystem.Instance.getBuffValue(attackee, "removeMDef");

                // check if player has increaseBDef buff on it
                attackeeDef += BattleSystem.Instance.getBuffValue(attackee, "increaseBDef");

                //if attack ignore def
                if (info.effectType == "ignoreDef")
                {
                    attackeeDef -= info.getEffectValue;
                }
                attackeeDef = Mathf.Clamp(attackeeDef, 0, 50);
            }
            
            abilityAttack = (1 - attackeeDef * 0.01f) * abilityAttack;
        }

        return (int)Mathf.Ceil( abilityAttack);
    }
}
