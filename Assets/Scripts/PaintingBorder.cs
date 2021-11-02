using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEditor;

public class PaintingBorder : MonoBehaviour
{
    public Transform canvas;

    public void ChangeCanvasTexture()
    {
        Material mat = canvas.GetComponent<Renderer>().material;
        string oldPath = AssetDatabase.GetAssetPath(mat.mainTexture);

        if (!oldPath.Contains("mustached"))
        {
            string[] split = oldPath.Split('.');
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(File.ReadAllBytes(split[0] + "_mustached.jpg"));
            AssetDatabase.CreateAsset(tex, split[0] + "_mustached.jpg_");
            mat.SetTexture("_MainTex", tex);
        }
        
    }
    
}
