using Doozy.Engine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    Transform originPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void showView()
    //{

    //}
     void ReviveFullHealth()
    {
        BattleSystem.Instance.player.revive();
        BattleSystem.Instance.player.healPercent(100);
    }
     void Revive20Health()
    {
        BattleSystem.Instance.player.revive();
        BattleSystem.Instance. player.healPercent(20);
    }
    public void reviveImmediately()
    {
        
        Revive20Health();
        Inventory.Instance.lossCurrency(0.1f);

        DialogueManager.ShowAlert("You get healed 20% hp and lost 10% coins");

        GameEventMessage.SendEvent("ExitGameOver");
    }
    public void reviveOrigin()
    {

        ReviveFullHealth();
        BattleSystem.Instance.player.Reset();
        DialogueManager.ShowAlert("You get healed.");
        GameEventMessage.SendEvent("ExitGameOver");

        DialogueManager.StartConversation("frogHeal");
    }

    //public void getBackToOrigin()
    //{

    //    BattleSystem.Instance.player.Reset();
    //}
}
