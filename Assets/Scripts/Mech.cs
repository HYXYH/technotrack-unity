using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using Vector3 = UnityEngine.Vector3;

public class Mech : MonoBehaviour, IMech
{

	[SerializeField] private float _walkSpeed;
	[SerializeField] private float _rotationSpeed;
	[SerializeField] private float _turretRotationSpeed;
	[SerializeField] private float _maxHealth = 100;
	[SerializeField] private float _currentHealth = 100;

	[SerializeField] private Transform _turret;
	[SerializeField] private GameObject _destroyedMechPrefab;

	[SerializeField] private WeaponHolder[] _weaponHolders;

	private Animator _animator;
	private Rigidbody _rigidbody;
	private NavMeshAgent _agent;
	private InGameGui _inGameGui;

	public bool UseNavMesh = false;


	// Use this for initialization
	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_animator = GetComponentInChildren<Animator>();
		_agent = gameObject.GetComponent<NavMeshAgent>();

		foreach (var weaponHolder in _weaponHolders)
		{
			var barrel = weaponHolder.GetBarrel();
			if (barrel != null)
			{
				foreach (var weapon in weaponHolder.WeaponPrefabs)
				{
					var created = Instantiate(weapon, barrel.transform.position, barrel.transform.rotation);
					created.transform.SetParent(barrel.transform);
				}

				weaponHolder.SetWeapons(barrel.GetComponentsInChildren<Weapon>());
				weaponHolder.InitWeapons(gameObject.GetInstanceID().ToString());
			}
		}
	}

	// Update is called once per frame
	private void Update()
	{
		if (UseNavMesh)
		{
			_animator.SetFloat("speed", _agent.velocity.magnitude);
		}
	}


	public void MoveToPoint(Vector3 targetPoint)
	{
		_agent.destination = targetPoint;

	}

	public bool CheckReachedTarget()
	{
		return _agent.remainingDistance < 1 && !_agent.pathPending;

	}

	public void MoveForward(float controllerValue)
	{
		_rigidbody.MovePosition(transform.position +
		                        transform.forward * controllerValue * _walkSpeed * Time.deltaTime);
		_animator.SetFloat("speed", controllerValue);

	}

	public void RotateBot(float controllerValue)
	{
		this.transform.Rotate(this.transform.up, controllerValue * Time.deltaTime * _rotationSpeed);
	}

	public void LookAt(Vector3 targetDir)
	{
		Quaternion rotation = Quaternion.LookRotation(targetDir, _turret.transform.up);
		_turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, rotation,
			Time.deltaTime * _turretRotationSpeed);
	}

	public Vector3 GetLookDirection()
	{
		return _turret.transform.forward;
	}

	public void Fire(int holderId)
	{
		if (holderId < _weaponHolders.Length)
		{
			_weaponHolders[holderId].Fire();
			_weaponHolders[holderId].GetWeapon();
			var weapon = _weaponHolders[holderId].GetWeapon();
			_inGameGui.SetWeaponInfo(holderId, weapon.GetName(), weapon.GetAmmoInfo());
		}
	}

	public void UseWeapon(int holderId, int weaponId)
	{
		var holder = _weaponHolders[holderId];
		holder.SetWeaponId(weaponId);
		var weapon = holder.GetWeapon();
		_inGameGui.SetWeaponInfo(weaponId, weapon.GetName(), weapon.GetAmmoInfo());
	}

	public void UseWeapon(int weaponId)
	{
		for (var i=0; i < _weaponHolders.Length;  i++)
		{
			UseWeapon(i, weaponId);
		}
	}

	public void ReceiveDamage(float damage)
	{
		_currentHealth -= damage;
		_inGameGui.SetHealthValue(_currentHealth / _maxHealth);

		if (_currentHealth <= 0)
		{
			if (_destroyedMechPrefab != null)
			{
				PoolManager.GetObject(_destroyedMechPrefab.name, transform.position + new Vector3(0, 1, 0),
					transform.rotation);
				_inGameGui.gameObject.SetActive(false);
				_inGameGui = null;
				if (!UseNavMesh)
				{
					Camera.main.transform.parent = null;
				}

				_currentHealth = _maxHealth;
				gameObject.SetActive(false);
			}
		}
	}

	public void SetGui(InGameGui inGameGui)
	{
		if (_inGameGui != null)
		{
			_inGameGui.GetComponent<PoolObject>().ReturnToPool();
		}

		_inGameGui = inGameGui;
		_inGameGui.SetHealthValue(_currentHealth / _maxHealth);
	}

}