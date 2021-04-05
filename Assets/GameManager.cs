using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float timeScale = 2f;
    public bool enemyImmortal;
    public bool playerImmortal;
    public bool noActionCost;
    public bool noManaCost;

    public GameObject cheat;

    public void killMonster() { }

    public void toggleEnemyImmortal(bool value)
    {
        enemyImmortal = value;
    }
    public void toggleplayerImmortal(bool value)
    {
        playerImmortal = value;
    }
    public void togglenoActionCost(bool value)
    {
        noActionCost = value;
    }
    public void togglenoManaCost(bool value)
    {
        noManaCost = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            cheat.SetActive(!cheat.active);
        }
    }
}
