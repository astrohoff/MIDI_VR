using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarController : MonoBehaviour {
    public Transform guitarBodyAnchor, neckAnchor, playerBodyAnchor, frettingHandAnchor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        guitarBodyAnchor.position = playerBodyAnchor.position;
        /*Vector3 oldDirection = neckAnchor.position - guitarBodyAnchor.position;
        Vector3 newDirection = frettingHandAnchor.position - guitarBodyAnchor.position;
        Quaternion guitarRotation = Quaternion.FromToRotation(oldDirection, newDirection);
        guitarBodyAnchor.rotation *= guitarRotation;*/
        guitarBodyAnchor.LookAt(frettingHandAnchor);
	}
}
