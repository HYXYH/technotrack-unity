using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectLifetime : MonoBehaviour
{

	[SerializeField] public float Lifetime;

	private float _endTime;
	private PoolObject _poolObject;

	
	// Use this for initialization
	void Start ()
	{
		_poolObject = GetComponent<PoolObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > _endTime)
		{
			_poolObject.ReturnToPool();
		}
	}
	
	void OnEnable()
	{
		_endTime = Time.time + Lifetime;
	}
}
