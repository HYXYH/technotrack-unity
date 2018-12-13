using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {


	[SerializeField]
	private float _damage = 20;
	[SerializeField]
	private float _lifetime = 5;
	[SerializeField] 
	private GameObject _explosionPrefab;

    private GameObject _explosion = null;
	private string _ownerName;
	private Rigidbody _rigidbody;
	private PoolObject _poolObject;
	private MeshRenderer _meshRenderer;
	private float _deadTime;


	public void SetOwnerName(string ownerNme)
	{
		_ownerName = ownerNme;
	}

	protected void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_poolObject = GetComponent<PoolObject>();
		_meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		if (_deadTime < Time.time)
		{
			CreateExplosion();
			_rigidbody.velocity = Vector3.zero;
			_poolObject.ReturnToPool();
		}
	}

	private void CreateExplosion(Vector3 position, Quaternion rotation)
	{
		if (_explosionPrefab != null)
		{
			_explosion = PoolManager.GetObject(_explosionPrefab.name, transform.position,
				transform.rotation);
			if (_explosion != null)
			{
				_explosion.GetComponent<ParticleSystem>().Play();
			}
			else
			{
				Debug.Log("_explosionPrefabName not found!");
			}
		}
	}

	private void CreateExplosion()
	{
		CreateExplosion(transform.position, transform.rotation);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isServer)
		{
			var networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
			if (networkIdentity == null || !_ownerName.Equals(networkIdentity.netId.ToString()))
			{
				Debug.Log(other.gameObject.tag);

				CreateExplosion();
				other.gameObject.SendMessage("ReceiveDamage", _damage);
				_rigidbody.velocity = Vector3.zero;
				_poolObject.ReturnToPool();
			}
		}
	}
	
	
	private void OnCollisionEnter(Collision other)
	{
		if (isServer)
		{
			var networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
			if (networkIdentity != null || !_ownerName.Equals(networkIdentity.netId.ToString()))
			{
				Debug.Log(other.gameObject.tag);

				CreateExplosion(other.contacts[0].point, Quaternion.LookRotation(other.contacts[0].normal));
				other.gameObject.SendMessage("ReceiveDamage", _damage);
				_rigidbody.velocity = Vector3.zero;
				_poolObject.ReturnToPool();
			}
		}
	}

	private void OnEnable()
	{
		_deadTime = Time.time + _lifetime;
	}
}
