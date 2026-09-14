using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    void Start()
    {
        
    }
    public void Next()
    {
        mainMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-3550, 378), 0.5f).SetEase(Ease.Linear);
    }
    public void Back()
    {
        mainMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-888, 355), 0.5f).SetEase(Ease.Linear);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Rasputin()
    {
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
