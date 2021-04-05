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
    public GameObject itemActions;
    public GameObject generalActions;

    public void showSkillActions()
    {
        skillActions.SetActive(true);
        itemActions.SetActive(false);
        generalActions.SetActive(false);
    }

    public void showItemActions()
    {

        skillActions.SetActive(false);
        itemActions.SetActive(true);
        generalActions.SetActive(false);
    }

    public void showGeneraActions()
    {

        skillActions.SetActive(false);
        itemActions.SetActive(false);
        generalActions.SetActive(true);
    }
}