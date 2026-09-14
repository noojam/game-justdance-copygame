using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kinectMonitor : MonoBehaviour
{
    public MultiSourceManager multiSourceManager;
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rawImage.texture = multiSourceManager.GetColorTexture();
    }
}
