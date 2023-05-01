using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string Name;
    public int Lvl;

    public PlayerInfo(string name)
    {
        this.Name = name;
        this.Lvl = 1;
    }
}
