using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour
{
    public BodySourceView BodySource;
    public GameObject Prefab;
    public RectTransform CreatePos;
    public RectTransform TargetObj;
    public AudioSource AudioSource;
    public GameObject BlackBG;
    public GameObject Result;
    public Slider[] score;
    public Slider[] result;
    public TextMeshProUGUI[] texts;
    public GameObject Next;
    public GameManager gameManager;
    void Apply()
    {
        BodySource.apply();
    }
    void MusicPlay()
    {
        AudioSource.Play();
    }
    void CreateImage(Sprite spriteInput)
    {
        GameObject a = Instantiate(Prefab, CreatePos.anchoredPosition, Quaternion.identity, GameObject.Find("Canvas").transform);
        a.gameObject.transform.position = CreatePos.position; 
        a.GetComponent<Image>().sprite = spriteInput;
        a.GetComponent<DotweenTest>().TargetObjPrefab = TargetObj;
        a.GetComponent<Image>().rectTransform.DOAnchorPos(TargetObj.anchoredPosition, 2).SetEase(Ease.Linear);
    }
    void Darker()
    { 
        List<int> Ranks = new List<int>();
        
        for (int i=0;i<gameManager.PlayerCount;i++)
        {
            GameManager.playerScoreList.Add(BodySource.score[i]);
        }
        List<float> scores = GameManager.playerScoreList.OrderByDescending(x => x).ToList();
        for (int i=0; i<gameManager.PlayerCount;i++)
        {
            for(int j=0;j<GameManager.playerScoreList.Count;j++)
            {
                if (GameManager.playerScoreList[GameManager.playerScoreList.Count-gameManager.PlayerCount+i] == scores[j])
                {
                    gameManager.FinishplayerRanks[i].text = (j+1).ToString();
                }
            }
        }

        BlackBG.GetComponent<Image>().DOColor(Color.white,1);
        AudioSource.Stop();
        switch (gameManager.PlayerCount)
        {
            case 1:
                result[0].gameObject.SetActive(true);
                result[1].gameObject.SetActive(false);
                result[2].gameObject.SetActive(false);
                break;
            case 2:
                result[0].gameObject.SetActive(true);
                result[1].gameObject.SetActive(true);
                result[2].gameObject.SetActive(false);
                break;
            case 3:
                result[0].gameObject.SetActive(true);
                result[1].gameObject.SetActive(true);
                result[2].gameObject.SetActive(true);
                break;
        }
        Result.SetActive(true);
        Invoke("ResultMd",1);
    }
    void ResultMd()
    {
        switch (gameManager.PlayerCount)
        {
            case 1:
                StartCoroutine("Valuing1");
                break;
            case 2:
                StartCoroutine("Valuing1");
                StartCoroutine("Valuing2");
                break;
            case 3:
                StartCoroutine("Valuing1");
                StartCoroutine("Valuing2");
                StartCoroutine("Valuing3");
                break;
        }
       
    }
    IEnumerator Valuing1()
    {
        float time = 0;
        float targetValue = BodySource.score[0];
        while (time < 3)
        {
            time += Time.deltaTime;
            float progress = time / 3;
            result[0].value =Mathf.Lerp(0,targetValue,progress);
            texts[0].text = Mathf.Lerp(0, targetValue, progress).ToString();
            yield return null;
        }
        result[0].value = targetValue;
    }
    IEnumerator Valuing2()
    {
        float time = 0;
        float targetValue = BodySource.score[1];
        while (time < 3)
        {
            time += Time.deltaTime;
            float progress = time / 3;
            result[1].value = Mathf.Lerp(0, targetValue, progress);
            texts[1].text = Mathf.Lerp(0, targetValue, progress).ToString();
            yield return null;
        }
        result[1].value = targetValue;
    }
    IEnumerator Valuing3()
    {
        float time = 0;
        float targetValue = BodySource.score[2];
        while (time < 3)
        {
            time += Time.deltaTime;
            float progress = time / 3;
            result[2].value = Mathf.Lerp(0, targetValue, progress);
            texts[2].text = Mathf.Lerp(0, targetValue, progress).ToString();
            yield return null;
        }
        result[2].value = targetValue;
    }

}
