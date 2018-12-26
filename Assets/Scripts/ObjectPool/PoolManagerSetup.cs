using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolManagerSetup : MonoBehaviour {
	
	[SerializeField] private PoolManager.PoolData[] _pools;  

	void OnValidate() 
	{
		for (int i = 0; i < _pools.Length; i++) 
		{
			_pools[i].PoolName = _pools[i].Prefab.name; 
		}
	}

	void Awake() 
	{
		Initialize ();
	}

	void Initialize () 
	{
		PoolManager.Initialize(_pools); 
	}

}