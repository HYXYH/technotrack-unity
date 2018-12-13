using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGui : InGameGui {
	
	[SerializeField]
	private Image _healthBar;
	[SerializeField]
	private Text _healthBarText;
	[SerializeField]
	private Text _weaponName;	
	[SerializeField]
	private Text _weaponAmmo;
	
	
	public override void SetHealthValue(float value)
	{
		_healthBarText.text = (int)(value * 100) + "%";
		_healthBar.color = Color.Lerp(Color.red, Color.green, value);
		_healthBar.fillAmount = value;
	}


	public override void SetWeaponInfo(int weaponId, string weaponName, string ammo)
	{
		_weaponName.text = weaponName;
		_weaponAmmo.text = ammo;

	}
}
