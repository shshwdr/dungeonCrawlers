using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;

public class HUD : Singleton<HUD>
{
    public TMP_Text battleDialogUI;
    public TMP_Text skillPointUI;
    public BattleStateUI enemyUI;
    public BattleStateUI playerUI;
    public BattleStateUI playerUI2;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
    public GameObject skillActions;
    public GameObject skillHint;
    public GameObject itemActions;
    public GameObject generalActions;

    public void showSkillActions()
    {
        skillActions.SetActive(true);

        if (!BattleSystem.Instance.successAbsorb)
        {
            skillHint.SetActive(true);
        }

        itemActions.SetActive(false);
        generalActions.SetActive(false);
    }

    public void showItemActions()
    {

        skillHint.SetActive(false);
        skillActions.SetActive(false);
        itemActions.SetActive(true);
        generalActions.SetActive(false);
    }

    public void showGeneraActions()
    {

        skillHint.SetActive(false);
        skillActions.SetActive(false);
        itemActions.SetActive(false);
        generalActions.SetActive(true);
    }
}