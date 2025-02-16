using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Helper
{
    public Texture2D ConvertRenderTextureToTexture2D(RenderTexture renderTexture)
    {
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new UnityEngine.Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        return texture;
    }
    public void CopyTextureRegion(Texture2D sourceTexture, Texture2D targetTexture, Vector4 region)
    {
        // // Calculate the pixel coordinates based on the UV region
        int startX = Mathf.FloorToInt(region.x * targetTexture.width);
        int startY = Mathf.FloorToInt(region.y * targetTexture.height);
        int width = Mathf.FloorToInt((region.z - region.x) * targetTexture.width);
        int height = Mathf.FloorToInt((region.w - region.y) * targetTexture.height);

        Color[] pixels = sourceTexture.GetPixels();
        // Debug.Log(startX+", "+startY+", "+width+", "+height);
        if(startX <= targetTexture.width){
            targetTexture.SetPixels(startX, startY, width, height, pixels);
            targetTexture.Apply();
        }
    }
    public Texture2D CreateTexture(Color color){
        int width = (int)(1920*6.25);
        int height = (int)(1080*1.6);
        Debug.Log(width +", "+ height);
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    public bool CloneTexture(RenderTexture renderTexture, Material material){
        try
        {
            Texture2D currentTexture = ConvertRenderTextureToTexture2D(renderTexture); 
            Vector4 region = material.GetVector("_Region");
            Texture2D backgroundTex = (Texture2D)material.GetTexture("_BackgroundTex");
            CopyTextureRegion(currentTexture, backgroundTex, region);
            material.SetTexture("_BackgroundTex", backgroundTex);    
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to clone texture: " + ex.Message);
            return false;
        }
        
    }
}
