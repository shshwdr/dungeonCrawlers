using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    PurchaseItem item;
    public TMP_Text nameLabel;
    public TMP_Text costLabel;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        if (item != null)
        {
            updateUI();
        }
        button = GetComponent<Button>();
    }

    void updateUI()
    {

        nameLabel.text = item.getItemName;
        costLabel.text = item.getCost.ToString();
    }
    public void Init(PurchaseItem it)
    {
        item = it;
        if (nameLabel)
        {
            updateUI();
        }
    }

    public void OnEnter()
    {
        if (item is PlayerPurchaseStatus)
        {
            var pitem = (PlayerPurchaseStatus)item;
            var level = Inventory.Instance.statusLevel[pitem.itemId];
            ShopManager.Instance.detailLabel.text = string.Format("{0} from level {1} to level {2}\nincrease the value from {3} to {4}.",
                pitem.itemName, level, level+1,pitem.getLevelValue,pitem.getNextLevelValue );
        }else if (item is PurchaseInventory)
        {
            var itemAction = Inventory.Instance.itemInfoDict[item.itemId];
            ShopManager.Instance.detailLabel.text = "Purchase " + itemAction.actionName+"\n";
            ShopManager.Instance.detailLabel.text += string.Format(itemAction.description,itemAction.param);
        }
        else if(item is PurchaseHeal)
        {
            ShopManager.Instance.detailLabel.text = item.getItemName;
        }
    }
    public void OnExit()
    {
        ShopManager.Instance.detailLabel.text = "";
    }
    public void OnClick()
    {
        Inventory.Instance.purchase(item);
        ShopManager.Instance.updateShop();
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = Inventory.Instance.canPurchase(item);
    }
}
