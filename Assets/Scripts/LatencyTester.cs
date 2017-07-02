using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatencyTester : MonoBehaviour {
	public AudioSource audioSource;
	public NoteSource noteSource;
	private float myTime = 0;
	long previousTime = 0;
	OVRHapticsClip pulse;
	bool previousDown = false;
	float trigDownVal;
	int framesSinceTrigDown = 90;
	float oldTrigVal = 0;

	// Use this for initialization
	void Start () {
		byte[] rumbleData = new byte[6];
		for (int i = 0; i < rumbleData.Length; i++) {
			rumbleData [i] = 255;
		}
		pulse = new OVRHapticsClip (rumbleData, rumbleData.Length);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		OVRInput.Update ();
		if (OVRInput.GetDown (OVRInput.Button.One)) {
			//audioSource.Play ();
			noteSource.Play (127);
			//OVRHaptics.LeftChannel.Mix (pulse);
			//OVRHaptics.Process ();
		}
			
		float trigVal = OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger);
		if (oldTrigVal == 0 && trigVal > 0) {
			framesSinceTrigDown = 0;
			trigDownVal = trigVal;
		} else if (framesSinceTrigDown == 1){
			float trigSpeed1 = (trigVal - trigDownVal) / (Time.fixedDeltaTime * framesSinceTrigDown);
			float trigSpeed2 = trigVal / (Time.fixedDeltaTime * (framesSinceTrigDown + 1));
			Debug.Log (trigSpeed1 + "\r\n" + trigSpeed2 + "\r\n" + trigDownVal);
		}
		framesSinceTrigDown++;
		oldTrigVal = trigVal;
	}
}
