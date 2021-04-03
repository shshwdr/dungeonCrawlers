using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : HPObject
{
    public BattlePlayerStatus playerStatus;


    protected int currentSP;
    // Start is called before the first frame update
    protected override void Start()
    {


        stateUI = HUD.Instance.playerUI;
        base.Start();
        Init(BattleCharacters.Instance.playerStatus);
        currentSP = playerStatus.mana;

    }
    protected override void Init(BattleCharacterStatus s)
    {
        playerStatus = (BattlePlayerStatus) s;
        base.Init(s);
        
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
        currentSP = Mathf.Clamp(currentSP, 0, playerStatus.mana);

        updateStatusUI();
    }

    protected override void initStatusUI()
    {
        stateUI.updateMaxSP(playerStatus.mana);
        updateBuffUI();
        base.initStatusUI();
    }

}
