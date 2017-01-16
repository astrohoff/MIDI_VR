using UnityEngine;

public class Instrument : MonoBehaviour {
    public MidiAdaptor midiAdaptor;
    public byte midiInstrument;
    public byte midiChannel;
    public NoteSource[] noteSources;
    public byte[] notes;

	// Use this for initialization
	void Start () {
        midiAdaptor.SendProgramChange(midiChannel, midiInstrument);
		for(int i = 0; i < noteSources.Length; i++)
        {
            byte tempNote = 64;
            if(notes.Length > i)
            {
                tempNote = notes[i];
            }
            noteSources[i].Ininitalize(midiAdaptor, midiChannel, tempNote);
        }
	}
}
