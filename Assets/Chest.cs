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
    string[] itemList = new string[]{
        "hpPotion1","hpPotion","spPotion","spPotion1"
        };
    public override void Interact()
    {
        if (!hasCollected)
        {
            hasCollected = true;
            //random coin
            var rand = Random.Range(coinMin, coinMax);
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
            foreach(var item in collectItem){

                Inventory.Instance.addItem(item.Key, item.Value);
            }
        }
        else
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
