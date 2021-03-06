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
using DB.Models;

public static class ApiRequests
{
    public static async Task<Level> GetLevel(int worldNum, int levelNum)
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/levels/" + worldNum + "-" + levelNum;

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

    public static async Task<User> Login(string name, string password, bool dividida)
    {
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

        if (response.IsSuccessStatusCode)
        {

            JObject userJson = JObject.Parse(JObject.Parse(await response.Content.ReadAsStringAsync())["user"].ToString());

            if (!dividida)
            {
                Globals.Token = (string)JObject.Parse(await response.Content.ReadAsStringAsync())["token"];
            }
            else
            {
                Globals.Token2 = (string)JObject.Parse(await response.Content.ReadAsStringAsync())["token"];
            }
            responseUser = JsonConvert.DeserializeObject<User>(userJson.ToString());
        }

        return responseUser;

    }

    public static async Task<bool> Logout(string token)
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/auth/logout";

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
        string url = "https://caboomgame.herokuapp.com/api/v1/items/";

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
        string url = "https://caboomgame.herokuapp.com/api/v1/users/additem";

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
        string url = "https://caboomgame.herokuapp.com/api/v1/users/equipped";
        
        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

        HttpResponseMessage response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            if (Globals.Token == token)
            {
                Globals.CurrentUser.equippedItems = JsonConvert
                    .DeserializeObject<List<Item>>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Globals.Player2.equippedItems = JsonConvert
                    .DeserializeObject<List<Item>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        return response.IsSuccessStatusCode;
    }

    public static async Task<bool> ToggleEquipped(string token, Item item)
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/users/toggle_equipped";

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
        string url = "https://caboomgame.herokuapp.com/api/v1/users/set_character";

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

    public static async Task<bool> AddIndivRanking(User user, int worldNum, int levelNum, int time)
    {
        if (levelNum == 5) levelNum = 4;
        string url = "https://caboomgame.herokuapp.com/api/v1/rankings";

        HttpClient client = new HttpClient();

        JObject json = new JObject(
            new JProperty("user_id", user.id),
            new JProperty("world_num", worldNum),
            new JProperty("level_num", levelNum),
            new JProperty("time", time)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        
        HttpResponseMessage responseMessage = await client.PostAsync(url, body);

        return responseMessage.IsSuccessStatusCode;
    }

    public static async Task<bool> SaveProgress(User user, string indivLevel)
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/users/" + user.id;

        HttpClient client = new HttpClient();

        int worldCoins = 0;

        switch (Globals.WorldNum)
        {
            case 1:
                worldCoins = 5;
                break;
            case 2:
                worldCoins = 15;
                break;
            case 3:
                worldCoins = 30;
                break;
        }

        JObject json = new JObject(
            new JProperty("indiv_level", indivLevel),
            new JProperty("money", user.money + worldCoins * Globals.LevelNum * 5)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await client.PutAsync(url, body);

        if (responseMessage.IsSuccessStatusCode)
        {
            user.money += worldCoins * Globals.LevelNum * 5;
        }

        return responseMessage.IsSuccessStatusCode;
    }

    public static async Task<bool> SaveMultiProgress(User winner, User loser, int wins, int roundDiff)
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/users/" + winner.id;

        HttpClient client = new HttpClient();

        int rewardCoins = 0;
        int multiplier = loser.multi_wins >= (winner.multi_wins - 1) ? loser.multi_wins - (winner.multi_wins - 1) : 1;

        switch (roundDiff)
        {
            case 1:
                rewardCoins = 5 * multiplier;
                break;
            case 2:
                rewardCoins = 15 * multiplier;
                break;
            case 3:
                rewardCoins = 30 * multiplier;
                break;
        }

        JObject json = new JObject(
            new JProperty("multi_wins", wins),
            new JProperty("money", winner.money + rewardCoins)
        );

        StringContent body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

        HttpResponseMessage responseMessage = await client.PutAsync(url, body);

        if (responseMessage.IsSuccessStatusCode)
        {
            winner.money += rewardCoins;
        }
        return responseMessage.IsSuccessStatusCode;
    }

    public static async Task<List<Ranking>> GetRankingsIndiv()
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/rankings/single/all";

        HttpClient client = new HttpClient();

        HttpResponseMessage responseMessage = await client.GetAsync(url);

        List<Ranking> rtrn = null;

        if (responseMessage.IsSuccessStatusCode)
        {
            rtrn = JsonConvert.DeserializeObject<List<Ranking>>(responseMessage.Content.ReadAsStringAsync().Result);
        }

        return rtrn;
    }
    
    public static async Task<List<User>> GetRankingsMult()
    {
        string url = "https://caboomgame.herokuapp.com/api/v1/rankings/multi";

        HttpClient client = new HttpClient();

        HttpResponseMessage responseMessage = await client.GetAsync(url);

        List<User> rtrn = null;

        if (responseMessage.IsSuccessStatusCode)
        {
            JToken usersJson = JObject.Parse(await responseMessage.Content.ReadAsStringAsync())["rankings"];
            rtrn = JsonConvert.DeserializeObject<List<User>>(usersJson.ToString());
        }

        return rtrn;
    }
}
