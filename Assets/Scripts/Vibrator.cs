using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static byte[] GenerateVibration(byte midiNote, float sharpness, float duration, float amplitude)
    {
        byte[] buffer = new byte[(int)(320 * duration)];
        float t = 0;
        for(int i = 0; i < buffer.Length; i++)
        {
            float freqVal = (Mathf.Sin(2 * Mathf.PI * GetFrequency(midiNote) * t) + 1) / 2;
            float sharpVal = Mathf.Clamp(Mathf.Sin(Mathf.PI * ((float)i / (buffer.Length -1))) * sharpness, 0, 1);
            buffer[i] = (byte)Mathf.RoundToInt(255 * (freqVal * sharpVal * amplitude));
            t += 1 / 320.0f;
        }
        return buffer;
    }

    private static float GetFrequency(byte midiNote)
    {
        float hzPerNote = 160.0f / 127;
        return hzPerNote * midiNote;
    }

    public static byte GetFreqencyMappedDrumNote(byte note)
    {
        switch (note)
        {
            case 36: return 32;
            case 38: return 80;
            case 47: return 56;
            case 45: return 48;
            case 46: return 112;
            case 49: return 104;
            default: return note;
        }
    }
}
