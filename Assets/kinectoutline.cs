using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kinectoutline : MonoBehaviour
{
    public Texture2D depthTexture;
    public Material lineMaterial;

    private void OnGUI()
    {
        if (depthTexture != null && lineMaterial != null)
        {

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), depthTexture);


            DrawContour();
        }
    }

    private void DrawContour()
    {
        if (depthTexture != null)
        {
            RenderTexture tempRT = RenderTexture.GetTemporary(depthTexture.width, depthTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(depthTexture, tempRT);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tempRT;

            Texture2D contourTexture = new Texture2D(tempRT.width, tempRT.height);
            contourTexture.filterMode = FilterMode.Point;

         
            for (int y = 0; y < tempRT.height; y++)
            {
                for (int x = 0; x < tempRT.width; x++)
                {
                    if (x == tempRT.width / 2 || y == tempRT.height / 2)
                    {
                        contourTexture.SetPixel(x, y, Color.red);
                    }
                    else
                    {
                        contourTexture.SetPixel(x, y, Color.clear);
                    }
                }
            }

            contourTexture.Apply();
            Graphics.Blit(contourTexture, tempRT, lineMaterial);
            RenderTexture.active = previous;

            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}
