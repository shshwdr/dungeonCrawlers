using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogs : MonoBehaviour
{
    //battle hint words
    public static string enemyAttack = "{0} attacks!";
    public static string enemyShowsUp = "A wild {0} appoaches...";
    public static string winBattle = "You won!";
    public static string lossBattle = "You lost!";
    public static string runFailedBattle = "You failed to run away!";
    public static string runSucceedBattle = "You run away from the monster!";
    public static string throwDice = "Your are throwing dice...";
    public static string afterThrowDice = "you thrown {0} points, choose your actions";
    public static string chooseAction = "you have {0} points, choose your actions";

    public static string attackFailed = "{0} attacked but failed";
    public static string attackFailedAndReflect = "{0} attacked but failed, and get damage from himself.";
    public static string graduallyHeal = "{0} restored {1} hp";

    //ability
    public static string unlockAbility = "You unlocked ability 	<color=red>{0}</color>.";
    public static string abilityMax = "Your ability <color=red>{0}</color> is at max level.";
    public static string abilityExpAdd = "You earned <color=red>{0}</color> for ability <color=red>{1}</color>.";
    public static string abilityLevelUp = " Your ability <color=red>{0}</color> level up!";

    //absorb
    public static string absorbFailed = " You tried to absorb {0} but failed...";
    public static string absorbSuccess = " You successfully absorbed {0}!";


    //battle actions
    public static string playerBasicAttack = "You attack!";
    public static string spellButton = "Select a spell to use";
    public static string itemButton = "Select an item to use";
    public static string runButton = "Run from the battle";

    public static string actionCost = "\tcost {0} action point";
    public static string manaCost = "\tcost {0} mana point";
    public static string actionCostNotEnough = "You don't have enough cost to {0}";
    public static string manaCostNotEnough = "You don't have enough mana to {0}";
}
