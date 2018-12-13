using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBullet : Bullet
{

	private TrailRenderer _trailRenderer;

	private new void Start()
	{
		base.Start();
		_trailRenderer = GetComponent<TrailRenderer>();
	}

	private void OnDisable()
	{
		if (_trailRenderer != null)
		{
			_trailRenderer.Clear();
		}
	}

}
