using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHitPlayer : MonoBehaviour {
    public float ringDurration = 5f;
    private NoteSource noteSource;

	// Use this for initialization
	void Start () {
        noteSource = GetComponent<NoteSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision c)
    {
        if(c.collider.tag == "Striker")
        {
            noteSource.Play(127, 127, ringDurration);
        }
    }
}
