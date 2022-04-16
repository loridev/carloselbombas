using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class User
{
    public string name;
    public string email;
    public bool isAdmin;
    public string indivLevel;
    public int multiWins;
    public int money;
    public List<Item> items;

    public User(string name, string email, bool isAdmin, string indivLevel, int multiWins, int money, List<Item> items)
    {
        this.name = name;
        this.email = email;
        this.isAdmin = isAdmin;
        this.indivLevel = indivLevel;
        this.multiWins = multiWins;
        this.money = money;
        this.items = items;
    }

    public int GetWorldNum()
    {
        return Int32.Parse(this.indivLevel.Split('-')[0]);
    }

    public int GetLevelNum()
    {
        return Int32.Parse(this.indivLevel.Split('-')[1]);
    }
}
