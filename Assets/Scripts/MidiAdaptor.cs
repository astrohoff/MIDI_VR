using System;
using UnityEngine;

public abstract class MidiAdaptor : MonoBehaviour
{
    public abstract void SetNoteOff(byte channel, byte note, byte velocity);
    public abstract void SetNoteOn(byte channel, byte note, byte velocity);
    public abstract void SetNoteAftertouch(byte channel, byte note, byte pressure);
    public abstract void SendControlChange(byte channel, byte controller, byte value);
    public abstract void SendProgramChange(byte channel, byte program);
    public abstract void SetChannelAftertouch(byte channel, byte pressure);
    public abstract void SendPitchBendChange(byte channel, ushort value);

    // Channel mode messages.

    public void SetAllSoundOff(byte channel)
    {
        SendControlChange(channel, ReservedControllers.AllSoundOff, 0);
    }

    public void ResetAllControllers(byte channel)
    {
        SendControlChange(channel, ReservedControllers.ResetAllControllers, 0);
    }

    public void SetLocalControl(byte channel, bool enabled)
    {
        byte value = 0;
        if (enabled)
            value = 127;
        SendControlChange(channel, ReservedControllers.LocalControl, value);
    }

    public void SetAllNotesOff(byte channel)
    {
        SendControlChange(channel, ReservedControllers.AllNotesOff, 0);
    }

    public void SetOmniModeOff(byte channel)
    {
        SendControlChange(channel, ReservedControllers.OmniModeOff, 0);
    }

    public void SetOmniModeOn(byte channel)
    {
        SendControlChange(channel, ReservedControllers.OmniModeOn, 0);
    }

    public void SetMonoModeOn(byte channel)
    {
        SendControlChange(channel, ReservedControllers.MonoModeOn, 0);
    }

    public void SetMonoPolyMode(byte channel)
    {
        SendControlChange(channel, ReservedControllers.PolyModeOn, 0);
    }


    protected void VerifyRange(int value, int min, int max)
    {
        if(value < min || value > max)
        {
            throw new Exception("Value out of range.");
        }
    }


    protected static class Statuses
    {
        public const byte NoteOff = 0x8;
        public const byte NotOn = 0x9;
        public const byte NoteAftertouch = 0xA;
        public const byte ControlChange = 0xB;
        public const byte ProgramChange = 0xC;
        public const byte ChannelAftertouch = 0xD;
        public const byte PitchBendChange = 0xE;
    }

    private static class ReservedControllers
    {
        public const byte AllSoundOff = 120;
        public const byte ResetAllControllers = 121;
        public const byte LocalControl = 122;
        public const byte AllNotesOff = 123;
        public const byte OmniModeOff = 124;
        public const byte OmniModeOn = 125;
        public const byte MonoModeOn = 126;
        public const byte PolyModeOn = 127;
    }
}