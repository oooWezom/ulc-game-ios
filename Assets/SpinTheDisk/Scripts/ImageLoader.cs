using UnityEngine;
using System.Collections;

public class ImageLoader : MonoBehaviour {

    private Texture2D texture;
    public Material material;
    public Material materialHighlighted;
    private string url = "";

    public void LoadImage(string url) {
        this.url = url;
        texture = new Texture2D(960, 480);
        StartCoroutine("imageLoader");
    }

    IEnumerator imageLoader() {
        WWW http = new WWW(url);
        yield return http;
        http.LoadImageIntoTexture(texture);
        material.SetTexture("_MainTex", texture);
        if (materialHighlighted != null) {
            materialHighlighted.SetTexture("_MainTex", texture);
        }
    }
}
