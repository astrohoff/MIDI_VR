using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManager : MonoBehaviour {
    public float speed;
    private Vector3 oldPosition;
    private Transform tip;
	// Use this for initialization
	void Start () {
        tip = transform.GetChild(0);
        oldPosition = tip.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position != oldPosition)
        {
            speed = (transform.position - oldPosition).magnitude / Time.deltaTime;
        }
        oldPosition = transform.position;
	}
}
