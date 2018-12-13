using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[AddComponentMenu("Pool/ObjectPooling")]
public class ObjectPooling
{

	#region Data

	List<PoolObject> objects;
	Transform objectsParent;

	#endregion
	void AddObject(PoolObject sample, Transform objects_parent) {
//		GameObject temp = GameObject.Instantiate(sample.gameObject);
		GameObject temp = NetworkManager.Instantiate(sample.gameObject);
		temp.name = sample.name;
		temp.transform.SetParent (objects_parent);
		objects.Add(temp.GetComponent<PoolObject> ());
//		if (temp.GetComponent<Animator> ())
//			temp.GetComponent<Animator> ().StartPlayback ();
		NetworkServer.Spawn(temp);
		temp.SetActive(false);
	}
	
	
	public void Initialize (int count, PoolObject sample, Transform objects_parent) {
		objects = new List<PoolObject> (); //инициализируем List
		objectsParent = objects_parent; //инициализируем локальную переменную для последующего использования
		for (int i=0; i<count; i++) {
			AddObject(sample, objects_parent); //создаем объекты до указанного количества
		}
	}
	
	public PoolObject GetObject () {
		for (int i=0; i<objects.Count; i++) {
			if (objects[i].gameObject.activeInHierarchy==false) {
				return objects[i];
			}
		}
		AddObject(objects[0], objectsParent);
		return objects[objects.Count-1];
	}


}