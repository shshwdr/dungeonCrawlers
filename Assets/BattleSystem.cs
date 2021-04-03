using Doozy.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { None, Start,PlayerTurn,PlayerAttack, EnemyTurn, Won,Lost,Absorb}

public struct BuffInfo
{
    //public string buffId;
    public int value;
    public int round;

    public BuffInfo Copy()
    {
        BuffInfo copy = new BuffInfo();
        //copy.buffId = this.buffId;
        copy.value = this.value;
        copy.round = this.round;
        return copy;
    }
}
public class BattleSystem : Singleton<BattleSystem>
{
    public BattleState state;
    [SerializeField]
    float startBattleTime = 1f;
    [SerializeField]
    float attackTime = 1f;

    public Monster monster;
    public BattlePlayer player;

    public bool isPopup;

    public int skillPoint;

    public bool isInBattle { get { return state != BattleState.None; } }


    public Dictionary<string, BuffInfo> playerBuffDict = new Dictionary<string, BuffInfo>() ;
    public Dictionary<string, BuffInfo> monsterBuffDict = new Dictionary<string, BuffInfo>() ;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void StartBattle(ZoneMonsterInfo zoneMonsterInfo, BattlePlayer p, Vector3 monsterPosition, Quaternion monsterRotation)
    {
        string monsterName = zoneMonsterInfo.monsterId;
        int monsterColor = zoneMonsterInfo.color;
        GameObject monster = Instantiate(Resources.Load<GameObject>("monsters/"+ monsterName+ monsterColor), monsterPosition, monsterRotation);
        isPopup = true;
        Monster mo = monster.GetComponent<Monster>();
        mo.level = zoneMonsterInfo.level;
        monster.transform.position = monsterPosition + mo.popupPositionAdjust;
        StartBattle(mo, p);
    }

    public void StartBattle(Monster m, BattlePlayer p)
    {
        if(state == BattleState.None)
        {
            FModSoundManager.Instance.GetIntoBattle();

            monster = m;
            player = p;
            state = BattleState.Start;
            StartCoroutine( SetupBattle());

        }
    }

    IEnumerator SetupBattle()
    {
        playerBuffDict.Clear();
        monsterBuffDict.Clear();
        monster.getIntoBattle();
        player.getIntoBattle();
        GameEventMessage.SendEvent("Battle");

        monster.updateStatusUI();
        player.updateStatusUI();
        //setup enemy hp?
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.enemyShowsUp,monster.getName());
        yield return new WaitForSeconds(startBattleTime);
        state = BattleState.PlayerTurn;

