using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonController : MonoBehaviour {
	public Transform tipAnchor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		if (Physics.Raycast (tipAnchor.position, tipAnchor.forward, out hitInfo)) {
			VRButtonController vrbc = hitInfo.collider.GetComponent<VRButtonController> ();
			if (vrbc != null) {
				vrbc.RegisterFocus ();
				if (OVRInput.GetDown (OVRInput.Button.Two)) {
					vrbc.RegisterClick ();
				}
			}
		}
	}

}
