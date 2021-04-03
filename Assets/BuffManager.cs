using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class VisualBuffInfo
{
    public string effectId;
    public string effectDesc;
    public string doEffectDesc;
}

[Serializable]
public class AllBuffInfo
{
    public List<VisualBuffInfo> buff;
}
public class BuffManager : Singleton<BuffManager>
{
    [SerializeField]
    TextAsset jsonFile;
    public Dictionary<string, VisualBuffInfo> buffDictionary;
    // Start is called before the first frame update
    void Start()
    {
        AllBuffInfo allActionInfoList = JsonUtility.FromJson<AllBuffInfo>(jsonFile.text);
        buffDictionary = allActionInfoList.buff.ToDictionary(x => x.effectId, x => x);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
