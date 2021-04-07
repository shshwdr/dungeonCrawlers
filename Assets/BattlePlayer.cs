using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : HPObject
{
    public Vector3 originPosition;
    public BattlePlayerStatus playerStatus;
    public AudioSource audioSource;

    protected int currentSP;
    // Start is called before the first frame update
    protected override void Start()
    {

        originPosition = transform.position;
        stateUI = HUD.Instance.playerUI;
        stateUI2 = HUD.Instance.playerUI2;
        base.Start();
        Init(BattleCharacters.Instance.playerStatus);
        currentSP = playerStatus.mana;
        BattleSystem.Instance.player = this;
        initStatusUI();

    }
    protected override void Init(BattleCharacterStatus s)
    {
        playerStatus = (BattlePlayerStatus) s;
        base.Init(s);
        
    }

    public override int getMaxHp()
    {
        return Inventory.Instance.playerStatus["hp"].getLevelValue;
    }

    public override int getAttack()
    {
        return Inventory.Instance.playerStatus["atk"].getLevelValue;
    }


    public override int getDef()
    {
        return Inventory.Instance.playerStatus["def"].getLevelValue;
    }

    public override int getMagic()
    {
        return Inventory.Instance.playerStatus["mag"].getLevelValue;
    }
    public override int getMagDef()
    {
        return Inventory.Instance.playerStatus["magDef"].getLevelValue;
    }

    public void getIntoBattle()
    {
        initStatusUI();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void takeDamage(int damage)
    {
        if (!GameManager.Instance.playerImmortal)
        {
            base.takeDamage(damage);
        }
    }

    public override void updateStatusUI()
    {
        base.updateStatusUI();
        stateUI.updateSP(currentSP);
        if (stateUI2)
        {

            stateUI2.updateSP(currentSP);
        }
    }

    public void updateBuffUI()
    {
        stateUI.updateBuff(BattleSystem.Instance.playerBuffDict);
    }

    public void consumeMana(int sp)
    {
        currentSP -= sp;
        stateUI.updateSP(currentSP);
    }

    public bool  canUseMana(int sp)
    {
        return currentSP >= sp;
    }

    public void restoreMana(int sp)
    {
        currentSP += sp;
        currentSP = Mathf.Clamp(currentSP, 0, getMana());

        updateStatusUI();
    }

    public int getMana()
    {

        return Inventory.Instance.playerStatus["mana"].getLevelValue;
    }

    public void initStatusUIPublic()
    {
        initStatusUI();
    }
    protected override void initStatusUI()
    {
        stateUI.updateMaxSP(getMana()); 
        if (stateUI2)
        {
            stateUI2.updateMaxSP(getMana());
        }
        updateBuffUI();
        base.initStatusUI();
    }

    public void Reset()
    {
        transform.position = originPosition;
        GetComponent<GridMovement>().Reset();
    }
    public void revive()
    {
        isDead = false;
    }

}
