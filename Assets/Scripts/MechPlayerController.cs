using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(IMech))]
public class MechPlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _controlsGuiPrefab;
    [SerializeField] private GameObject _menuPrefab;

    [SerializeField] private GameObject _cameraPosition;

    private IMech _mech;
    private Camera _camera;
    private GameObject _menuInstance;
    private PlayerMenu _playerMenu;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        _mech = GetComponent<IMech>();
        _camera = Camera.main;
        
        _menuInstance = Instantiate(_menuPrefab);
        _playerMenu = _menuInstance.GetComponent<PlayerMenu>();
        _playerMenu.SetPlayer(gameObject);
        _menuInstance.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);
        _menuInstance.transform.localPosition = _menuPrefab.transform.position; //fixme
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
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

    private void ResetGui()
    {
        var gui = PoolManager
            .GetObject(_controlsGuiPrefab, _controlsGuiPrefab.transform.position, Quaternion.identity)
            .GetComponent<InGameGui>();
        gui.gameObject.SetActive(true);
        gui.gameObject.transform.localPosition = _controlsGuiPrefab.transform.position;
        gui.SetOwner(gameObject);
        _mech.SetGui(gui);
    }

    private void OnEnable()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (_mech != null)
        {
            _camera.transform.parent = _cameraPosition.transform;
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.localRotation = Quaternion.identity;

            ResetGui();
        }
    }

    private void OnDisable()
    {
        if (isLocalPlayer)
        {
            _playerMenu.ShowPlayButton();
        }
    }

    private void OnDrawGizmos()
    {
        if (_camera == null)
        {
            return;
        }

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var newDir = Vector3.ProjectOnPlane(ray.direction, transform.up).normalized;

        Gizmos.DrawLine(transform.position, transform.position + newDir * 5);
    }
}
