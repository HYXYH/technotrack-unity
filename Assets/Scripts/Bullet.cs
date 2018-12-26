using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour {


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
	private float _deadTime;


	public void SetOwnerName(string ownerNme)
	{
		_ownerName = ownerNme;
	}

	protected void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_poolObject = GetComponent<PoolObject>();
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

		var hitName = other.gameObject.GetInstanceID().ToString();
		var canDamage = other.gameObject.CompareTag("Damagable");
		var notMe = !_ownerName.Equals(hitName);
		if (notMe && canDamage)
			{
				CreateExplosion();
				other.gameObject.SendMessage("ReceiveDamage", _damage);
				_rigidbody.velocity = Vector3.zero;
				_poolObject.ReturnToPool();
			}
	}
	
	
	private void OnCollisionEnter(Collision other)
	{

		var hitName = other.collider.gameObject.GetInstanceID().ToString();
		var canDamage = other.collider.gameObject.CompareTag("Damagable");
		var notMe = !_ownerName.Equals(hitName);
		if (notMe && canDamage)
			{
				CreateExplosion(other.contacts[0].point, Quaternion.LookRotation(other.contacts[0].normal));
				other.gameObject.SendMessage("ReceiveDamage", _damage);
				_rigidbody.velocity = Vector3.zero;
				_poolObject.ReturnToPool();
			}
	}

	private void OnEnable()
	{
		_deadTime = Time.time + _lifetime;
	}
}
