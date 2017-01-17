using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitNotePlayer : MonoBehaviour {
    private NoteSource noteSource;
    private List<int> triggeringObjectIDs;
    public bool hold = true;
    public float ringDurration = 3;
    public float vibrationSharpness = 3;
    public float vibrateDurration = 0.25f;
    public float vibrationAmplitude = 0.5f;

	// Use this for initialization
	void Start () {
        triggeringObjectIDs = new List<int>();
        noteSource = GetComponent<NoteSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Left Stick" || c.tag == "Right Stick")
        {
            if (triggeringObjectIDs.IndexOf(c.gameObject.GetInstanceID()) == -1)
            {
                byte attack;
                if (c.tag == "Left Stick")
                {
                    attack = (byte)(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) * 127);
                }
                else
                {
                    attack = (byte)(OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) * 127);
                }
                if (hold)
                {
                    noteSource.Play(attack);
                }
                else
                {
                    noteSource.Play(attack, 127, ringDurration);
                }
                byte note = noteSource.GetNote();
                if(noteSource.GetChannel() == 9)
                {
                    note = Vibrator.GetFreqencyMappedDrumNote(note);
                }
                byte[] hapticBuffer = Vibrator.GenerateVibration(note, vibrationSharpness, vibrateDurration, vibrationAmplitude * ((float)attack / 127));
                OVRHapticsClip clip = new OVRHapticsClip(hapticBuffer, hapticBuffer.Length);
                if (c.tag == "Left Stick")
                {
                    
                    OVRHaptics.LeftChannel.Mix(clip);
                }
                else
                {
                    OVRHaptics.RightChannel.Mix(clip);
                }
            }
            triggeringObjectIDs.Add(c.gameObject.GetInstanceID());
        }       
    }

    private void OnTriggerExit(Collider c)
    {
        if(c.tag == "Left Stick" || c.tag == "Right Stick")
        {
            triggeringObjectIDs.Remove(c.gameObject.GetInstanceID());
            if (hold && triggeringObjectIDs.IndexOf(c.gameObject.GetInstanceID()) == -1)
            {
                noteSource.Deaden(127);
            }
        }      
    }
}
