using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Doozy.Engine.Progress;

public class BattleStateUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text Name;
    [SerializeField]
    TMP_Text level;
    [SerializeField]
    Progressor hpProgressor;
    [SerializeField]
    Progressor spProgressor;

    [SerializeField]
    TMP_Text statusText;
    [SerializeField]
    GameObject statusGameObject;
    [SerializeField]
    BuffsUI buffsUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public BattleStateUI updateName(string n)
    {

        Name.text = n;
        return this;
    }
    public BattleStateUI updateLevel(string n)
    {

        level.text = n;
        return this;
    }
    public BattleStateUI updateMaxHP(int hp)
    {

        hpProgressor.SetMax(hp);
        return this;
    }
    public BattleStateUI updateHP(int hp)
    {

        hpProgressor.SetValue(hp);
        return this;
    }

    public BattleStateUI updateSP(int sp)
    {

        spProgressor.SetValue(sp);
        return this;
    }

    public BattleStateUI updateMaxSP(int sp)
    {

        spProgressor.SetMax(sp);
        return this;
    }

    public BattleStateUI updateBuff(Dictionary<string, BuffInfo> buffDict)
    {
        buffsUI.UpdateUI(buffDict);
        return this;
    }
    HPObject ob;
    public void updateHPObject(HPObject o)
    {
        ob = o;
    }
    public void showStates()
    {
        statusGameObject.SetActive(true);
        statusText.text = "atk: " + ob.getAttack();
        statusText.text += "\ndef: " + ob.getBattleDef();
        statusText.text += "\nmag: " + ob.getMagic();
        statusText.text += "\nmag def: " + ob.getBattleMagDef();
    }

    public void hideStates()
    {

        statusGameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
