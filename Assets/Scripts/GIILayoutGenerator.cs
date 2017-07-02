using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
// Generates the matrix of note pads for a Generic Instrument Interface.
// Used as a tool to quickly get layout.
// Check run to generate pad layout with given parameters.
public class GIILayoutGenerator : MonoBehaviour
{
	// Each row is an octave, so 12 notes / columns.
	private const int ColumnCount = 12;
	private const int RowCount = 11;
	// # of valid notes for MIDI.
	private const int NoteCount = 128;

	// Source GameObject that is copied for each note.
	public GameObject padPrefab;
	// Transform that will be the parent of the note pads.
	public Transform padParent;
	// Instrument script for GII, needed to setup note sources.
	public Instrument instrument;
	// X and Y bounds of the whole matrix of pads.
	public Vector2 instrumentSize = new Vector2(1.5f, 1f);
	public float padScale = 1f;
	// This is checked in the inspector to regenerate pads.
	// Can be used if pads get duplicated.
	public bool generate = false;

	// Array of cloned pad GameObjects.
	private GameObject[] padObjs;

	void Start()
	{
		padObjs = new GameObject[NoteCount];
	}

	// Update is called once per frame
	void Update()
	{
		if (VerifyGenerationRequirements())
		{
			DestroyPads();
			GeneratePads();
			SetInstrumentNoteSources();
		}
		else
		{
			generate = false;
		}
	}

	// Destroy all pads.
	// Used to prevent generation of duplicate pads.
	private void DestroyPads()
	{
		int padCount = padParent.childCount;
		for (int i = 0; i < padCount; i++)
		{
			DestroyImmediate(padParent.GetChild(0).gameObject);
		}
	}

	// Calculate the position of each pad.
	private Vector3[] GetPadPositions()
	{
		Vector3[] padPositions = new Vector3[NoteCount];
		Bounds padBounds = padPrefab.GetComponent<MeshRenderer>().bounds;
		float xStart = -instrumentSize.x / 2 + padBounds.size.x / 2;
		float yStart = instrumentSize.y / 2 - padBounds.size.y / 2;
		float deltaX = (instrumentSize.x - padBounds.size.x) / (ColumnCount - 1);
		float deltaY = -(instrumentSize.y - padBounds.size.y) / (RowCount - 1);

		for (int row = 0; row < RowCount; row++)
		{
			int noteIndex = row * ColumnCount;
			for (int col = 0; col < ColumnCount && noteIndex < NoteCount; col++)
			{				
				float xPos = xStart + deltaX * col;
				float yPos = yStart + deltaY * row;
				padPositions[noteIndex] = new Vector3(xPos, yPos, 0);
				noteIndex++;
			}
		}
		return padPositions;
	}

	// Spawn and place pads.
	private void GeneratePads()
	{
		Vector3[] padPositions = GetPadPositions();
		Vector3 padScaleVect = new Vector3(padScale, padScale, padScale);
		for (int i = 0; i < NoteCount; i++)
		{
			padObjs[i] = Instantiate(padPrefab);
			padObjs[i].name = "pad " + i;
			padObjs[i].transform.SetParent(padParent);
			padObjs[i].transform.localPosition = padPositions[i];
			padObjs[i].transform.localScale = padScaleVect;
			padObjs[i].GetComponent<NotePadController>().instrument = instrument;
		}
		generate = false;
	}

	// Check whether the references needed to generate pads have been set.
	private bool VerifyGenerationRequirements()
	{
		if (!generate)
		{
			return false;
		}
		if (padPrefab == null)
		{
			Debug.Log("Pad prefab not set.");
			return false;
		}
		if (padParent == null)
		{
			Debug.Log("Pad parent not set.");
			return false;
		}
		if (instrument == null)
		{
			Debug.Log("Instrument script not set.");
			return false;
		}
		return true;
	}

	private void SetInstrumentNoteSources()
	{
		NoteSource[] noteSources = new NoteSource[NoteCount];
		byte[] noteValues = new byte[NoteCount];
		for (int i = 0; i < NoteCount; i++)
		{
			noteSources[i] = padObjs[i].GetComponent<NoteSource>();
			noteValues[i] = (byte)i;
		}
		instrument.noteSources = noteSources;
		instrument.notes = noteValues;
	}
}
