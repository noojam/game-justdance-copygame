using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ready;
    public RectTransform TargetObj;
    public Animator animatorIpi;
    public Animator animatorSanta;
    public BodySourceView bodySourceView;
    public bool onReady;
    public int PlayerCount;
    public GameObject P1Nametag;
    public GameObject P2Nametag;
    public GameObject P3Nametag;
    public GameObject[] PlayerNames;
    public RectTransform[] TagTransform;
    public GameObject[] Sliders;
    public GameObject FinishPanel;
    public GameObject NextButton;
    public GameObject StartButton;
    public TextMeshProUGUI[] playerNameTags;
    public TextMeshProUGUI[] playerNames;
    public TextMeshProUGUI[] FinishplayerNames;
    public TextMeshProUGUI[] FinishplayerNames2;
    public TextMeshProUGUI[] FinishplayerRanks;
    public static List<string> playerList = new List<string>();
    public static List<float> playerScoreList = new List<float>();

    private void Start()
    {
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Ready()
    {
        ready.GetComponent<RectTransform>().DOAnchorPos(TargetObj.anchoredPosition, 0.5f).SetEase(Ease.Linear);
    }
    public void Back()
    {
        ready.GetComponent<RectTransform>().DOAnchorPos(new Vector2(2584,0), 0.5f).SetEase(Ease.Linear);
    }
    public void ReadyNext()
    {
        ready.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero,0.5f).SetEase(Ease.Linear);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Next()
    {
        FinishPanel.GetComponent<RectTransform>().DOAnchorPos(TargetObj.anchoredPosition, 0.5f).SetEase(Ease.Linear);
    }
    public void Startg()
    {
        PlayerCount = bodySourceView.PlayertrackIds.Count;
        for(int i=0;i<PlayerCount;i++)
        {
            FinishplayerNames[i].text = playerNameTags[i].text;
        }
        for (int i = 0; i < PlayerCount; i++)
        {
            FinishplayerNames2[i].text = playerNameTags[i].text;
        }
        for (int i=0;i<PlayerCount;i++)
        {
            playerList.Add(playerNameTags[i].text);
        }
        ready.SetActive(false);
        
        animatorSanta.enabled = true;
        animatorIpi.enabled = true;
    }
    private void Update()
    {
        if (ready.active == true)
            onReady = true;
        else
        onReady = false;

        if (onReady)
        {
            for(int i=0;i<playerNameTags.Length;i++)
            {
                playerNames[i].text = playerNameTags[i].text;
            }
            switch (bodySourceView.PlayertrackIds.Count)
            {
                case 1:
                    PlayerCount = 1;
                    P1Nametag.SetActive(true);
                    P2Nametag.SetActive(false);
                    P3Nametag.SetActive(false);
                    P1Nametag.GetComponent<RectTransform>().anchoredPosition = TagTransform[0].anchoredPosition;
                    PlayerNames[0].SetActive(true);
                    PlayerNames[1].SetActive(false);
                    PlayerNames[2].SetActive(false);
                    Sliders[0].SetActive(true);
                    Sliders[1].SetActive(false);
                    Sliders[2].SetActive(false);
                    break;
                case 2:
                    PlayerCount = 2;
                    P1Nametag.SetActive(true);
                    P2Nametag.SetActive(true);
                    P3Nametag.SetActive(false);
                    P1Nametag.GetComponent<RectTransform>().anchoredPosition = TagTransform[1].anchoredPosition;
                    P2Nametag.GetComponent<RectTransform>().anchoredPosition = TagTransform[2].anchoredPosition;
                    for (int i = 0; i < PlayerCount; i++)
                    {
                        PlayerNames[i].SetActive(true);
                    }
                    PlayerNames[2].SetActive(false);
                    Sliders[0].SetActive(true);
                    Sliders[1].SetActive(true);
                    Sliders[2].SetActive(false);
                    break;
                case 3:
                    PlayerCount = 3;
                    P1Nametag.GetComponent<RectTransform>().anchoredPosition = TagTransform[1].anchoredPosition;
                    P2Nametag.GetComponent<RectTransform>().anchoredPosition = TagTransform[0].anchoredPosition;
                    P3Nametag.GetComponent<RectTransform>().anchoredPosition = TagTransform[2].anchoredPosition;
                    P1Nametag.SetActive(true);
                    P2Nametag.SetActive(true);
                    P3Nametag.SetActive(true);
                    for (int i = 0; i < PlayerCount; i++)
                    {
                        PlayerNames[i].SetActive(true);
                    }
                    Sliders[0].SetActive(true);
                    Sliders[1].SetActive(true);
                    Sliders[2].SetActive(true);
                    break;

                default:
                    PlayerCount = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        PlayerNames[i].SetActive(false);
                    }
                    P1Nametag.SetActive(false);
                    P2Nametag.SetActive(false);
                    P3Nametag.SetActive(false);
                    Sliders[0].SetActive(false);
                    Sliders[1].SetActive(false);
                    Sliders[2].SetActive(false);
                    break;
            }
        }
        if(onReady)
        {
            if (bodySourceView.PlayertrackIds.Count > 0)
            {
                if (playerNameTags[0].text == null && playerNameTags[1].text == null && playerNameTags[2].text == null)
                    StartButton.SetActive(false);
                else
                    StartButton.SetActive(true);

                NextButton.SetActive(true);

            }else
            {
                StartButton.SetActive(false);
                NextButton.SetActive(false);
            }
        }
    }
}
