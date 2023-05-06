using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static void Save<T>(string key, T saveData)
    {
        string jsonDataString = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(key, jsonDataString);
    }

    public static T load<T>(string key) where T : new()
    {
        if(PlayerPrefs.HasKey(key))
        {
            string loadedString = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(loadedString);
        }
        else
        {
            return new T();
        }
    }
    public static bool CheckData(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}

public class Forest{
    public void Create(float[] pos, int cellSize, System.Random random)
    {

    }
}

public class Homes
{
    public void Create(float[] pos, int cellSize, System.Random random)
    {

    }
}

