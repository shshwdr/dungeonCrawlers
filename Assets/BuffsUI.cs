using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffsUI : MonoBehaviour
{
    [SerializeField]
    Transform buffsParent;
    List<BuffUI> buffs = new List<BuffUI>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform buffTransform in buffsParent)
        {
            buffs.Add(buffTransform.GetComponent<BuffUI>());
        }
    }

    public void UpdateUI(Dictionary<string, BuffInfo> buffDict)
    {
        int i = 0;
        foreach(var pair in buffDict)
        {
            buffs[i].gameObject.SetActive(true);
            buffs[i].UpdateUI(pair);
            i++;
        }
        for (; i < buffs.Count; i++)
        {

            buffs[i].gameObject.SetActive(false);
        }
    }
}
