using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameGui : MonoBehaviour {
	
	
	protected GameObject _observer;
	protected GameObject _owner;
	
	public void SetOwner(GameObject owner)
	{
		_owner = owner;
	}
	
	
	public virtual void SetHealthValue(float value)
	{
		throw new NotImplementedException();
	}

	public virtual void SetWeaponInfo(int weaponId, string weaponName, string ammo)
	{
		throw new NotImplementedException();
	}
}
