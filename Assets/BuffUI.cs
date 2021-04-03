using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    KeyValuePair<string, BuffInfo> buffInfoPair;
    [SerializeField]
    Image image;
    [SerializeField]
    TMP_Text buffDetailLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showDetailInfo()
    {
        var buffInfo = BuffManager.Instance.buffDictionary[buffInfoPair.Key];
        if(buffInfoPair.Value.round > 0)
        {

            buffDetailLabel.text = string.Format( buffInfo.effectDesc, buffInfoPair.Value.value, buffInfoPair.Value.round);
        }
        else
        {
            buffDetailLabel.text = string.Format(buffInfo.effectDesc, buffInfoPair.Value.value);
        }
    }
    public void hideDetailInfo()
    {
        buffDetailLabel.text = "";
    }



    // Update is called once per frame
    public void UpdateUI(KeyValuePair<string, BuffInfo> pair)
    {
        buffInfoPair = pair;
        Texture2D effectPrefab = Resources.Load<Texture2D>("effectIcon/" + pair.Key);
        if (effectPrefab)
        {
            image.sprite = Sprite.Create(effectPrefab, new Rect(0, 0, effectPrefab.width, effectPrefab.height), new Vector2(0, 0));
        }

    }
}
