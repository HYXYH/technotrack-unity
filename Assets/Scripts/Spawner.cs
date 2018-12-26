using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	private Queue<GameObject> _queue = new Queue<GameObject>();
	private Queue<float> _timeQueue = new Queue<float>();
	private GameObject _currentObj = null;
	private float _currentSpawnTime;

	public void Spawn(GameObject toSpawn)
	{
		_queue.Enqueue(toSpawn);
		_timeQueue.Enqueue(Time.time + 2);
	}

	private void Update()
	{
		if (_currentObj == null && _queue.Count > 0)
		{
			_currentObj = _queue.Dequeue();
			_currentSpawnTime = _timeQueue.Dequeue();
		}

		if (_currentSpawnTime < Time.time && _currentObj != null)
		{
			_currentObj.SetActive(true);
			_currentObj = null;
		}
	}


	public IEnumerator timedSpawn(GameObject toSpawn)
	{
		yield return new WaitForSeconds(7);
		gameObject.SetActive(true);
	}

}
