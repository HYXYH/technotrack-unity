using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Mech))]
public class MechPlayerController : NetworkBehaviour
{
	
	[SerializeField]
	private GameObject _controlsGuiPrefab;
	[SerializeField]
	private GameObject _appearanceGuiPrefab;
	[SerializeField]
	private GameObject _menuPrefab;

	[SerializeField]
	private GameObject _cameraPosition;

	private Mech _mech;
	private Camera _camera;
	private GameObject _menuInstance;

	// Use this for initialization
	void Start ()
	{
		_mech = GetComponent<Mech>();
		_camera = Camera.main;
		if (isLocalPlayer)
		{
//			_camera.transform.parent = _cameraPosition.transform;
//			_camera.transform.localPosition = Vector3.zero;
//			_camera.transform.localRotation = Quaternion.identity;
			
			_menuInstance = Instantiate(_menuPrefab);
			var menuComponent = _menuInstance.GetComponent<PlayerMenu>();
			menuComponent.SetPlayer(gameObject);
			_menuInstance.transform.parent = GameObject.FindWithTag("Canvas").transform;
			_menuInstance.transform.localPosition = _menuPrefab.transform.position;
			gameObject.SetActive(false);
		}

	}
	
	// Update is called once per frame
	private void Update () {
		if (!isLocalPlayer)
		{
			return;
		}
		
		if (Input.GetAxis("Fire1") > 0)
		{
			_mech.Fire(0);
			_mech.Fire(1);
		}

		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			_mech.UseWeapon(0);
		}
		if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			_mech.UseWeapon(1);
		}
		if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			_mech.UseWeapon(2);
		}
	}
	
	void FixedUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		_mech.MoveForward(Input.GetAxis("Vertical"));
		_mech.RotateBot(Input.GetAxis("Horizontal"));
		
		var ray = _camera.ScreenPointToRay(Input.mousePosition);
		var newDir = Vector3.ProjectOnPlane(ray.direction, transform.up).normalized;
		
		_mech.LookAt(newDir);
	}

	private IEnumerator ResetGui()
	{
		yield return null;
		InGameGui gui;
		if (!isLocalPlayer)
		{
			gui = PoolManager
				.GetObject(_appearanceGuiPrefab, _controlsGuiPrefab.transform.position, Quaternion.identity)
				.GetComponent<InGameGui>();
		}
		else
		{
			gui = PoolManager
				.GetObject(_controlsGuiPrefab, _controlsGuiPrefab.transform.position, Quaternion.identity)
				.GetComponent<InGameGui>();
		}

		gui.gameObject.SetActive(true);
		gui.gameObject.transform.localPosition = _controlsGuiPrefab.transform.position;
		gui.SetOwner(gameObject);
		_mech.SetGui(gui);
	}

	private void OnEnable()
	{
		if (_mech != null)
		{
			if (isLocalPlayer)
			{
				_camera.transform.parent = _cameraPosition.transform;
				_camera.transform.localPosition = Vector3.zero;
				_camera.transform.localRotation = Quaternion.identity;
			}

			StartCoroutine(ResetGui());
		}

		Debug.Log("NotLocal");
	}

	private void OnDrawGizmos()
	{
		if (_camera == null)
		{
			return;
		}
		var ray = _camera.ScreenPointToRay(Input.mousePosition);
		var newDir = Vector3.ProjectOnPlane(ray.direction, transform.up).normalized;
		
		Gizmos.DrawLine(this.transform.position, this.transform.position + newDir * 5);

	}
}


/*

 рисовать линию огня как сейчас гизмо
 взрывы ракет с отталкиванием, скорость ракет уменьшить
 убрать вращения (angular drag на максималках)
 лёгкое отталкивание от лазера
 иногда лазер невидимый
 
 Меню
	можно старое
	(либо то крутое с постпроцессингом, но зачем?)
 
 Интерфейс
	UI менеджер для отображения хп, кнопок и тд.
	кнопки выбора оружия
	свой health bar
	
 Мультиплеер:
	заходить каждый сам за себя, 1 спаунер
	разные цвета ботов
 
 Карта:
	спаунеры
	управлене спаунерами

 ИИ:
	захват/защита спаунеров, ресурсов, оружия
	
*/