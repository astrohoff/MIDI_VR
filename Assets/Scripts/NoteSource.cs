using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSource : MonoBehaviour {
    private MidiAdaptor midiAdaptor;
    private byte midiChannel;
    private byte midiNote = 64;
    private IEnumerator playCoroutine;

    public void Ininitalize(MidiAdaptor midiAdaptor, byte channel, byte note)
    {
        this.midiAdaptor = midiAdaptor;
        midiChannel = channel;
        midiNote = note;
    }

    public void SetChannel(byte channel)
    {
        midiChannel = channel;
    }

    public void SetNote(byte note)
    {
        midiAdaptor.SetNoteOff(midiChannel, midiNote, 127);
        midiNote = note;
    }

    public void Play(byte attackVelocity, byte releaseVelocity, float duration)
    {
        if(playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        if(duration > 0)
        {
            playCoroutine = PlayCoroutine(attackVelocity, releaseVelocity, duration);
            StartCoroutine(playCoroutine);
        }
    }

    public void Play(byte attackVelocity)
    {
        if(playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        midiAdaptor.SetNoteOn(midiChannel, midiNote, attackVelocity);
    }

    private IEnumerator PlayCoroutine(byte attackVelocity, byte releaseVelocity, float durration)
    {
        midiAdaptor.SetNoteOn(midiChannel, midiNote, attackVelocity);
        yield return new WaitForSeconds(durration);
        midiAdaptor.SetNoteOff(midiChannel, midiNote, releaseVelocity);
    }

    public void Deaden(byte releaseVelocity)
    {
        if(playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        midiAdaptor.SetNoteOff(midiChannel, midiNote, releaseVelocity);
    }

    public byte GetNote()
    {
        return midiNote;
    }

    public byte GetChannel()
    {
        return midiChannel;
    }
}