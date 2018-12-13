using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PoolManager
{
    private static PoolPart[] _pools;

    [System.Serializable]
    public struct PoolPart
    {
        public string name; //имя префаба
        public PoolObject Prefab; //сам префаб, как образец
        public int Count; //количество объектов при инициализации пула
        public ObjectPooling Ferula; //сам пул
        public GameObject PoolContainerObject;
    }


    public static void Initialize(PoolPart[] newPools)
    {
        _pools = newPools; //заполняем информацию
        var objectsParent = new GameObject();
        objectsParent.name = "DefaultPoolContainer"; //создаем на сцене объект Pool, чтобы не заслонять иерархию
        for (int i = 0; i < _pools.Length; i++)
        {
            if (_pools[i].Prefab != null)
            {
                _pools[i].Ferula = new ObjectPooling(); //создаем свой пул для каждого префаба
                if (_pools[i].PoolContainerObject == null)
                {
                    _pools[i].PoolContainerObject = objectsParent;
                }
                
                _pools[i].Ferula.Initialize(_pools[i].Count, _pools[i].Prefab, _pools[i].PoolContainerObject.transform);
//инициализируем пул заданным количество объектов
            }
        }
    }

    public static GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject result = null;
        if (_pools != null)
        {
            for (int i = 0; i < _pools.Length; i++)
            {
                if (String.CompareOrdinal(_pools[i].name, name) == 0)
                {
                    //если имя совпало с именем префаба пула
                    result = _pools[i].Ferula.GetObject().gameObject; //дергаем объект из пула
                    result.transform.position = position;
                    result.transform.rotation = rotation;
                    result.SetActive(true); //выставляем координаты и активируем
                    return result;
                }
            }
        }

        return result; //если такого объекта нет в пулах, вернет null
    }
    
    public static GameObject GetObject(GameObject sample, Vector3 position, Quaternion rotation)
    {
        //todo: создавать pool если не нашли готовый
        return GetObject(sample.name, position, rotation);
    }
}