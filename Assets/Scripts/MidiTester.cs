using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiTester : MonoBehaviour {
    public MidiAdaptor midiAdaptor;
    private NoteSource noteSource;
    public int channel = 0;
    public int instrument = 0;
    public int note = 60;
    public int velocity = 127;
    public float duration = 2;
    public bool playWithoutRing = false;
    public bool playWithRing = false;
    public bool deaden = false;
    private int oldInstrument, oldNote, oldChannel;
    private bool oldPlayWithoutRing = false;

	// Use this for initialization
	private void Start () {
        noteSource = gameObject.AddComponent<NoteSource>();
        midiAdaptor.SendProgramChange((byte)channel, (byte)instrument);
        oldInstrument = instrument;
        oldNote = note;
        oldChannel = channel;
        noteSource.Ininitalize(midiAdaptor, (byte)channel, (byte)note);
	}
	
	// Update is called once per frame
	private void Update () {
        if(oldChannel != channel)
        {
            midiAdaptor.SetAllSoundOff((byte)oldChannel);
            midiAdaptor.SendProgramChange((byte)channel, (byte)instrument);
            noteSource.SetChannel((byte)channel);
            oldChannel = channel;
        }
        if(oldInstrument != instrument)
        {
            midiAdaptor.SetAllSoundOff((byte)channel);
            midiAdaptor.SendProgramChange((byte)channel, (byte)instrument);
            oldInstrument = instrument;
        }
        if(oldNote != note)
        {
            noteSource.SetNote((byte)note);
            oldNote = note;
        }
        if(playWithoutRing != oldPlayWithoutRing)
        {
            if (playWithoutRing)
            {
                noteSource.Play((byte)velocity);
            }
            else
            {
                noteSource.Deaden((byte)velocity);
            }
            oldPlayWithoutRing = playWithoutRing;
        }
        if (playWithRing)
        {
            playWithRing = false;
            noteSource.Play((byte)velocity, (byte)velocity, duration);
        }
        if (deaden)
        {
            deaden = false;
            noteSource.Deaden((byte)velocity);
        }
	}
}
