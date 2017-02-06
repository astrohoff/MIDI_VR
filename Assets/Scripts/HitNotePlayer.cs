using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitNotePlayer : MonoBehaviour
{
    private NoteSource noteSource;
    private List<int> collidingObjectIDs;
    public float maxVelocity = 2;
    public bool hold = true;
    public float ringDurration = 3;
    public float vibrationSharpness = 3;
    public float vibrateDurration = 0.25f;
    public float vibrationAmplitude = 0.5f;
    private uint frame;

    // Use this for initialization
    void Start()
    {
        collidingObjectIDs = new List<int>();
        noteSource = GetComponent<NoteSource>();
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
    }

    private void OnCollisionEnter(Collision c)
    {
        // Check the colliding object.
        if (c.collider.tag == "Left Stick" || c.collider.tag == "Right Stick" || c.collider.tag == "Striker")
        {
            // Make sure the colliding object was not already colliding.
            if (collidingObjectIDs.IndexOf(c.gameObject.GetInstanceID()) == -1)
            {
                byte attack;
                if (c.collider.tag == "Left Stick")
                {
                    Vector3 stickVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                    //stickSpeed += Mathf.Tan(OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch).eulerAngles) * 0.15f
                    attack = (byte)(Mathf.Clamp01(stickVelocity.magnitude / maxVelocity) * 127);
                    //Debug.Log("Velocity: " + stickVelocity);
                }
                else if (c.collider.tag == "Right Stick")
                {
                    Vector3 stickVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                    attack = (byte)(Mathf.Clamp01(stickVelocity.magnitude / maxVelocity) * 127);
                    //Debug.Log("Velocity: " + stickVelocity);
                }
                else
                {
                    attack = (byte)(Mathf.Clamp01(c.impulse.magnitude / maxVelocity) * 127);
                    //Debug.Log("Velocity: " + c.relativeVelocity);
                }
                // Play note.
                if (hold)
                {
                    noteSource.Play(attack);
                }
                else
                {
                    noteSource.Play(attack, 127, ringDurration);
                }
                // Generate note haptic vibration if a controller object collided.
                if (c.collider.tag != "Striker")
                {
                    byte note = noteSource.GetNote();
                    // Drum channel (9) notes do not correspond to frequency, must use manual mapping.
                    if (noteSource.GetChannel() == 9)
                    {
                        note = Vibrator.GetFreqencyMappedDrumNote(note);
                    }
                    byte[] hapticBuffer = Vibrator.GenerateVibration(note, vibrationSharpness, vibrateDurration, vibrationAmplitude * ((float)attack / 127));
                    OVRHapticsClip clip = new OVRHapticsClip(hapticBuffer, hapticBuffer.Length);
                    if (c.collider.tag == "Left Stick")
                    {

                        OVRHaptics.LeftChannel.Mix(clip);
                    }
                    else
                    {
                        OVRHaptics.RightChannel.Mix(clip);
                    }
                }

            }
            collidingObjectIDs.Add(c.gameObject.GetInstanceID());
        }
    }

    private void OnCollisionExit(Collision c)
    {
        if (c.collider.tag == "Left Stick" || c.collider.tag == "Right Stick" || c.collider.tag == "Striker")
        {
            collidingObjectIDs.Remove(c.gameObject.GetInstanceID());
            if (hold && collidingObjectIDs.Count == 0)
            {
                noteSource.Deaden(127);
            }
        }
    }
}
