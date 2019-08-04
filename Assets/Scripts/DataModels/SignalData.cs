using System;
using UnityEngine;
using System.Collections.Generic;

public class SignalData
{
  Dictionary<string, System.Object> data = new Dictionary<string, System.Object>();


  public void set(string key, System.Object value)
  {
    if (!data.ContainsKey(key))
    {
      data.Add(key, value);
    }
    else
    {
      Debug.Log(String.Format("Cannot Add key {0} already exists", key));
    }
  }

  public T get<T>(string key)
  {
    if (data.ContainsKey(key) && data[key] is T)
    {
      return (T)data[key];
    }
    return default(T);
  }
  // public void setInt(string key, int value)
  // {
  //   if (!data.ContainsKey(key))
  //   {
  //     data.Add(key, value);
  //   }
  //   else
  //   {
  //     Debug.Log(String.Format("Cannot Add key {0} already exists", key));
  //   }
  // }

  // public int getInt(string key)
  // {
  //   if (data.ContainsKey(key) && data[key] is int)
  //   {
  //     return Convert.ToInt32(data[key]);
  //   }
  //   return 0;
  // }

  // public void setString(string key, string value)
  // {
  //   if (!data.ContainsKey(key))
  //   {
  //     data.Add(key, value);
  //   }
  //   else
  //   {
  //     Debug.Log(String.Format("Cannot Add key {0} already exists", key));
  //   }
  // }

  // public string getString(string key)
  // {
  //   if (data.ContainsKey(key) && data[key] is string)
  //   {
  //     return Convert.ToString(data[key]);
  //   }
  //   return null;
  // }

  // public void setFloat(string key, float value)
  // {
  //   if (!data.ContainsKey(key))
  //   {
  //     data.Add(key, value);
  //   }
  //   else
  //   {
  //     Debug.Log(String.Format("Cannot Add key {0} already exists", key));
  //   }
  // }

  // public float getFloat(string key)
  // {
  //   if (data.ContainsKey(key) && data[key] is float)
  //   {
  //     return Convert.ToSingle(data[key]);
  //   }
  //   return 0;
  // }

  // public void setObject(string key, System.Object value)
  // {
  //   if (!data.ContainsKey(key))
  //   {
  //     data.Add(key, value);
  //   }
  //   else
  //   {
  //     Debug.Log(String.Format("Cannot Add key {0} already exists", key));
  //   }
  // }

  // public Object getFloat(string key)
  // {
  //   if (data.ContainsKey(key) && data[key] is float)
  //   {
  //     return Convert.ToSingle(data[key]);
  //   }
  //   return 0;
  // }
}