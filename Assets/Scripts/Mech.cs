using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;
using Vector3 = UnityEngine.Vector3;

public class Mech : MonoBehaviour
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
	void Start()
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
	void Update()
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

	public void Fire(int weaponId)
	{
		if (weaponId < _weaponHolders.Length)
		{
			_weaponHolders[weaponId].Fire();
			_weaponHolders[weaponId].GetWeapon();
			var weapon = _weaponHolders[weaponId].GetWeapon();
			_inGameGui.SetWeaponInfo(weaponId, weapon.GetName(), weapon.GetAmmoInfo());
		}
	}

	public void UseWeapon(int weaponId)
	{
		foreach (var holder in _weaponHolders)
		{
			holder.SetWeaponId(weaponId);
			var weapon = holder.GetWeapon();
			_inGameGui.SetWeaponInfo(weaponId, weapon.GetName(), weapon.GetAmmoInfo());
		}
	}

	private void ReceiveDamage(float damage)
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


	public IEnumerator DeleteDestroyed()
	{
		yield return new WaitForSeconds(10);
		Destroy(gameObject);
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

[System.Serializable]
public class WeaponHolder
{
	[SerializeField] 
	private GameObject _barrel;

	[SerializeField]
	public GameObject[] WeaponPrefabs;
	private Weapon[] _weapons;
	[SerializeField] 
	private int _weaponId = 0;

	public GameObject GetBarrel()
	{
		return _barrel;
	}

	public void InitWeapons(string ownerName)
	{
		foreach (var weapon in _weapons)
		{
			weapon.SetOwnerName(ownerName);
		}
	}

	public void SetWeapons(Weapon[] weapons)
	{
		_weapons = weapons;
	}

	public void SetWeaponId(int weaponId)
	{
		if (weaponId < _weapons.Length)
		{
			_weaponId = weaponId;
		}
		else
		{
			Debug.Log("Weapon not found!");
		}
	}

	public Weapon GetWeapon()
	{
		return _weapons[_weaponId];
	}
	

	public void Fire()
	{
		if (_weaponId == -1 || _weaponId >= _weapons.Length)
		{
			Debug.Log("Weapon not installed!");
			return;
		}
		_weapons[_weaponId].Fire();
	}
}