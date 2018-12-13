using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MechLegacy : MonoBehaviour {
	
	[SerializeField]
	private bool _canFollowPlayer = true;
	[SerializeField]
	private GameObject _bulletPrefab;
	[SerializeField]
	private GameObject _turret;
	[SerializeField]
	private GameObject _barrel1;
	[SerializeField]
	private GameObject _barrel2;
	[SerializeField]
	private float _turretRotationSpeed = 1;
	[SerializeField]
	private float _bulletForce;
	[SerializeField]
	private float _cooldown = 1;
	[SerializeField]
	private float _attackTriggerRadius = 25;
//	[SerializeField]
//	private float retreatTriggerRadius = 40;
	[SerializeField]
	private float _fireBias = 3;
	
	[SerializeField]
	private float _maxHealth = 100;
	[SerializeField]
	private float _currentHealth = 100;
	
	[SerializeField]
	private GameObject _healthGuiElement;
	private Image _healthBarImage;
	private Text _healthBarText;
	[SerializeField]
	private float _maxGuiDistance = 50;
	
	[SerializeField] 
	private Transform[] _waypoints;

	private int _currentWaypoint = 0;

	private float _lastShotTime = 0;
	private Animator _animator;
	private NavMeshAgent _agent;
	private Transform _player;

	// Use this for initialization
	void Start () {
		_animator = this.gameObject.GetComponentInChildren<Animator>();
		_agent = this.gameObject.GetComponent<NavMeshAgent>();
		_player = GameObject.FindWithTag("Player").transform;

		_healthBarImage = _healthGuiElement.transform.GetChild(0).GetComponent<Image>();
		_healthBarText = _healthGuiElement.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(this.transform.position, _player.position) < _attackTriggerRadius)
		{
			if (_canFollowPlayer)
			{
				_agent.destination = _player.position;
			}
		}
		else
		{
			if (_agent.remainingDistance < 1 && !_agent.pathPending)
			{
				_currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
			}
			_agent.destination = _waypoints[_currentWaypoint].position;
			
//			Debug.Log(_currentWaypoint);
		}
		_animator.SetFloat("speed", _agent.velocity.magnitude);
		
		if (Vector3.Angle(Camera.main.transform.forward, this.transform.position - Camera.main.transform.position) < 90)
		{
			_healthGuiElement.SetActive(true);
			Vector2 screenPoint =
				RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position + transform.up * 4);
			_healthGuiElement.transform.position = screenPoint;
			var dist = (this.transform.position - _player.transform.position).magnitude;
			var scale = Mathf.Max(1 - dist / _maxGuiDistance, 0);
			_healthGuiElement.transform.localScale = new Vector3(scale, scale, scale);
		}
		else
		{
			_healthGuiElement.SetActive(false);
		}
	}

	void FixedUpdate()
	{
		if (_player == null)
		{
			_player = GameObject.FindWithTag("Player").transform;
		}
		else if (Vector3.Distance(this.transform.position, _player.position) < _attackTriggerRadius)
		{
			var enemyDirection = _player.position - _barrel1.transform.position;
			var proj = Vector3.Project(enemyDirection, _barrel1.transform.right);
			if (proj.magnitude < _fireBias)
			{
				Fire();
			}
			
			var newDir = Vector3.ProjectOnPlane(enemyDirection, this.transform.up).normalized;
			_turret.transform.forward = Vector3.Lerp(
				_turret.transform.forward,
				newDir,
				_turretRotationSpeed * Time.deltaTime);
		}
	}

//	private void OnDrawGizmos()
//	{
//		Gizmos.DrawRay(this.transform.position, _agent.velocity);
//		Gizmos.DrawWireSphere(this.transform.position, _attackTriggerRadius);
//
////		Gizmos.DrawLine(this.transform.position, player.position);
//		
//		var enemyDirection = _player.position - _barrel1.transform.position;
//		var newDir = Vector3.ProjectOnPlane(enemyDirection, _turret.transform.up).normalized;
//		Gizmos.DrawRay(this.transform.position, newDir * 10);
//	}


	private void Fire()
	{
		if (_lastShotTime + _cooldown < Time.time)
		{
			var bullet = Instantiate(_bulletPrefab, _barrel1.transform.position,
				_barrel1.transform.rotation);
			var force = _bulletForce * bullet.transform.forward;

			bullet.GetComponent<Rigidbody>().AddForce(force,
				ForceMode.Impulse);
			
//			bullet.GetComponent<Bullet>().SetParentTag(this.gameObject.tag);

			_lastShotTime = Time.time;
		}
	}

	private void ReceiveDamage(float damage)
	{
//		Debug.Log("received: " + damage);	
		this._currentHealth -= damage;
		var barValue = _currentHealth / _maxHealth;
		_healthBarText.text = barValue * 100 + "%";
		_healthBarImage.fillAmount = barValue;
		
		if (this._currentHealth <= 0)
		{
			foreach (var child in transform.GetComponentsInChildren<MeshRenderer>())
			{
				var r = child.gameObject.AddComponent<Rigidbody>();
				var c = child.gameObject.AddComponent<SphereCollider>();
				child.transform.parent = this.transform;
			}
			_healthGuiElement.SetActive(false);
			_animator.enabled = false;
			_agent.enabled = false;
			this.enabled = false;
			this.GetComponent<CapsuleCollider>().enabled = false;
			StartCoroutine(this.DeleteDestroyed());
//			this.gameObject.SetActive(false);
		}
	}

	public IEnumerator DeleteDestroyed()
	{
		yield return new WaitForSeconds(10);
		GameObject.Destroy(this.gameObject);
	}
}
