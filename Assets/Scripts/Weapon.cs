using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon: MonoBehaviour
{

	[SerializeField] 
	public string Name = "NONAME";
	[SerializeField]
	private float _cooldown;
	[SerializeField]
	private float _maxAmmo;
	[SerializeField]
	private float _currentAmmo;

	private float _lastShootTime;
	private Transform _barrelPosition;
	protected string _ownerName;


	public string GetName()
	{
		return Name;
	}

	public string GetAmmoInfo()
	{
		return _currentAmmo + " / " + _maxAmmo;
	}
	
	public void SetOwnerName(string ownerName)
	{
		_ownerName = ownerName;
	}


	public bool CheckWeaponReady()
	{
		if (_lastShootTime + _cooldown > Time.time)
		{
			return false;
		}
		
		if (_currentAmmo <= 0)
		{
			return false;
		}

		return true;
	}
	
	public virtual void Fire()
	{
		_lastShootTime = Time.time;
		_currentAmmo -= 1;
	}
}


