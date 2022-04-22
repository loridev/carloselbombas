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
        string url = "http://localhost:8000/api/v1/auth/login";

        HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        JObject json = new JObject(
                new JProperty("name", name),
                new JProperty("password", password)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(url, body);

        User responseUser = null;

        if (response.IsSuccessStatusCode)
        {

            JObject userJson = JObject.Parse(JObject.Parse(await response.Content.ReadAsStringAsync())["user"].ToString());

            Globals.Token = (string)JObject.Parse(await response.Content.ReadAsStringAsync())["token"];
            responseUser = JsonConvert.DeserializeObject<User>(userJson.ToString());
        }

        return responseUser;

    }

    public static async Task<bool> Logout(string token)
    {
        string url = "http://localhost:8000/api/v1/auth/logout";

        HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        HttpResponseMessage response = await client.PostAsync(url, new StringContent("", Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            Globals.CurrentUser = null;
            Globals.Token = null;
        }

        return response.IsSuccessStatusCode;
    }

    public static async Task<Item[]> GetShop()
    {
        string url = "http://localhost:8000/api/v1/items/";

        HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync(url);

        Item[] responseArray = null;

        if (response.IsSuccessStatusCode)
        {
            responseArray = JsonConvert.DeserializeObject<Item[]>(response.Content.ReadAsStringAsync().Result);
        }

        return responseArray;
    }

    public static async Task<bool> BuyItem(int id, string token)
    {
        string url = "http://localhost:8000/api/v1/users/additem";

        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        JObject json = new JObject(
            new JProperty("item_id", id)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await client.PostAsync(url, body);

        return responseMessage.IsSuccessStatusCode;
    }

    public static async Task<bool> GetEquipped(string token)
    {
        string url = "http://localhost:8000/api/v1/users/equipped";
        
        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        HttpResponseMessage response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            Globals.CurrentUser.equippedItems = JsonConvert
                .DeserializeObject<List<Item>>(response.Content.ReadAsStringAsync().Result);
        }

        return response.IsSuccessStatusCode;
    }

    public static async Task<bool> ToggleEquipped(string token, Item item)
    {
        string url = "http://localhost:8000/api/v1/users/toggle_equipped";

        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        JObject json = new JObject(
            new JProperty("item_id", item.id)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await client.PostAsync(url, body);

        if (responseMessage.IsSuccessStatusCode)
        {
            if (Globals.CurrentUser.IsEquipped(item))
            {
                Globals.CurrentUser.equippedItems.Remove(item);
            }
            else
            {
                Item sameTypeItem = Globals.CurrentUser.equippedItems.Find((itemFind) => item.type == itemFind.type);
                if (sameTypeItem != null)
                {
                    Globals.CurrentUser.equippedItems.Remove(sameTypeItem);
                }
                Globals.CurrentUser.equippedItems.Add(item);
            }
        }

        return responseMessage.IsSuccessStatusCode;
    }

    public static async Task<bool> SetCharacter(string token, string character)
    {
        string url = "http://localhost:8000/api/v1/users/set_character";

        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        JObject json = new JObject(
            new JProperty("character", character)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await client.PostAsync(url, body);

        if (responseMessage.IsSuccessStatusCode)
        {
            Globals.CurrentUser.character = character;
        }

        return responseMessage.IsSuccessStatusCode;
    }
}
