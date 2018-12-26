using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedMech : MonoBehaviour
{
	// fixme: Что за фигня тут происходит, почему без SerializeField все данные из полей удаляются после setActive false/true?
	[SerializeField]
	private Transform[] _children;
	[SerializeField]
	private List<Vector3> _positions = new List<Vector3>();
	[SerializeField]
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
//		Debug.Log(_children);
		for (var i = 0; i < _children.Length; i++)
		{
			_children[i].position = _positions[i];
			_children[i].rotation = _rotations[i];
		}
	}
}
