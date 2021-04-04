using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable:MonoBehaviour
{
    public virtual void Interact()
    {

    }
}

public class Chest : Interactable
{
    [SerializeField]
    int coinMin = 5;
    [SerializeField]
    int coinMax = 10;

    [SerializeField]
    int itemCount = 5;
    [SerializeField]
    bool hasCollected = false;

    int showTime = 5;
    bool isInteracting = false;
    float countDown = 1;

    string[] itemList = new string[]{
        "hpPotion1","hpPotion","spPotion","spPotion1"
        };
    public override void Interact()
    {
        if (isInteracting)
        {
            return;
        }
        isInteracting = true;
        countDown = showTime;
        if (!hasCollected)
        {
            hasCollected = true;
            //random coin
            var rand = Random.Range(coinMin * Utils.currencyScale, coinMax * Utils.currencyScale);
            Inventory.Instance.addCurrency(rand * Utils.currencyScale);
            Dictionary<string,int> collectItem = new Dictionary<string, int>();
            for (int i = 0;i< itemCount; i++)
            {
                var r = Random.Range(0, itemList.Length);
                var item = itemList[r];
                if (!collectItem.ContainsKey(item))
                {
                    collectItem[item] = 0;
                }
                collectItem[item]++;
            }
            string res = string.Format("You got {0} coins", rand);
            foreach(var item in collectItem){

                Inventory.Instance.addItem(item.Key, item.Value);
                res += string.Format(" {0} {1}", item.Value, Inventory.Instance. itemInfoDict[item.Key].actionName);
            }

            DialogueManager.ShowAlert(res, showTime);
        }
        else
        {
            DialogueManager.ShowAlert("It is empty");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0) {
            isInteracting = false;
        }
    }
}
