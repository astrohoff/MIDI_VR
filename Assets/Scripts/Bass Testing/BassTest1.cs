using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Un4seen.Bass.AddOn.Midi;
using Un4seen.Bass;
using Un4seen.BassAsio;
using System;
using System.IO;

public class BassTest1 : MonoBehaviour {
	int streamHandle;
	int soundFontHandle;
	private BassAsioHandler asioHandler;
	public bool externalPlaying = false;
	private bool internalPlaying = false;
	// Use this for initialization
	void Start () {
		bool success = true;

		string debug = "";
		debug += "Device count: " + Bass.BASS_GetDeviceCount () + "\r\n";
		for(int i = 0; i < Bass.BASS_GetDeviceCount (); i++){
			debug += Bass.BASS_GetDeviceInfo (i) + "\r\n";
		}
		debug += "\r\n";
		debug += "ASIO device count: " + BassAsio.BASS_ASIO_GetDeviceCount () + "\r\n";
		for(int i = 0; i < BassAsio.BASS_ASIO_GetDeviceCount (); i++){
			debug += "  " + BassAsio.BASS_ASIO_GetDeviceInfo (i) + "\r\n";
		}

		success = Bass.BASS_SetConfig (BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0);
		if(!success){
			throw new Exception ("Config setting error: " + Bass.BASS_ErrorGetCode ());
		}

		success = Bass.BASS_Init (0, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
		if(!success){
			throw new Exception("Bass init error: " + Bass.BASS_ErrorGetCode ());
		}

		success = BassAsio.BASS_ASIO_Init (0, BASSASIOInit.BASS_ASIO_THREAD);
		if(!success){
			throw new Exception ("ASIO init error: " + BassAsio.BASS_ASIO_ErrorGetCode ());
		}

		streamHandle = BassMidi.BASS_MIDI_StreamCreate(16, BASSFlag.BASS_STREAM_DECODE, 0);
		if(streamHandle == 0){
			throw new Exception ("MIDI stream creation error: " + Bass.BASS_ErrorGetCode ());
		}

		soundFontHandle = BassMidi.BASS_MIDI_FontInit ("FluidR3_GM.SF2");
		if(soundFontHandle == 0){
			throw new Exception ("Sound font init error: " + Bass.BASS_ErrorGetCode ());
		}

		BASS_MIDI_FONT[] fonts = {new BASS_MIDI_FONT (soundFontHandle, -1, 0)};
		success = BassMidi.BASS_MIDI_StreamSetFonts (streamHandle, fonts, 1);
		if(!success){
			throw new Exception ("Sound font stream application error: " + Bass.BASS_ErrorGetCode ());
		}

		asioHandler = new BassAsioHandler (0, 0, streamHandle);
		success = asioHandler.Start (256, 0);
		if(!success){
			throw new Exception ("ASIO start error: " + BassAsio.BASS_ASIO_ErrorGetCode ());
		}
		debug += "Debug end";
		Debug.Log (debug);
	}
	
	// Update is called once per frame
	void Update () {
		OVRInput.Update ();
		if(!internalPlaying && externalPlaying || OVRInput.GetDown (OVRInput.Button.One)){
			bool success = BassMidi.BASS_MIDI_StreamEvent(streamHandle, 9, BASSMIDIEvent.MIDI_EVENT_NOTE, 38, 100);
			if(!success){
				throw new Exception ("MIDI event error: " + Bass.BASS_ErrorGetCode ());
			}
			internalPlaying = true;
		}else if(internalPlaying && !externalPlaying || OVRInput.GetUp (OVRInput.Button.One)){
			bool success = BassMidi.BASS_MIDI_StreamEvent(streamHandle, 9, BASSMIDIEvent.MIDI_EVENT_NOTE, 38, 0);
			if(!success){
				throw new Exception ("MIDI event error: " + Bass.BASS_ErrorGetCode ());
			}
			internalPlaying = false;
		}
	}


	string GetDetailedDeviceInfo(BASS_DEVICEINFO deviceInfo){
		string info = "";
		info += "Name: " + deviceInfo.name + "\r\n";
		info += "Default: " + deviceInfo.IsDefault + "\r\n";
		info += "Enabled: " + deviceInfo.IsEnabled + "\r\n";
		info += "Initialized: " + deviceInfo.IsInitialized + "\r\n";
		info += "Type: " + deviceInfo.type + "\r\n";
		return info;
	}

	void OnDestroy(){
		if(asioHandler != null){
			asioHandler.Stop ();
		}
		Bass.BASS_StreamFree (streamHandle);
		BassAsio.BASS_ASIO_Free ();

		Bass.BASS_Free();
	}
}
