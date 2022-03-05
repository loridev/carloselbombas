using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int worldNum;
    public int levelNum;
    public List<string[]> content;

    public Level(int worldNum, int levelNum, List<string[]> content)
    {
        this.worldNum = worldNum;
        this.levelNum = levelNum;
        this.content = content;
    }
}
