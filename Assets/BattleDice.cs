using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDice : Singleton<BattleDice>
{
    public List<Die> dices;
    bool thrown;
    bool finished;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void rollDices()
    {
        if(!thrown && !finished)
        {
            foreach(var die in dices)
            {
                die.gameObject.SetActive(true);
                die.RollDice();
            }
            thrown = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rollDices();
        //}
        if (thrown)
        {
            if (finished)
            {
            }
            else
            {
                bool notFinish = false;
                int value = 0;
                foreach (var die in dices)
                {
                    if (die.value == -1)
                    {
                        notFinish = true;
                        break;
                    }
                    value += die.value;
                }
                finished = !notFinish;
                if (finished)
                {
                    Debug.Log("values " + value);
                    if(HUD.Instance.battleDialogUI)
                    BattleSystem.Instance.finishDice(value);

                    thrown = false;
                    finished = false;
                    //send finished info
                    StartCoroutine(hideDices());
                }
            }
        }   
    }
    IEnumerator hideDices()
    {
        yield return new WaitForSeconds(2);
        foreach (var die in dices)
        {
            die.reset();
            die.gameObject.SetActive(false);
        }
    }
}
