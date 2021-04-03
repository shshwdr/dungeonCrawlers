using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static int currencyScale = 10;
    public static int hpScale = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public string RemoveNumbers(string text, out int num)
    {
        string newText = "";
        string number = "";
        for (int i = 0; i < text.Length; i++)
        {
            if ((text[i] < 48) || (text[i] > 57))
            { //is a char
                newText += text[i];
            }
            else
            { //is number
                number += text[i];
            }
        }
        num = int.Parse(number);
        return newText;
    }


    public static int getRandomIdInDistribution(List<int> distribution)
    {
        int sum = 0;
        foreach (var i in distribution)
        {
            sum += i;
        }
        int rand = Random.Range(0, sum );
        sum = 0;
        for(int i = 0;i<distribution.Count;i++)
        {
            sum += distribution[i];
            if (rand < sum)
            {
                return i;
            }
        }
        Debug.LogError("calculation wrong");
        return -1;
    }
}
