using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tank : NetworkBehaviour {

    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private GameObject _barrel;
    [SerializeField]
    private GameObject _turret;
    [SerializeField]
    private float _turretRotationSpeed = 1;
    [SerializeField]
    private float _bulletForce;
    [SerializeField]
    private float _cooldown = 1;
    [SerializeField]
    private float _accelSpeed = 1;
    [SerializeField]
    private float _maxSpeed = 1;
    [SerializeField]
    private float _rotationSpeed = 1;

    [SerializeField]
    private float _health = 100;

    private float _lastShotTime = 0;
    private Rigidbody _rigidbody;

    private Camera _camera;
    
	void Start ()
	{
        _rigidbody = this.gameObject.GetComponent<Rigidbody>();
	    _camera = gameObject.GetComponentInChildren<Camera>();
	    if (isLocalPlayer)
	    {
	        _camera.enabled = true;
	    }

	    Camera.main.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetAxis("Fire1") > 0)
        {
            Fire();
        }

	}

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        float drot = Input.GetAxis("Horizontal");
        float dpos = Input.GetAxis("Vertical");
        
        _rigidbody.MovePosition(this.transform.position + this.transform.forward * dpos * _accelSpeed);
        this.transform.Rotate(this.transform.up, drot * Time.deltaTime * _rotationSpeed);

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var newDir = Vector3.ProjectOnPlane(ray.direction, this.transform.up).normalized;
        _turret.transform.forward = Vector3.Lerp(
            transform.forward,
            newDir,
            _turretRotationSpeed * Time.deltaTime);


    }

    private void Fire()
    {
        if (_lastShotTime + _cooldown < Time.time)
        {
            var bullet = Instantiate(_bulletPrefab, _barrel.transform.position,
                _barrel.transform.rotation);
            var force = _bulletForce * bullet.transform.forward;

            bullet.GetComponent<Rigidbody>().AddForce(force,
                ForceMode.Impulse);
            
//            bullet.GetComponent<Bullet>().SetParentTag(this.gameObject.tag);

            _lastShotTime = Time.time;
        }
    }
    
    private void ReceiveDamage(float damage)
    {
        this._health -= damage;
        if (this._health <= 0)
        {
            
        }
    }
}



/* 
     Убрать хаки, вынести сложные условия в переменные
     Разделить UI и управление (сделать менеджер UI)
     Сделать нормальную разрушаемость или вынести её в функцию с понятным названием. 
     Вынести magic numbers.
     В общем, улучшить читабельность кода.
     
     нужно что-то решить с камерами
*/