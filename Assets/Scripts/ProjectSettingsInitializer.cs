using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSettingsInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Time.fixedDeltaTime = 1.0f / Screen.currentResolution.refreshRate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
