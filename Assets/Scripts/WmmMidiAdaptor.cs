using System;
using System.Text;
using System.Runtime.InteropServices;

public class WmmMidiAdaptor : IMidiAdaptor
{
    // Imported Windows Multimedia functions.

    [DllImport("winmm.dll")]
    private static extern long mciSendString(string command, StringBuilder returnValue,
                                             int returnLength, IntPtr winHandel);
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

    private delegate void MidiCallBack(int handel, int msg, int instance, int param1, int param2);


    public override void SendControlChange(byte channel, byte controller, byte value)
    {
        throw new NotImplementedException();
    }

    public override void SendPitchBendChange(byte channel, ushort value)
    {
        throw new NotImplementedException();
    }

    public override void SendProgramChange(byte channel, byte program)
    {
        throw new NotImplementedException();
    }

    public override void SetChannelAftertouch(byte channel, byte pressure)
    {
        throw new NotImplementedException();
    }

    public override void SetNoteAftertouch(byte channel, byte note, byte pressure)
    {
        throw new NotImplementedException();
    }

    public override void SetNoteOff(byte channel, byte note, byte velocity)
    {
        throw new NotImplementedException();
    }

    public override void SetNoteOn(byte channel, byte note, byte velocity)
    {
        throw new NotImplementedException();
    }
}
