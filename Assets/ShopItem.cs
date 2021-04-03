using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    PurchaseItem item;
    public TMP_Text nameLabel;
    public TMP_Text costLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (item != null)
        {
            nameLabel.text = item.itemName;
            //costLabel.text = item.
        }
    }
    public void Init(PurchaseItem it)
    {
        item = it;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
