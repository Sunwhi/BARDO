using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class DataManager : Singleton<DataManager>
{
    private readonly Dictionary<string, Dictionary<int, object>> dataDics = new(); //Resources/Json으로부터 자동로드.

    #region Unity Life Cycles
    public void Init()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Json");
        foreach (var jsonFile in jsonFiles)
        {
            string fileName = jsonFile.name;
            string jsonData = jsonFile.text;

            var type = Type.GetType(fileName);
            var genericMethod = GetType().GetMethod("ConvertToDic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).MakeGenericMethod(type);
            genericMethod.Invoke(this, new object[] { jsonData });
        }
    }
    #endregion

    #region Main Methods
    /// <summary>
    /// 원하는 타입의 데이터를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">___Data.cs</typeparam>
    /// <param name="id">spread sheet의 id</param>
    public T GetObj<T>(int id)
    {
        string key = typeof(T).Name;
        if (dataDics.TryGetValue(key, out var objDic))
        {
            if (objDic.TryGetValue(id, out var obj))
            {
                return (T)obj;
            }
        }

        Debug.LogWarning($"{key}은 DataDics에 존재하지 않습니다.");
        return default;
    }
    #endregion

    #region Sub Methods
    private void ConvertToDic<T>(string jsonData) where T : IData
    {
        try
        {
            List<T> objList = JsonConvert.DeserializeObject<List<T>>(jsonData);
            var dic = new Dictionary<int, object>();
            foreach (var obj in objList)
            {
                dic[obj.Id] = obj;
            }

            dataDics[typeof(T).Name] = new Dictionary<int, object>(dic);
        }
        catch (Exception)
        {
            Debug.LogError($"JSON parsing error: {jsonData}");
        }
    }
    #endregion
}