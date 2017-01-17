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
            float freqVal = (Mathf.Sin(2 * Mathf.PI * GetFrequency(midiNote, 40) * t) + 1) / 2;
            float sharpVal = Mathf.Clamp(Mathf.Sin(Mathf.PI * ((float)i / (buffer.Length -1))) * sharpness, 0, 1);
            buffer[i] = (byte)Mathf.RoundToInt(255 * (freqVal * sharpVal * amplitude));
            t += 1 / 320.0f;
        }
        return buffer;
    }

    private static float GetFrequency(byte midiNote, float minFrequency)
    {
        float hzPerNote = (160 - minFrequency) / 127;
        return minFrequency + hzPerNote * midiNote;
    }

    public static byte GetFreqencyMappedDrumNote(byte note)
    {
        switch (note)
        {
            case 35: return 0;
            case 36: return 0;
            case 37: return 80;
            case 38: return 80;
            case 39: return 80;
            case 40: return 80;
            case 41: return 16;
            case 42: return 112;
            case 43: return 26;
            case 44: return 112;
            case 45: return 36;
            case 46: return 112;
            case 47: return 46;
            case 48: return 56;
            case 49: return 112;
            case 50: return 66;
            case 51: return 104;
            case 52: return 96;
            case 53: return 104;
            case 54: return 96;
            case 55: return 104;
            case 56: return 64;
            case 57: return 112;
            case 58: return 64;
            case 59: return 104;
            default:
                Debug.Log("Missing drum frequency mapping: " + note + ".");
                return note;
        }
    }
}
