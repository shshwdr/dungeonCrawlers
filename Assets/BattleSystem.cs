using Doozy.Engine;
using PixelCrushers.DialogueSystem;
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
    [SerializeField]
    GameObject diceImage;

    public Monster monster;
    public BattlePlayer player;

    public bool isPopup;

    public int skillPoint;
    public int keepSkillPoint = 5;
    public bool successAbsorb = false;
    public bool isInBattle { get { return state != BattleState.None; } }


    public AudioClip absorbClip;
    public AudioClip runClip;
    public AudioClip throwDiceClip;


    AudioSource audioSource;

    public Dictionary<string, BuffInfo> playerBuffDict = new Dictionary<string, BuffInfo>() ;
    public Dictionary<string, BuffInfo> monsterBuffDict = new Dictionary<string, BuffInfo>() ;
    bool isFirst = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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


            monster = m;
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
        diceImage.SetActive(true);
        monster.updateStatusUI();
        player.updateStatusUI();

        if (monster.monsterStatus.absorbCost == 0)
        {

            FModSoundManager.Instance.GetIntoBossBattle();
        }
        else
        {

            FModSoundManager.Instance.GetIntoBattle();
        }
        //set up action buttons
        if (monster.getAbsorbId() == "")
        {
            BattleActions.Instance.actionButtonDictionary["absorb"].gameObject.SetActive(false);
        }
        else
        {
            BattleActions.Instance.actionButtonDictionary["absorb"].gameObject.SetActive(true);
        }

        //setup enemy hp?
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.enemyShowsUp,monster.getName());
        yield return new WaitForSeconds(startBattleTime);
        state = BattleState.PlayerTurn;

        monster.updateStatusUI();
        player.updateStatusUI();
        StartCoroutine( PlayerTurn());
    }

    public int getPlayerDamage(AbilityInfo info, bool isNextLevel = false)
    {
        return player.getDamageValue(info, player, monster,isNextLevel);
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
    void applyEffect(AbilityInfo info, HPObject attacker)
    {

        bool isPlayer = attacker is BattlePlayer;
        //add effect
        if (info.effectRound >= 0)
        {
            if (info.applyOnSelf== isPlayer)
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

                    ((BattlePlayer)attacker).restoreMana(info.getEffectValue);
                    break;
                case "healPercent":
                    attacker.heal(info.getEffectValue * attacker.getMaxHp()/100);
                    break;
            }
        }
    }
    IEnumerator PlayerUseAbility(AbilityInfo info)
    {
        //damage
        HUD.Instance.battleDialogUI.text = info.doActionDesc;

        applyEffect(info, player);

        if (info.descriptionType == "effect")
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
                removeBuff(player, "increaseChanceToFail");
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
        if (checkDeath()) { }
        else
        {
            //state = BattleState.EnemyTurn;

            GameEventMessage.SendEvent("Action");
            //StartCoroutine(EnemyTurn());
        }
        //change state to enemy
    }

    public bool checkDeath()
    {

        //check if enemy is dead
        if (monster.isDead)
        {
            //end battle
            state = BattleState.Won;
            EndBattle();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void removeBuff(HPObject checkObject, string buffId)
    {
        if (checkObject is BattlePlayer)
        {
            if (playerBuffDict.ContainsKey(buffId))
            {
                playerBuffDict.Remove(buffId);
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
                monsterBuffDict.Remove(buffId);
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
        monster.startAttackCamera();
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.enemyAttack, monster.getName());
        yield return new WaitForSeconds(1f);


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

        //show particle effect
        showParticleEffect(selectedAbilityInfo.actionId);
        if (selectedAbilityInfo.descriptionType == "attack")
        {
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

                int damageValue = monster.getDamageValue(selectedAbilityInfo, monster, player);
                monster.attack(player, damageValue);
                ////instant effect
                //switch (info.effectType)
                //{
                //    case "restoreMana":
                //        player.restoreMana(info.getEffectValue);
                //        break;
                //}
                yield return StartCoroutine(yieldAndShowText(string.Format(selectedAbilityInfo.description, monster.getName(), damageValue)));
            }
            else
            {
                if (missReflect)
                {

                    //missed and reflect
                    yield return StartCoroutine(yieldAndShowText(string.Format(Dialogs.attackFailedAndReflect, monster.monsterStatus.playerName)));
                }
                else
                {

                    //missed
                    yield return StartCoroutine(yieldAndShowText(string.Format(Dialogs.attackFailed, monster.monsterStatus.playerName)));
                }
            }
        }
        else
        {
            applyEffect(selectedAbilityInfo, monster);
            yield return StartCoroutine(yieldAndShowText(string.Format(selectedAbilityInfo.description, monster.getName())));
        }


        


        yield return new WaitForSeconds(1f);
        if (player.isDead)

        {
            playerDead();
        }
        else
        {

            player.updateBuffUI();
            monster.updateBuffUI();
            state = BattleState.PlayerTurn;

            monster.finishDamage();
            monster.finishAttackCamera();
            StartCoroutine(PlayerTurn());
        }

    }

    public void playerDead()
    {
        state = BattleState.Lost;
        EndBattle();
    }

    public void OnContinue()
    {
        GameEventMessage.SendEvent("StopAction");
        var leftPoint = Mathf.Min(skillPoint, keepSkillPoint);
        setSkillPoint(leftPoint);
        StartCoroutine(EnemyTurn());
    }

    public void OnAbsorbButton()
    {
        StartCoroutine(OnAbsorb());
    }

    public IEnumerator OnAbsorb()
    {
        GameEventMessage.SendEvent("StopAction");

        //show particle effect
        //check possibility
        var value = Random.value;
        audioSource.PlayOneShot(absorbClip);
        showParticleEffect("Absorb");
        if (value < monster.monsterStatus.absorbRate || CheatManager.Instance.defeiniteAbsorb)
        {
            successAbsorb = true;
            yield return StartCoroutine( yieldAndShowText(string.Format(Dialogs.absorbSuccess, monster.getName())));
           // yield return StartCoroutine(yieldAndShowText(string.Format(selectedAbilityInfo.description, monster.monsterStatus.playerName, selectedAbilityInfo.getDamage)));


            string abosrbText = AbilityManager.Instance.addExp(monster.getAbsorbId(), monster.monsterStatus.absorbExp);

            DialogueManager.ShowAlert(abosrbText);
            yield return StartCoroutine(yieldAndShowText(abosrbText, 4));
            //end game
            state = BattleState.Absorb;
            //
            //monster.gameObject.SetActive(false);
            EndBattle();
        }
        else
        {
            yield return StartCoroutine(yieldAndShowText(string.Format( Dialogs.absorbFailed,monster.getName())));
            GameEventMessage.SendEvent("Action");
        }

    }

    public void EndBattle()
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

            StartCoroutine(cleanBattle());
        }
        else if(state == BattleState.Absorb)
        {

            HUD.Instance.battleDialogUI.text = Dialogs.winBattle;
            StartCoroutine(cleanBattle());
        }
        else
        {
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
            newReward.itemName = Inventory.Instance.itemInfoDict[reward.itemName].actionName ;
            newReward.gotItem = true;
            Inventory.Instance.addItem(reward.itemName, 1);
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

        DialogueManager.ShowAlert(text);
        yield return new WaitForSeconds(3f);
        StartCoroutine(cleanBattle());
    }

    IEnumerator cleanBattle()
    {
        skillPoint = 0;
        diceImage.SetActive(false);
        if (monster)
            monster.clearCamera();
        //yield return new WaitForSeconds(1f);
        GameEventMessage.SendEvent("StopBattle");

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
        if (state == BattleState.Won || state == BattleState.Absorb || isPopup)
        {

            if(monster.monsterStatus.playerName == "Magic Lizard")
            {
                monster.dieIdle();

                yield return new WaitForSeconds(0.5f);
                DialogueManager.StartConversation("finish");
            }
            else
            {

                monster.fullyDie();
                yield return new WaitForSeconds(0.5f);
                if (monster)
                {

                    Destroy(monster.gameObject);
                }
            }

        }
        if (state == BattleState.Lost)
        {

            GameEventMessage.SendEvent("GameOver");
        }
        state = BattleState.None;
        isPopup = false;
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

        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(throwDiceClip);
        //HUD.Instance.battleDialogUI.text = "Choose an action:";
    }

    public void finishDice(int value)
    {

        GameEventMessage.SendEvent("Action");

        updateSkillPoint(-value);
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.chooseAction, skillPoint);
        if (isFirst)
        {
            isFirst = false;
            StartCoroutine(showHint());
        }
    }

    IEnumerator showHint()
    {
        yield return new WaitForSeconds(0.5f);

        DialogueManager.StartConversation("attack");
    }
    public void OnHeal(int value)
    {
        player.heal(value);
    }
    public void OnRestoreSP(int value)
    {
        player.restoreMana(value);
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

        audioSource.PlayOneShot(runClip);
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
