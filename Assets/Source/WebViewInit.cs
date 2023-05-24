using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewInit : MonoBehaviour
{
    [SerializeField] private UniWebView _webView;
    private static string Link = "url";

    private void Start()
    {
        OpenLink(PlayerPrefs.GetString(Link));

     }
    private void OnEnable()
    {
        _webView.OnOrientationChanged += (view, orientation) =>
        {
            _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        };
    }

    public void OpenLink(string link)
    {
        _webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        _webView.Load(link);
        _webView.Show();
    }


    
}
