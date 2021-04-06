using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopMenu : Singleton<ShopMenu>
{
    public TMP_Text detailLabel;
    public TMP_Text coinLabel;

    // Start is called before the first frame update
    void Start()
    {
        updateCoin();
    }

    public void updateCoin()
    {
        if (coinLabel)
        {
            coinLabel.text = "Coin: " + Inventory.Instance.currentCurrency;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
