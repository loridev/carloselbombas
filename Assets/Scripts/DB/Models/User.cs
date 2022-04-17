using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class User
{
    public string name;
    public string email;
    public bool is_admin;
    public string indiv_level;
    public int multi_wins;
    public int money;
    public List<Item> items;

    public User(string name, string email, bool is_admin, string indiv_level, int multi_wins, int money, List<Item> items)
    {
        this.name = name;
        this.email = email;
        this.is_admin = is_admin;
        this.indiv_level = indiv_level;
        this.multi_wins = multi_wins;
        this.money = money;
        this.items = items;
    }

    public int GetWorldNum()
    {
        return Int32.Parse(this.indiv_level.Split('-')[0]);
    }

    public int GetLevelNum()
    {
        return Int32.Parse(this.indiv_level.Split('-')[1]);
    }
}
