using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{


    protected BattleCharacterStatus status;

    protected int currentHP;
    protected BattleStateUI stateUI;

    public bool isDead;
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    protected virtual void Init(BattleCharacterStatus s)
    {
        status = s;
        currentHP = status.hp;
    }

    public void UpdateStatus()
    {

    }

    protected virtual void initStatusUI()
    {
        stateUI.updateHPObject(this);
        stateUI.updateName(status.playerName).updateMaxHP(status.hp);
        updateStatusUI();
    }

    public virtual void updateStatusUI()
    {
        stateUI.updateHP(currentHP);
    }

    public virtual void heal(int value)
    {
        currentHP += value;
        currentHP = Mathf.Clamp(currentHP, 0, status.hp);

        updateStatusUI();
    }


    public virtual void takeDamage(int damage )
    {
        currentHP -= damage;
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

    public int getAttack()
    {
        return status.atk;
    }

    public int getBattleAttack()
    {
        return status.atk;
    }

    public int getDef()
    {
        return status.def;
    }

    public int getBattleDef()
    {
        return Mathf.Clamp( status.def + BattleSystem.Instance.getBuffValue(this, "increaseBDef"),0,50);
    }
    public int getMagic()
    {
        return status.mag;
    }
    public int getMagDef()
    {
        return status.magdef;
    }
    public int getBattleMagDef()
    {
        return Mathf.Clamp(status.magdef + BattleSystem.Instance.getBuffValue(this, "increaseBDef") - BattleSystem.Instance.getBuffValue(this, "removeMDef"), 0, 50);
    }

    public virtual void attack(HPObject attakee, int damage)
    {
            attakee.takeDamage(damage);
    }
    // Update is called once per frame

    public int getDamageValue(AbilityInfo info, HPObject attacker, HPObject attackee)
    {
        //get info attack
        float abilityAttack = info.getDamage;

        if (info.abilityType == "physical")
        {
            //apply attacker atk
            abilityAttack = (1 + attacker.getAttack() * 0.01f) * abilityAttack;
            //apply attackee def
            var attackeeDef = attackee.getDef();

            // check if player has increaseBDef buff on it
            attackeeDef += BattleSystem.Instance.getBuffValue(attackee, "increaseBDef");

            //if attack ignore def
            if (info.effectType == "ignoreDef")
            {
                attackeeDef -= info.getEffectValue;
            }
            attackeeDef = Mathf.Clamp(attackeeDef, 0, 50);
            abilityAttack = (1 - attackeeDef * 0.01f) * abilityAttack;
        }
        else if (info.abilityType == "magical")
        {
            //apply attacker atk
            abilityAttack = (1 + attacker.getMagic() * 0.01f) * abilityAttack;
            //apply attackee def
            var attackeeDef = attackee.getMagDef();

            attackeeDef -= BattleSystem.Instance.getBuffValue(attackee, "removeMDef");

            // check if player has increaseBDef buff on it
            attackeeDef += BattleSystem.Instance.getBuffValue(attackee, "increaseBDef");
            
            //if attack ignore def
            if (info.effectType == "ignoreDef")
            {
                attackeeDef -= info.getEffectValue;
            }
            attackeeDef = Mathf.Clamp(attackeeDef, 0, 50); 
            abilityAttack = (1 - attackeeDef * 0.01f) * abilityAttack;
        }

        return (int)Mathf.Ceil( abilityAttack);
    }
}
