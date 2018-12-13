using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotGui : InGameGui {

	[SerializeField]
	private GameObject _healthBg;
	[SerializeField]
	private Image _healthBar;
	[SerializeField]
	private Text _healthBarText;
	[SerializeField]
	private float _maxGuiDistance = 50;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{	
		//todo: use observer camera
		var observingAngle = Vector3.Angle(Camera.main.transform.forward,
			_owner.transform.position - Camera.main.transform.position);
		if (observingAngle < 90)
		{
			_healthBg.SetActive(true);
			Vector2 screenPoint =
				RectTransformUtility.WorldToScreenPoint(Camera.main, _owner.transform.position + _owner.transform.up * 4);
			_healthBg.transform.position = screenPoint;
			var dist = (_owner.transform.position - _observer.transform.position).magnitude;
			var scale = Mathf.Max(1 - dist / _maxGuiDistance, 0);
			_healthBg.transform.localScale = new Vector3(scale, scale, scale);
		}
		else
		{
			_healthBg.SetActive(false);
		}
	}

	public override void SetWeaponInfo(int weaponId, string weaponName, string ammo)
	{
		
	}

	public override void SetHealthValue(float value)
	{
		_healthBarText.text = (int)(value * 100) + "%";
		_healthBar.fillAmount = value;
	}

	void OnEnable()
	{
		_healthBar.fillAmount = 1;
		_observer = GameObject.FindWithTag("Player");
	}
}
