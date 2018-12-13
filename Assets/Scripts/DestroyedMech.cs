using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedMech : MonoBehaviour
{
	private Transform[] _children;
	private List<Vector3> _positions = new List<Vector3>();
	private List<Quaternion> _rotations = new List<Quaternion>();
	
	
	// Use this for initialization
	void Start ()
	{
		_children = GetComponentsInChildren<Transform>();
		foreach (var child in _children)
		{
			_positions.Add(child.position);
			_rotations.Add(child.rotation);
		}
	}
	
	void OnDisable()
	{
		for (var i = 0; i < _children.Length; i++)
		{
			_children[i].position = _positions[i];
			_children[i].rotation = _rotations[i];
		}
	}
}
