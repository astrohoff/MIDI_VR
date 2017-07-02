using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityManager : MonoBehaviour {
	public float speed;
	public Transform pickCenterAnchor;
	public Transform handAnchor;
	private float[] speedBuffer = new float[5];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float speedBufferSum = 0;
		for (int i = 1; i < speedBuffer.Length; i++) {
			speedBuffer [i] = speedBuffer [i - 1];
			speedBufferSum += speedBuffer [i];
		}
		speedBuffer[0] = OVRInput.GetLocalControllerVelocity (OVRInput.Controller.RTouch).magnitude;
		speedBufferSum += speedBuffer [0];
		speed = speedBufferSum / speedBuffer.Length;
		//To-do: add angular velocity.
	}
}
