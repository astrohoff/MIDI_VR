using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Un4seen.Bass;
using Un4seen.BassWasapi;
using Un4seen.Bass.AddOn.Midi;

public class BassWasapiTest : MonoBehaviour {
	public bool externalPlaying;
	private bool internalPlaying;
	public byte midiChannel = 9;
	public byte midiNote = 38;
	private BassWasapiHandler wasapiHandeler;
	private int streamHandle, soundFontHandle;

	void Start () {
		bool success = true;

		string debug = "";
		debug += "Device count: " + Bass.BASS_GetDeviceCount () + "\r\n";
		for(int i = 0; i < Bass.BASS_GetDeviceCount (); i++){
			debug += "  " + Bass.BASS_GetDeviceInfo (i) + "\r\n";
		}
		debug += "\r\n";

		BASS_WASAPI_DEVICEINFO[] wasapiDeviceInfos = BassWasapi.BASS_WASAPI_GetDeviceInfos ();
		debug += "WASAPI device count: " + wasapiDeviceInfos.Length + "\r\n";
		for(int i = 0; i < wasapiDeviceInfos.Length; i++){
			if(wasapiDeviceInfos[i].IsEnabled){
				debug += "  " + i + ") " + wasapiDeviceInfos [i] + "\r\n";
			}
		}
		debug += "\r\n";


		success = Bass.BASS_SetConfig (BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0);
		CheckForError (success, "Bass config");

		success = Bass.BASS_Init (0, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
		CheckForError (success, "Bass init");

		streamHandle = BassMidi.BASS_MIDI_StreamCreate (16, BASSFlag.BASS_STREAM_DECODE , 1);
		CheckForError (streamHandle, "Stream create");

		wasapiHandeler = new BassWasapiHandler (6, true, 48000, 2, 0.015f, 0);

		success = wasapiHandeler.AddOutputSource (streamHandle, BASSFlag.BASS_DEFAULT);
		CheckForError (success, "WASAPI source add");

		soundFontHandle = BassMidi.BASS_MIDI_FontInit ("FluidR3_GM.SF2");
		CheckForError (soundFontHandle, "SoundFont init");

		BASS_MIDI_FONT[] fonts = {new BASS_MIDI_FONT (soundFontHandle, -1, 0)};
		success = BassMidi.BASS_MIDI_StreamSetFonts (streamHandle, fonts, 1);
		CheckForError (success, "SoundFont stream application");

		success = wasapiHandeler.Init ();
		CheckForError (success, "WASAPI init");

		success = wasapiHandeler.Start ();
		CheckForError (success, "WASAPI start");


		debug += "Device stats\r\n";
		debug += "  Bass playback buffer length: " + Bass.BASS_GetConfig (BASSConfig.BASS_CONFIG_BUFFER) + " ms\r\n";
		debug += "  WASAPI buffer length: " + wasapiHandeler.BufferLength + " s\r\n";
		debug += "  Update rate: " + wasapiHandeler.UpdatePeriod + " s\r\n";
		Debug.Log (debug);

	}
	
	void Update () {
		OVRInput.Update ();
		if(!internalPlaying && externalPlaying || OVRInput.GetDown (OVRInput.Button.One)){
			bool success = BassMidi.BASS_MIDI_StreamEvent(streamHandle, midiChannel, BASSMIDIEvent.MIDI_EVENT_NOTE, midiNote, 100);
			if(!success){
				throw new Exception ("MIDI event error: " + Bass.BASS_ErrorGetCode ());
			}
			internalPlaying = true;
		}else if(internalPlaying && !externalPlaying || OVRInput.GetUp (OVRInput.Button.One)){
			bool success = BassMidi.BASS_MIDI_StreamEvent(streamHandle, midiChannel, BASSMIDIEvent.MIDI_EVENT_NOTE, midiNote, 0);
			if(!success){
				throw new Exception ("MIDI event error: " + Bass.BASS_ErrorGetCode ());
			}
			internalPlaying = false;
		}
	}

	private void OnDestroy(){
		Debug.Log ("Freeing Bass");
		BassWasapi.BASS_WASAPI_Free ();
		Bass.BASS_Free ();
	}

	private void CheckForError(bool success, string errorType){
		if(!success){
			throw new Exception (errorType + " error: " + Bass.BASS_ErrorGetCode ());
		}
	}

	private void CheckForError(int handel, string errorType){
		CheckForError (handel != 0, errorType);
	}
}
