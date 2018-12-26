using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PoolManager
{
    private static PoolData[] _poolsData;

    [System.Serializable]
    public struct PoolData
    {
        public string PoolName; 
        public PoolObject Prefab; 
        public int Count; 
        public Pool pool; //сам пул
        public GameObject PoolContainerObject;
    }


    public static void Initialize(PoolData[] newPools)
    {
        _poolsData = newPools; 
        var objectsParent = new GameObject();
        objectsParent.name = "DefaultPoolContainer"; 
        for (var i = 0; i < _poolsData.Length; i++)
        {
            if (_poolsData[i].Prefab != null)
            {
                _poolsData[i].pool = new Pool(); 
                if (_poolsData[i].PoolContainerObject == null)
                {
                    _poolsData[i].PoolContainerObject = objectsParent;
                }
                _poolsData[i].pool.Initialize(_poolsData[i].Count, _poolsData[i].Prefab, _poolsData[i].PoolContainerObject.transform);
            }
        }
    }
    

    public static GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject result = null;
        if (_poolsData != null)
        {
            for (int i = 0; i < _poolsData.Length; i++)
            {
                if (String.CompareOrdinal(_poolsData[i].PoolName, name) == 0)
                {
                    result = _poolsData[i].pool.GetObject().gameObject;
                    result.transform.position = position;
                    result.transform.rotation = rotation;
                    result.SetActive(true);
                    return result;
                }
            }
        }

        return result; 
    }
    
    public static GameObject GetObject(GameObject sample, Vector3 position, Quaternion rotation)
    {
        //todo: создавать pool если не нашли готовый
        return GetObject(sample.name, position, rotation);
    }
}