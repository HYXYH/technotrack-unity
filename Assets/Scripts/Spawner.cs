using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private GameObject _botPrefab;
	[SerializeField] private GameObject[] _spawnPoints;

	private List<GameObject> _myCharacters;
	private BoxCollider _collider;
	
	
	// Use this for initialization
	void Start ()
	{
		_collider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(GameObject toSpawn)
	{
		StartCoroutine(timedSpawn(toSpawn));
	}
	
	
	public IEnumerator timedSpawn(GameObject toSpawn)
	{
		yield return new WaitForSeconds(5);
		gameObject.SetActive(true);
	}

}
