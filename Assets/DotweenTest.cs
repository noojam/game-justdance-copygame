using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DotweenTest : MonoBehaviour
{
    public RectTransform TargetObjPrefab;
    private RectTransform rectTransform;
    public Sprite sprite;
    void Start()
    {
        
    }
    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        gameObject.GetComponent<Image>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if ((rectTransform.anchoredPosition - TargetObjPrefab.anchoredPosition).magnitude <= 0.1f )
            Fade();
    }
    void Fade()
    {
        rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f),1);
        gameObject.GetComponent<Image>().DOFade(0,1);
        Invoke("DestoryObj",1);
    }
    void DestoryObj()
    {
        Destroy(gameObject);
    }
}
