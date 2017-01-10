using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WmmMidiAdaptor : MidiAdaptor
{
    private int handle = 0;

    // Imported Windows Multimedia functions.

    [DllImport("winmm.dll")]
    private static extern int midiOutGetNumDevs();
    [DllImport("winmm.dll")]
    private static extern int midiOutGetDevCaps(Int32 uDeviceID, ref MidiOutCaps lpMidiOutCaps, UInt32 cbMidiOutCaps);
    [DllImport("winmm.dll")]
    private static extern int midiOutOpen(ref int handel, int deviceID, MidiCallBack proc, int instance, int flags);
    [DllImport("winmm.dll")]
    protected static extern int midiOutShortMsg(int handle, int message);
    [DllImport("winmm.dll")]
    protected static extern int midiOutClose(int handle);

    [StructLayout(LayoutKind.Sequential)]
    public struct MidiOutCaps
    {
        public UInt16 wMid;
        public UInt16 wPid;
        public UInt32 vDriverVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String szPname;
        public UInt16 wTechnology;
        public UInt16 wVoices;
        public UInt16 wNotes;
        public UInt16 wChanneMask;
        public UInt32 dwSupport;
    }

    private delegate void MidiCallBack(int handle, int msg, int instance, int param1, int param2);

    private void SendMessage(byte status, byte channel, byte param1, byte param2)
    {
        int message = (status << 4) | channel | (param1 << 8) | (param2 << 16);
        int error = midiOutShortMsg(handle, message);
        if(error != 0)
        {
            throw new Exception("MIDI error " + error + ".");
        }
    }

    private void SendMessage(byte status, byte channel, byte param)
    {
        SendMessage(status, channel, param, 0);
    }


    // MIDI messages.

    public override void SendControlChange(byte channel, byte controller, byte value)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(controller, 0, 127);
        VerifyRange(value, 0, 127);
        SendMessage(Statuses.ControlChange, channel, controller, value);
    }

    public override void SendPitchBendChange(byte channel, ushort value)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(value, 0, 0x4000);
        // Value split into 7 most significant bits and 7 least significant bits.
        SendMessage(Statuses.PitchBendChange, channel, (byte)(value & 0x7F), (byte)(value >> 7));
    }

    public override void SendProgramChange(byte channel, byte program)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(program, 0, 127);
        SendMessage(Statuses.ProgramChange, channel, program);
    }

    public override void SetChannelAftertouch(byte channel, byte pressure)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(pressure, 0, 127);
        SendMessage(Statuses.ChannelAftertouch, channel, pressure);
    }

    public override void SetNoteAftertouch(byte channel, byte note, byte pressure)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(note, 0, 127);
        VerifyRange(pressure, 0, 127);
        SendMessage(Statuses.NoteAftertouch, channel, note, pressure);
    }

    public override void SetNoteOff(byte channel, byte note, byte velocity)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(note, 0, 127);
        VerifyRange(velocity, 0, 127);
        SendMessage(Statuses.NoteOff, channel, note, velocity);
    }

    public override void SetNoteOn(byte channel, byte note, byte velocity)
    {
        VerifyRange(channel, 0, 15);
        VerifyRange(note, 0, 127);
        VerifyRange(velocity, 0, 127);
        SendMessage(Statuses.NotOn, channel, note, velocity);
    }

    // MonoBehaviour methods.

    private void Awake()
    {
        midiOutOpen(ref handle, 0, null, 0, 0);
    }

    private void OnDestroy()
    {
        for(byte i = 0; i < 15; i++)
        {
            SetAllSoundOff(i);
        }
        midiOutClose(handle);
    }
}
