using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechAIController : MonoBehaviour {
	
	[SerializeField]
	private bool _canFollowPlayer = true;
	[SerializeField]
	private float _attackTriggerRadius = 25;
//	[SerializeField]
//	private float retreatTriggerRadius = 40;
	[SerializeField]
	private float _fireBias = 3;

	[SerializeField]
	private GameObject _guiPrefab;
	[SerializeField] 
	private Transform[] _waypoints;

	private int _currentWaypoint = 0;
	private GameObject _enemy;
	private Mech _mech;


	// Use this for initialization
	void Start () {
		_mech = GetComponent<Mech>();
		_mech.UseNavMesh = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var distanceToPlayer = _attackTriggerRadius + 1;
		if (_enemy == null || _enemy.activeInHierarchy == false)
		{
			var enemies = GameObject.FindGameObjectsWithTag("Damagable");
			if (enemies.Length == 1)
			{
				return;
			}
			_enemy = enemies[Random.Range(0, enemies.Length)];
			while (_enemy.gameObject == gameObject)
			{
				_enemy = enemies[Random.Range(0, enemies.Length)];
			}
		}
		else
		{
			distanceToPlayer = Vector3.Distance(this.transform.position, _enemy.transform.position);
		}
		
		if (distanceToPlayer < _attackTriggerRadius)
		{
			if (_canFollowPlayer)
			{
				_mech.MoveToPoint(_enemy.transform.position);
			}
			
			var enemyDirection = _enemy.transform.position - transform.position;
			var bias = Vector3.Cross(_mech.GetLookDirection(), enemyDirection).magnitude;
			if (bias < _fireBias)
			{
				_mech.Fire(0);
				_mech.Fire(1);
				
			}
			
			var newDir = Vector3.ProjectOnPlane(enemyDirection, this.transform.up).normalized;
			_mech.LookAt(newDir);
		}
		else
		{
			if (_waypoints.Length > 0)
			{
				if (_mech.CheckReachedTarget())
				{
					_currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
				}

				_mech.MoveToPoint(_waypoints[_currentWaypoint].position);
			}
		}
	}

	private void OnEnable()
	{
		if (_mech == null)
		{
			Start();
		}
		var gui = PoolManager.GetObject(_guiPrefab, Vector3.zero, Quaternion.identity).GetComponent<BotGui>();
		gui.gameObject.SetActive(true);
		gui.SetOwner(gameObject);
		_mech.SetGui(gui);
	}
	
		private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, _attackTriggerRadius);

		if (_enemy == null)
		{
			return;
		}
		
		var enemyDirection = _enemy.transform.position - transform.position;
		var newDir = Vector3.ProjectOnPlane(enemyDirection, transform.up).normalized;
		var bias = Vector3.Cross(_mech.GetLookDirection(), enemyDirection).magnitude;

		if (bias < _fireBias)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.red;
		}
		Gizmos.DrawRay(transform.position, newDir * _attackTriggerRadius);

	}


	private void OnDisable()
	{
		transform.parent.gameObject.SendMessage("Spawn", gameObject);
	}
}