        monster.updateStatusUI();
        player.updateStatusUI();
        StartCoroutine( PlayerTurn());
    }

    public int getPlayerDamage(AbilityInfo info)
    {
        return player.getDamageValue(info, player, monster);
    }

    void addEffect(Dictionary<string, BuffInfo> buffDict, AbilityInfo info)
    {
        var effectType = info.effectType;
        if(effectType == "none")
        {
            return;
        }
        BuffInfo buff;
        //buff.buffId = info.effectType;
        buff.value = info.getEffectValue;
        buff.round = info.effectRound;
        if (buffDict.ContainsKey(effectType))
        {
            if (info.wouldExtend)
            {
                var oldBuff = buffDict[effectType];
                oldBuff.round += info.effectRound;
                buffDict[effectType] = oldBuff;
            }
            else
            {
                var oldBuff = buffDict[effectType];
                oldBuff.round = info.effectRound;
                buffDict[effectType] = oldBuff;
            }
        }
        else
        {
            buffDict[effectType] = buff;
        }

        //update UI
    }

    IEnumerator yieldAndShowText(string t, float delayTime = 2)
    {

        HUD.Instance.battleDialogUI.text = t;
        yield return new WaitForSeconds(delayTime);
    }

    void showParticleEffect(string particleId)
    {
        //show particle effect
        GameObject effectPrefab = Resources.Load<GameObject>("ability/" + particleId);
        if (effectPrefab)
        {

            GameObject effect = Instantiate(effectPrefab, monster.transform.position + effectPrefab.transform.position, effectPrefab.transform.rotation);
        }
    }

    IEnumerator PlayerUseAbility(AbilityInfo info)
    {
        //damage
        HUD.Instance.battleDialogUI.text = Dialogs.playerBasicAttack;


        //add effect
        if (info.effectRound >= 0)
        {
            if (info.applyOnSelf)
            {
                addEffect(playerBuffDict, info);
            }
            else
            {
                addEffect(monsterBuffDict, info);
            }
        }
        else
        {
            //instant effect
            switch (info.effectType)
            {
                case "restoreMana":
                    player.restoreMana(info.getEffectValue);
                    break;
            }
        }

        if(info.descriptionType == "effect")
        {
            //this spell only has effect
        }
        else
        {
            int damageValue = player.getDamageValue(info, player, monster);

            

            bool attackSucceed = true;

            //check if increaseChanceToFail valid
            int failedChance = getBuffValue(player, "increaseChanceToFail");
            if (failedChance>0)
            {
                var rand = Random.Range(0, 100);
                if (rand < failedChance)
                {
                    //failed
                    attackSucceed = false;
                }
            }
            if (attackSucceed)
            {

                player.attack(monster, damageValue);

                //if attacked
                if (info.effectType == "restoreManaFromDamage")
                {
                    int restoredMana = (int)Mathf.Ceil(info.getEffectValue * 0.01f * damageValue);
                    player.restoreMana(restoredMana);
                }
            }
            else
            {
                //missed
                yield return StartCoroutine(yieldAndShowText(string.Format( Dialogs.attackFailed,player.playerStatus.playerName)));
            }
        }

        //show particle effect
        showParticleEffect(info.actionId);

        yield return new WaitForSeconds(attackTime);
        //check if enemy is dead
        if (monster.isDead)
        {
            //end battle
            state = BattleState.Won;
            EndBattle();
        }
        else
        {
            //state = BattleState.EnemyTurn;

            GameEventMessage.SendEvent("Action");
            //StartCoroutine(EnemyTurn());
        }
        //change state to enemy
    }

    public void removeBuff(HPObject checkObject, string buffId)
    {
        if (checkObject is BattlePlayer)
        {
            if (playerBuffDict.ContainsKey(buffId))
            {
                playerBuffDict.ContainsKey(buffId);
            }
            else
            {
                Debug.LogError("buff does not exist get removed");
            }
        }
        else
        {
            if (monsterBuffDict.ContainsKey(buffId))
            {
                monsterBuffDict.ContainsKey(buffId);
            }
            else
            {
                Debug.LogError("buff does not exist get removed");
            }
        }
    }
    public int getBuffValue(HPObject checkObject, string buffId)
    {
        if (checkObject is BattlePlayer)
        {
            if (playerBuffDict.ContainsKey(buffId))
            {
                return playerBuffDict[buffId].value;
            }
        }
        else
        {
            if (monsterBuffDict.ContainsKey(buffId))
            {
                return monsterBuffDict[buffId].value;
            }
        }
        return 0;
    }

    IEnumerator EnemyTurn()
    {
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.enemyAttack, monster.getName());
        yield return new WaitForSeconds(1f);
        bool attackSucceed = true;
        bool missReflect = false;
        //check if failAndReflectHalf valid
        int missReflectChance = getBuffValue(monster, "failAndReflectHalf");
        if (missReflectChance > 0)
        {
            removeBuff(monster, "failAndReflectHalf");
            var rand = Random.Range(0, 100);
            if (rand < missReflectChance)
            {
                //failed
                attackSucceed = false;
                missReflect = true;
            }
        }
        //check if increaseChanceToFail valid
        int failedChance = getBuffValue(monster, "increaseChanceToFail");
        if (failedChance > 0)
        {
            removeBuff(monster, "increaseChanceToFail");
            var rand = Random.Range(0, 100);
            if (rand < failedChance)
            {
                //failed
                attackSucceed = false;
            }
        }
        if (attackSucceed)
        {

            //randomly pick one skill
            var abilities = monster.monsterStatus.ability;

            List<int> monsterRates = new List<int>();
            foreach (var zoneMonsterInfo in abilities)
            {
                monsterRates.Add(zoneMonsterInfo.freq);
                //var monsterInfo = BattleCharacters.Instance.monsterStatusDict[monsterId];
                //Debug.Log("battle monster " + monsterId);
                //BattleSystem.Instance.StartBattle(monsterId, player, monsterPosition, monsterRotation);
            }
            var randomId = Utils.getRandomIdInDistribution(monsterRates);
            var selectedAbility = abilities[randomId];
            var selectedAbilityInfo = MonsterAbilityManager.Instance.abilityDictionary[selectedAbility.abilityName];

            int damageValue = monster.getDamageValue(selectedAbilityInfo, monster, player);
            monster.attack(player, damageValue);
            ////instant effect
            //switch (info.effectType)
            //{
            //    case "restoreMana":
            //        player.restoreMana(info.getEffectValue);
            //        break;
            //}
            yield return StartCoroutine(yieldAndShowText(string.Format(selectedAbilityInfo.description, monster.getName(), selectedAbilityInfo.getDamage)));
        }
        else
        {
            if (missReflect)
            {

                //missed and reflect
                yield return StartCoroutine(yieldAndShowText(string.Format(Dialogs.attackFailedAndReflect, player.playerStatus.playerName)));
            }
            else
            {

                //missed
                yield return StartCoroutine(yieldAndShowText(string.Format(Dialogs.attackFailed, player.playerStatus.playerName)));
            }
        }


        yield return new WaitForSeconds(1f);
        if (player.isDead)

        {
            state = BattleState.Lost;
            EndBattle();
        }
        else
        {

            player.updateBuffUI();
            monster.updateBuffUI();
            state = BattleState.PlayerTurn;

            monster.finishDamage();
            StartCoroutine(PlayerTurn());
        }

    }

    public void OnContinue()
    {
        GameEventMessage.SendEvent("StopAction");
        setSkillPoint(0);
        StartCoroutine(EnemyTurn());
    }

    public void OnAbsorbButton()
    {
        StartCoroutine(OnAbsorb());
    }

    public IEnumerator OnAbsorb()
    {
       // GameEventMessage.SendEvent("StopAction");

        //show particle effect
        //check possibility
        var value = Random.value;
        showParticleEffect("Absorb");
        if (value < monster.monsterStatus.absorbRate)
        {

            yield return StartCoroutine( yieldAndShowText(string.Format(Dialogs.absorbSuccess, monster.getName())));
           // yield return StartCoroutine(yieldAndShowText(string.Format(selectedAbilityInfo.description, monster.monsterStatus.playerName, selectedAbilityInfo.getDamage)));


            string abosrbText = AbilityManager.Instance.addExp(monster.getAbsorbId(), monster.monsterStatus.absorbExp);
            yield return StartCoroutine(yieldAndShowText(abosrbText, 4));
            //end game
            state = BattleState.Absorb;
            //
            monster.gameObject.SetActive(false);
            EndBattle();
        }
        else
        {
            yield return StartCoroutine(yieldAndShowText(string.Format( Dialogs.absorbFailed,monster.getName())));
        }

        GameEventMessage.SendEvent("Action");
        yield return new WaitForSeconds(0.1f);
    }

    void EndBattle()
    {
        GameEventMessage.SendEvent("StopAction");
        if (state == BattleState.Won)
        {

            HUD.Instance.battleDialogUI.text = Dialogs.winBattle;
            StartCoroutine(rewardBattle());
        }
        else if (state == BattleState.Lost)
        {

            HUD.Instance.battleDialogUI.text = Dialogs.lossBattle;
            //load game
        }else if(state == BattleState.Absorb)
        {

            HUD.Instance.battleDialogUI.text = Dialogs.winBattle;
            StartCoroutine(cleanBattle());
        }

    }

    struct GotReward
    {
        public string itemName;
        public bool gotItem;
        public int currency;
    }
    string rewardText(GotReward reward)
    {
        string res = "you earned ";
        res += reward.currency + " coins";
        if(reward.gotItem)
        {
            res += " and "+reward.itemName;
        }
        return res;
    }
    GotReward giveReward()
    {
        GotReward newReward = new GotReward();
        float value = Random.value;
        var reward = monster.monsterStatus.reward;
        if (value <= reward.itemRate)
        {
            newReward.itemName = reward.itemName;
            newReward.gotItem = true;
            Inventory.Instance.addItem(newReward.itemName, 1);
        }
        newReward.currency = Random.Range(reward.currencyMin * Utils.currencyScale, reward.currencyMax * Utils.currencyScale);
        Inventory.Instance.addCurrency(newReward.currency);

        return newReward;
    }

    IEnumerator rewardBattle()
    {
        //get item
        //get exp
        var reward = giveReward();
        var text = rewardText(reward);
        HUD.Instance.battleDialogUI.text = text;
        yield return new WaitForSeconds(3f);
        StartCoroutine(cleanBattle());
    }

    IEnumerator cleanBattle()
    {
        monster.clearCamera();
        yield return new WaitForSeconds(1f);
        GameEventMessage.SendEvent("StopBattle");
        if (state == BattleState.Won || state == BattleState.Absorb || isPopup)
        {

            Destroy(monster.gameObject);//better way to die?
        }
        isPopup = false;
        state = BattleState.None;

        monsterBuffDict.Clear();
        playerBuffDict.Clear();

        //get music back
        LayerMask layer = 1 << LayerMask.NameToLayer("DangerZone");
        //check if need to trigger battle

        var overlaps = Physics.OverlapSphere(transform.position, 0.1f, layer);
        if (overlaps.Length >= 1)
        {
            FModSoundManager.Instance.GetIntoDangerZone();
        }
        else
        {
            FModSoundManager.Instance.GetIntoSafeZone();
        }
    }
    void UpdateOneTypeBuff(Dictionary<string,BuffInfo> buffDict)
    {
        Dictionary<string, BuffInfo> newBuffDict = new Dictionary<string, BuffInfo>();
        foreach (var pair in buffDict)
        {
            var oldBuff = pair.Value;
            var newBuff = oldBuff.Copy();
            if (oldBuff.round > 0)
            {
                var round = oldBuff.round - 1;
                if (round > 0)
                {
                    newBuff.round = round;
                    newBuffDict[pair.Key] = newBuff;
                }

            }
            else
            {

                newBuffDict[pair.Key] = newBuff;
            }
        }
        buffDict.Clear();
        foreach(var pair in newBuffDict)
        {
            buffDict[pair.Key] = newBuffDict[pair.Key];
        }
    }
    void UpdateBuffs()
    {
        UpdateOneTypeBuff(playerBuffDict);
        UpdateOneTypeBuff(monsterBuffDict);
        player.updateBuffUI();
        monster.updateBuffUI();
    }
    IEnumerator PlayerTurn()
    {
        //restoreHPGradually effect
        foreach (var pair in playerBuffDict)
        {
            switch (pair.Key) 
            {
                case "restoreHPGradually":
                    player.heal(pair.Value.value);
                    yield return StartCoroutine( yieldAndShowText(string.Format( Dialogs.graduallyHeal, player.playerStatus.playerName, pair.Value.value)));
                    break;
            }
        }


        UpdateBuffs();
        BattleDice.Instance.rollDices();

        yield return new WaitForSeconds(0.1f);

        HUD.Instance.battleDialogUI.text = Dialogs.throwDice;
        //HUD.Instance.battleDialogUI.text = "Choose an action:";
    }

    public void finishDice(int value)
    {

        GameEventMessage.SendEvent("Action");

        setSkillPoint(value);
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.afterThrowDice, skillPoint);
    }
    public void OnHeal(int value)
    {
        player.heal(value);
    }

    public void OnAbilityButton(AbilityInfo info)
    {

        GameEventMessage.SendEvent("StopAction");
        //play particle effect
        StartCoroutine(PlayerUseAbility(info));


        player.updateBuffUI();
        monster.updateBuffUI();
    }


    public void OnRun()
    {
        GameEventMessage.SendEvent("StopAction");
        var runRate = monster.monsterStatus.runRate;
        var randValue = Random.value;
        if (randValue <= runRate)
        {
            //run succeed

            HUD.Instance.battleDialogUI.text = Dialogs.runSucceedBattle;
            StartCoroutine(cleanBattle());
        }
        else
        {
            StartCoroutine(runFailed());
        }
    }

    IEnumerator runFailed()
    {

        HUD.Instance.battleDialogUI.text = Dialogs.runFailedBattle;
        yield return new WaitForSeconds(2f);
        OnContinue();

    }

    public void updateSkillPoint(int minus)
    {
        setSkillPoint(skillPoint - minus);
    }
    public void setSkillPoint(int value)
    {
        skillPoint = value;
        HUD.Instance.skillPointUI.text = skillPoint.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
