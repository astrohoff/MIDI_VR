using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationVibrationController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 leftAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.LTouch);
		//Debug.Log (leftAccel.magnitude);
		byte vibVal = (byte)(Mathf.Clamp01(leftAccel.magnitude / 25 )* 128);
		byte[] buffer = new byte[10];
		for (int i = 0; i < buffer.Length; i++) {
			buffer [i] = vibVal;
		}
		OVRHaptics.LeftChannel.Preempt (new OVRHapticsClip (buffer, buffer.Length));

	}
}
