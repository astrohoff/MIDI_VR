using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrettingController : MonoBehaviour {
    private int[] stringBaseNotes;
    public NoteSource[] strings;
    public Instrument instrument;
    private List<int> currentFrets = new List<int>();
    private enum Chord { Straight, Major6, Major5, Minor6, Minor5, Power6, Power5};
    private Chord currentChord = Chord.Straight;
    //private int rootString = 0;
	// Use this for initialization
	void Start () {
        stringBaseNotes = new int[instrument.notes.Length];
	    for(int i = 0; i < stringBaseNotes.Length; i++)
        {
            stringBaseNotes[i] = instrument.notes[i];
        }	
	}
	
	// Update is called once per frame
	void Update () {
        // Change root string.
        /*if (OVRInput.GetDown(OVRInput.Button.Four) && rootString < 5)
        {
            rootString++;
        }
        else if(OVRInput.GetDown(OVRInput.Button.Three) && rootString > 0)
        {
            rootString--;
        }*/

        // Chords.
        Chord newChord = Chord.Straight;
        if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger))
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
                {
                    //if(rootString == 0)
                    if (OVRInput.Get(OVRInput.Touch.Three))
                    {
                        newChord = Chord.Minor5;
                    }
                    //else if(rootString == 1)
                    else
                    {
                        newChord = Chord.Major6;

                    }
                }
                else
                {
                    //if(rootString == 0)
                    if (OVRInput.Get(OVRInput.Touch.Three))
                    {
                        newChord = Chord.Major5;

                    }
                    //else if(rootString == 1)
                    else
                    {
                        newChord = Chord.Minor6;

                    }
                }
            }
            else
            {
                if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
                {
                    if (OVRInput.Get(OVRInput.Touch.Three))
                    {
                        newChord = Chord.Power5;
                    }
                    else
                    {
                        newChord = Chord.Power6;
                    }
                }
            } 
        }
        if(newChord != currentChord)
        {
            currentChord = newChord;
            UpdateNotes();
        }
	}

    private void OnTriggerEnter(Collider c)
    {
        if(c.tag != "Fret")
        {
            return;
        }
        currentFrets.Add(c.GetComponent<FretInfo>().fretNumber);
        UpdateNotes();
        byte[] vibBuf = Vibrator.GenerateVibration(64, 1, 0.1f, 0.5f);
        OVRHaptics.LeftChannel.Mix(new OVRHapticsClip(vibBuf, vibBuf.Length));

    }

    private void OnTriggerExit(Collider c)
    {
        if(c.tag != "Fret")
        {
            return;
        }
        currentFrets.Remove(c.GetComponent<FretInfo>().fretNumber);
        UpdateNotes();
    }

    private void UpdateNotes()
    {
        int root = 0;
        if(currentFrets.Count > 0)
        {
            root = 1 + currentFrets[currentFrets.Count - 1];
        }
        int[] chordPattern = GetChordPattern(currentChord);
        for(int i = 0; i < strings.Length; i++)
        {
            strings[i].SetMute(chordPattern[i] == -1);
            strings[i].SetNote((byte)(stringBaseNotes[i] + root + chordPattern[i]));
            //strings[i].Play(96);
        }
    }
    
    private static int[] GetChordPattern(Chord chord)
    {
        switch (chord)
        {
            case Chord.Major6:
                return new int[6] { 0, 2, 2, 1, 0, 0 };
            case Chord.Minor6:
                return new int[6] { 0, 2, 2, 0, 0, 0 };
            case Chord.Major5:
                return new int[6] { -1, 0, 2, 2, 2, 0 };
            case Chord.Minor5:
                return new int[6] { -1, 0, 2, 2, 1, 0 };
            case Chord.Power6:
                return new int[6] { 0, 2, 2, -1, -1, -1 };
            case Chord.Power5:
                return new int[6] { -1, 0, 2, 2, -1, -1 };
            default:
                return new int[6] { 0, 0, 0, 0, 0, 0 };
        }
    }
    
}
