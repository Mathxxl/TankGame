using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

public class TextUpdate : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] private RandomPondered randomPondered;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeNumber()
    {
        text.text = randomPondered.GetRandom().ToString();
    }

    public void TestPonderation(int tests = 1000)
    {
        int[] results = new int[randomPondered.Count];
        for (int i = 0; i < tests; i++)
        {
            int value = randomPondered.GetRandom();
            results[value]++;
        }

        string sresult = "[";
        foreach (var v in results)
        {
            sresult += v.ToString() + " ";
        }

        sresult += "]";
        Debug.Log(sresult);
    }

    public void AddAt(int index)
    {
        randomPondered.AddAt(index, 10);
    }
    
    public void AddWeight(int index)
    {
        randomPondered.AddWeight(index, 10);
    }
}
