using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotePadController : MonoBehaviour {
	public Instrument instrument;
	public int noteSourceIndex;

	private MeshRenderer meshRend;
	// List of interacting GameObjects focused on this pad.
	// Works like a stack, but GameObjects can leave from any position.
	private LinkedList<GameObject> focusOwners;
	private NoteSource noteSource;
	private bool isPlaying = false;

	public enum State {Idle, Selected, Triggered };
	// Use this for initialization
	void Start () {
		meshRend = GetComponent<MeshRenderer> ();
		focusOwners = new LinkedList<GameObject> ();
		noteSource = GetComponent<NoteSource> ();
		noteSourceIndex = Array.IndexOf(instrument.noteSources, noteSource);
	}
	
	// Update is called once per frame
	void Update () {
	}
		
	public void RegisterFocusLoss(GameObject source){
		if(focusOwners.First.Value == source){
			if(isPlaying){
				noteSource.Deaden (127);
				isPlaying = false;
			}

		}
		focusOwners.Remove (source);
		if(focusOwners.Count > 0){
			SetState (State.Selected);
		}else{
			SetState (State.Idle);
		}
	}

	public void RegisterPlay(byte velocity, GameObject source){
		focusOwners.Remove (source);
		focusOwners.AddFirst (source);
		noteSource.Play (velocity);
		SetState (State.Triggered);
		isPlaying = true;
	}

	public void RegisterRelease(byte velocity, GameObject source){
		if(source == focusOwners.First.Value){
			noteSource.Deaden (velocity);
			SetState (State.Selected);
			isPlaying = false;
		}
	}

	public void RegisterFocus(GameObject source){
		focusOwners.AddLast (source);
		if(focusOwners.First.Value == source){
			SetState (State.Selected);
		}
	}

	public void SetState(State state){
		if (state == State.Idle) {
			meshRend.material.SetColor ("_EmissionColor", Color.black);
		}else if (state == State.Selected) {
			meshRend.material.SetColor ("_EmissionColor", focusOwners.First.Value.GetComponent<PointerNotePlayer>().GetHighlightColor((State.Selected)));
		} else if (state == State.Triggered) {
			meshRend.material.SetColor ("_EmissionColor", focusOwners.First.Value.GetComponent<PointerNotePlayer>().GetHighlightColor((State.Triggered)));
		}
	}

	private static string GetStateString(State state){
		switch(state){
		case State.Idle:
			return "idle";
		case State.Selected:
			return "selected";
		case State.Triggered:
			return "triggered";
		default:
			return "";
		}
	}
}
