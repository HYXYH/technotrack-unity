using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MechNet : NetworkBehaviour, IMech
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveForward(float controllerValue)
	{
		throw new System.NotImplementedException();
	}

	public void RotateBot(float controllerValue)
	{
		throw new System.NotImplementedException();
	}

	public void LookAt(Vector3 targetDir)
	{
		throw new System.NotImplementedException();
	}

	public Vector3 GetLookDirection()
	{
		throw new System.NotImplementedException();
	}

	public void Fire(int holderId)
	{
		throw new System.NotImplementedException();
	}

	public void UseWeapon(int holderId, int weaponId)
	{
		throw new System.NotImplementedException();
	}

	public void UseWeapon(int weaponId)
	{
		throw new System.NotImplementedException();
	}

	public void ReceiveDamage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void SetGui(InGameGui inGameGui)
	{
		throw new System.NotImplementedException();
	}
}
