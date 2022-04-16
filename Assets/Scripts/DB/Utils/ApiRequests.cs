using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

public class ApiRequests
{
    public static async Task<Level> GetLevel(int worldNum, int levelNum)
    {
        string url = "https://carloselbombas.herokuapp.com/api/v1/levels/" + worldNum + "-" + levelNum;

        HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.GetAsync(url);

        Level responseLevel = null;

        if (response.IsSuccessStatusCode)
        {
            responseLevel = JsonConvert.DeserializeObject<Level>(response.Content.ReadAsStringAsync().Result);

        }

        return responseLevel;
    }

    public static async Task<User> Login(string name, string password)
    {
        name = "holabuenas";
        password = "Erl37122";
        string url = "https://caboomgame.herokuapp.com/api/v1/auth/login";

        HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        JObject json = new JObject(
                new JProperty("name", name),
                new JProperty("password", password)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(url, body);

        User responseUser = null;

        Debug.Log(JObject.Parse(await response.Content.ReadAsStringAsync())["token"]);

        // TODO: CONSTRUCTOR DE USER RELLENITO DE LAS COSSASS ESTAS ROLLO LO DEL TOKEN;

        if (response.IsSuccessStatusCode)
        {
            responseUser = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
        }

        return responseUser;

    }
}
