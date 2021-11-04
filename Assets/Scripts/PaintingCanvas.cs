using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEditor;

public class PaintingCanvas : MonoBehaviour
{
    public string imgPath;

    void Start()
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(File.ReadAllBytes(imgPath));
        GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
        AssetDatabase.CreateAsset(tex, imgPath + "_");
    }
}
