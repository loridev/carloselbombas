using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string category;
    public int price;
    public string type;
    public string skinTexture;
    public List<User> users;

    public Item(string category, int price, string type, string skinTexture, List<User> users)
    {
        this.category = category;
        this.price = price;
        this.type = type;
        this.skinTexture = skinTexture;
        this.users = users;
    }
}
