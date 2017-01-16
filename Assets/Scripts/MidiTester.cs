using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiTester : MonoBehaviour {
    public MidiAdaptor midiAdaptor;
    private NoteSource noteSource;
    public byte channel = 0;
    public byte instrument = 0;
    public byte note = 60;
    public byte velocity = 127;
    public bool sendNoteAftertouch = false;
    public bool sendChannelAftertouch = false;
    public float duration = 2;
    public bool playWithoutRing = false;
    public bool playWithRing = false;
    public bool deaden = false;
    private byte oldInstrument, oldNote, oldChannel;
    private bool oldPlayWithoutRing = false;
    public ushort pitchChange = 0x2000;
    public bool sendPitchChange = false;
    public byte controlNumber = 0;
    public byte controlValue = 0;
    public bool sendControlChange = false;

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
            midiAdaptor.SetAllSoundOff(oldChannel);
            midiAdaptor.SendProgramChange(channel, instrument);
            noteSource.SetChannel(channel);
            oldChannel = channel;
        }
        if(oldInstrument != instrument)
        {
            midiAdaptor.SetAllSoundOff(channel);
            midiAdaptor.SendProgramChange(channel, instrument);
            oldInstrument = instrument;
        }
        if(oldNote != note)
        {
            noteSource.SetNote(note);
            oldNote = note;
        }
        if(playWithoutRing != oldPlayWithoutRing)
        {
            if (playWithoutRing)
            {
                noteSource.Play(velocity);
            }
            else
            {
                noteSource.Deaden(velocity);
            }
            oldPlayWithoutRing = playWithoutRing;
        }
        if (playWithRing)
        {
            playWithRing = false;
            noteSource.Play(velocity, velocity, duration);
        }
        if (sendPitchChange)
        {
            sendPitchChange = false;
            midiAdaptor.SendPitchBendChange(channel, pitchChange);
        }
        if (sendNoteAftertouch)
        {
            sendNoteAftertouch = false;
            midiAdaptor.SetNoteAftertouch(channel, note, velocity);
        }
        if (sendChannelAftertouch)
        {
            sendChannelAftertouch = false;
            midiAdaptor.SetChannelAftertouch(channel, velocity);
        }
        if (sendControlChange)
        {
            sendControlChange = false;
            midiAdaptor.SendControlChange(channel, controlNumber, controlValue);
        }
        if (deaden)
        {
            deaden = false;
            noteSource.Deaden(velocity);
        }
	}
}
