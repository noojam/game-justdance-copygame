using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LoadRank : MonoBehaviour
{
    public List<string> playerNameList = new List<string>();
    public List<float> playerScoreList = new List<float>();
    public TextMeshProUGUI[] rankText;
    public TextMeshProUGUI[] rankScore;
    void Start()
    {
        playerNameList = new List<string>();
        playerScoreList = new List<float>();
        playerNameList = GameManager.playerList;
        playerScoreList = GameManager.playerScoreList;
        List<string> tempList= new List<string>();
        List<float> scores = playerScoreList.OrderByDescending(x => x).ToList();
        for (int i=0;i<playerScoreList.Count;i++)
        {            
            for(int j=0;j<playerScoreList.Count;j++)
            {
                if (playerScoreList[j] == scores[i])
                {
                    tempList.Add(playerNameList[j]);
                }
            }
        }
        for(int i=0;i<rankText.Length;i++)
        {
            rankText[i].text = tempList[i];
            rankScore[i].text = scores[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
