using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string category;
    public int price;
    public string type;
    public string skin_texture;

    public Item(int id, string category, int price, string type, string skin_texture)
    {
        this.id = id;
        this.category = category;
        this.price = price;
        this.type = type;
        this.skin_texture = skin_texture;
    }
}
