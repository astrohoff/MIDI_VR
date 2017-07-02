using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointerNotePlayer : MonoBehaviour
{
    public GameObject pointerPrefab;
    public Hand hand = Hand.Right;
    public bool pointerLockEnabled = false;
	public Color baseColor;

    private GameObject pointer;
    private TouchControls touchControls;
    private bool pointerEnabled = false;
    private float defaultPointerLength = 20f;
    private NotePadController focusPad;
	// Chord note pads after root (focus).
	private List<NotePadController> additionalPads;
	public List<int> chordStructure;
	private Color pointerAlbedo;
	private Color pointerEmission;
	private Color highlighted;
	private Color triggered;

    public enum Hand { Left, Right};

    // Use this for initialization
    void Start()
    {
		// Calculate colors from base color.
		pointerAlbedo = ApplySVA(baseColor, 1, 0.5f, 0.5f);
		pointerEmission = ApplySVA(baseColor, 1, 0.75f);
		highlighted = ApplySVA(baseColor, 1, 0.125f);
		triggered = ApplySVA(baseColor, 1, 0.25f);

		additionalPads = new List<NotePadController>();
        pointer = Instantiate(pointerPrefab);
        pointer.transform.SetParent(transform);
        pointer.transform.localPosition = Vector3.zero;
        pointer.transform.localRotation = Quaternion.identity;
		pointer.GetComponent<MeshRenderer>().material.SetColor("_Color", pointerAlbedo);
		pointer.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", pointerEmission);
        touchControls = new TouchControls(hand);
        SetPointerEnable(false);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(touchControls.handTriggerButton))
        {
            SetPointerEnable(true);
        }
        if (OVRInput.GetUp(touchControls.handTriggerButton))
        {
            SetPointerEnable(false);
        }
        // If we are pointing...
        if (pointerEnabled)
        {
            float pointerLength = defaultPointerLength;
            RaycastHit hitInfo;
            Vector3 pointDirection = transform.forward;
			bool pointerLocked = pointerLockEnabled && OVRInput.Get(touchControls.indexTriggerButton) && focusPad != null;
            if (pointerLocked)
            {
				pointDirection = focusPad.transform.position - transform.position;
            }
            pointer.transform.forward = pointDirection;
            // If are pointing at something...
            if (Physics.Raycast(transform.position, pointDirection, out hitInfo))
            {
                pointerLength = (hitInfo.point - transform.position).magnitude;
                // If we are pointing at something different than last frame...
				if (focusPad != hitInfo.collider.GetComponent <NotePadController>())
                {
                    // If we are not holding index trigger down...
                    if (!OVRInput.Get(touchControls.indexTriggerButton))
                    {
                        SwitchFocus(hitInfo.collider.GetComponent <NotePadController>());
                    }
                }
            }
			// If we are pointing at nothing...
			else
            {
                // If we are not holding index trigger down...
                if (!OVRInput.Get(touchControls.indexTriggerButton))
                {
                    SwitchFocus(null);
                }
            }
            pointer.transform.localScale = new Vector3(1, 1, pointerLength);

            // If we are focused on a note pad...
			if (focusPad != null)
            {	
                // Register any plays or releases.
                if (OVRInput.GetDown(touchControls.indexTriggerButton))
                {
					focusPad.RegisterPlay(127, gameObject);
					for(int i = 0; i < additionalPads.Count; i++){
						additionalPads[i].RegisterPlay(127, gameObject);
					}
                }
                else if (OVRInput.GetUp(touchControls.indexTriggerButton))
                {
					focusPad.RegisterRelease(127, gameObject);
					for(int i = 0; i < additionalPads.Count; i++){
						additionalPads[i].RegisterRelease(127, gameObject);
					}
                }
            }
        }
		// If we are not pointing...
		else
        {
            // If we are pointing at something
			if (focusPad != null)
            {
                SwitchFocus(null);
            }
        }
    }

    private void SwitchFocus(NotePadController newFocus)
    {
        // If another note pad had our focus...
		if (focusPad != null)
        {
			focusPad.RegisterFocusLoss(gameObject);
			for(int i = 0; i < additionalPads.Count; i++){
				additionalPads[i].RegisterFocusLoss(gameObject);
			}
        }
        // If our new focus is a note pad...
        if (newFocus != null)
        {
            newFocus.RegisterFocus(gameObject);

			// Set additional chord notes based on structure offsets.
			additionalPads.Clear();
			int rootNoteSourceIndex = newFocus.noteSourceIndex;
			for(int i = 0; i < chordStructure.Count; i++){
				int chordNoteSourceIndex = rootNoteSourceIndex + chordStructure[i];
				if(chordNoteSourceIndex >= 0 && chordNoteSourceIndex < newFocus.instrument.noteSources.Length){
					NoteSource chordNoteSource = newFocus.instrument.noteSources[chordNoteSourceIndex];
					additionalPads.Add(chordNoteSource.GetComponent<NotePadController>());
					additionalPads[i].RegisterFocus(gameObject);
				}
			}
        }
		focusPad = newFocus;
    }

    private void SetPointerEnable(bool enabled)
    {
        pointer.GetComponent<MeshRenderer>().enabled = enabled;
        pointerEnabled = enabled;
    }

	private Color ApplySVA(Color color, float overrideS, float overrideV, float alpha = 1){
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		Color modified = Color.HSVToRGB(h, overrideS, overrideV);
		modified.a = alpha;
		return modified;
	}

	public Color GetHighlightColor(NotePadController.State state){
		switch(state){
			case NotePadController.State.Selected:
				return highlighted;
			case NotePadController.State.Triggered:
				return triggered;
			default:
				return Color.black;
		}
	}

    private class TouchControls
    {
        public readonly OVRInput.Axis1D indexTrigger;
        public readonly OVRInput.Button indexTriggerButton;
        public readonly OVRInput.Axis1D handTrigger;
        public readonly OVRInput.Button handTriggerButton;
        public readonly OVRInput.Axis2D stick;

        public TouchControls(Hand hand)
        {
            if (hand == Hand.Right)
            {
                indexTrigger = OVRInput.Axis1D.SecondaryIndexTrigger;
                indexTriggerButton = OVRInput.Button.SecondaryIndexTrigger;
                handTrigger = OVRInput.Axis1D.SecondaryHandTrigger;
                handTriggerButton = OVRInput.Button.SecondaryHandTrigger;
                stick = OVRInput.Axis2D.SecondaryThumbstick;
            }
            else
            {
                indexTrigger = OVRInput.Axis1D.PrimaryIndexTrigger;
                indexTriggerButton = OVRInput.Button.PrimaryIndexTrigger;
                handTrigger = OVRInput.Axis1D.PrimaryHandTrigger;
                handTriggerButton = OVRInput.Button.PrimaryHandTrigger;
                stick = OVRInput.Axis2D.PrimaryThumbstick;
            }
        }
    }
}
