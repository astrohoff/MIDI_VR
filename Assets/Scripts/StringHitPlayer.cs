using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHitPlayer : MonoBehaviour {
    public float ringDurration = 5f;
	public VelocityManager strikerVelocityManager;
    private NoteSource noteSource;

	private System.Diagnostics.Stopwatch timer;
	private long lastEventTime = 0;

	// Use this for initialization
	void Start () {
		timer = new System.Diagnostics.Stopwatch ();
        noteSource = GetComponent<NoteSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision c)
    {
        if(c.collider.tag == "Striker")
        {            
        }
    }

	private void OnCollisionStay(Collision c){
		if (c.collider.tag == "Striker") {
		}
	}

	private void OnCollisionExit(Collision c){
		if (c.collider.tag == "Striker") {
			byte noteVelocity = (byte)(Mathf.Clamp(strikerVelocityManager.speed / 1f,0.6f, 1) * 127);
			noteSource.Play(127, 127, ringDurration);

			byte[] vibBuf = Vibrator.GenerateVibration(noteSource.GetNote(), 4, 0.05f, 0.2f);
			OVRHaptics.RightChannel.Mix(new OVRHapticsClip(vibBuf, vibBuf.Length));
		}
	}
}
