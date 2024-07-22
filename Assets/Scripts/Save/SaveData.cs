using System;
using UnityEngine;

public static class SaveData
{ 
    public static bool Has(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void AddInt(string key, int value)
    {
        SetInt(key, GetInt(key) + value);
    }
    public static void AddInt(string key, int value, int defaultValue)
    {
        SetInt(key, GetInt(key,defaultValue) + value);
    }
    public static void RemoveInt(string key, int value)
    {
        AddInt(key, -value);
    }

    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }
    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key,defaultValue);
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key)
    {
        var value = PlayerPrefs.GetInt(key) == 1;
        return value;
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static int TryGetOrDefault(string key)
    {
        if (Has(key))
            return GetInt(key);

        return 0;
    }

    public static bool TryGet(string key, out int value)
    {
        value = 0;

        if (Has(key))
        {
            value = GetInt(key);
            return true;
        }

        return false;
    }
   
}
