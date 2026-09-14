using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg : MonoBehaviour
{
    public Animator animatorIpi;
    public Animator animatorSanta;
    public GameObject ready;
    private void Start()
    {
        Invoke("Startg",3);
    }
    public void Startg()
    {
        gameObject.SetActive(false);
       ready.SetActive(true);
    }
}
