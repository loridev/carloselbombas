using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DB.Models;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class LocationUtils
{
    public static IEnumerator GetUserCity()
    {
        string ip = new WebClient().DownloadString("https://api.ipify.org");
        string url = "https://ipapi.co/" + ip + "/json";
        Debug.Log(url);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            Globals.City = (string) JObject.Parse(webRequest.downloadHandler.text)["city"];
            
            Debug.Log(webRequest.downloadHandler.text);
        }
    }
}
