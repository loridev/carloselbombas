using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class User : IComparable
{
    public int id;
    public string name;
    public string email;
    public bool is_admin;
    public string indiv_level;
    public int multi_wins;
    public int money;
    public string character;
    public List<Item> items;
    public List<Item> equippedItems;

    [JsonConstructor]
    public User(int id, string name, string email, bool is_admin, string indiv_level, int multi_wins, int money, string character, List<Item> items)
    {
        this.id = id;
        this.name = name;
        this.email = email;
        this.is_admin = is_admin;
        this.indiv_level = indiv_level;
        this.multi_wins = multi_wins;
        this.money = money;
        this.character = character;
        this.items = items;
    }

    public User(string name)
    {
        this.name = name;
    }

    public User(string name, int multiWins)
    {
        this.name = name;
        multi_wins = multiWins;
    }

    public int GetWorldNum()
    {
        return Int32.Parse(indiv_level.Split('-')[0]);
    }

    public int GetLevelNum()
    {
        return Int32.Parse(indiv_level.Split('-')[1]);
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public bool IsEquipped(Item item)
    {
        return equippedItems.Contains(item);
    }
    
    public override bool Equals(object obj)
    {
        User parsedObj = (User) obj;

        return parsedObj != null && parsedObj.id == id;
    }
    
    public override int GetHashCode()
    {
        return id;
    }

    public int CompareTo(object obj)
    {
        User userCompare = obj as User;

        if (userCompare != null)
        {
            return userCompare.multi_wins.CompareTo(multi_wins);
        }

        return 1;
    }
}
