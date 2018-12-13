using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolSetup : NetworkBehaviour {//обертка для управления статическим классом PoolManager
	
	#region Unity scene settings
	[SerializeField] private PoolManager.PoolPart[] pools; //структуры, где пользователь задает префаб для использования в пуле и инициализируемое количество 
	#endregion

	#region Methods
	void OnValidate() {
		for (int i = 0; i < pools.Length; i++) {
			pools[i].name = pools[i].Prefab.name; //присваиваем имена заранее, до инициализации
		}
	}

	void Awake() {
		Initialize ();
	}

	void Initialize () {
		if (isClient)
		{
			return;
		}
		PoolManager.Initialize(pools); //инициализируем менеджер пулов
	}
	#endregion

}