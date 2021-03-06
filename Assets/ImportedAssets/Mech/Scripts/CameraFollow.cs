using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private Transform mechTransform;
	private Transform pelvis;
	private Vector3 offset;

	void Start () {
		
		mechTransform = GameObject.Find ("mech").transform;
		pelvis = mechTransform.Find ("/mech/Mech/Root/Pelvis");
		offset = transform.position - pelvis.position;

	}
	

	void LateUpdate () {
		
		Vector3 camPos = transform.position;
		Vector3 newPos =  Vector3.Lerp(camPos, pelvis.position + offset, Time.deltaTime *3);
		transform.position = newPos;
	}
}
