using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerPrepareManager : NetworkBehaviour
{
	[SerializeField]
	private GameObject[] _serverOnlyObjects;
	[SerializeField]
	private GameObject[] _clientOnlyObjects;

	// Use this for initialization
	void Start () {
		if (isServer)
		{
			foreach (var obj in _serverOnlyObjects)
			{
				obj.gameObject.SetActive(true);
			}
		}
		
		if (isClient)
		{
			foreach (var obj in _clientOnlyObjects)
			{
				obj.gameObject.SetActive(true);
			}
		}
		gameObject.SetActive(false);
	}

}
