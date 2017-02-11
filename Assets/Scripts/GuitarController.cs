using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarController : MonoBehaviour {
    public Transform guitarBodyAnchor, neckAnchor, playerBodyAnchor, frettingHandAnchor;
    private bool updatePos = true;
	// Use this for initialization
	void Start () {

        StartCoroutine(DelayedPosSet());
	}
	
	// Update is called once per frame
	void Update () {
        if (updatePos)
        {
            guitarBodyAnchor.position = playerBodyAnchor.position;

        }
        /*Vector3 oldDirection = neckAnchor.position - guitarBodyAnchor.position;
        Vector3 newDirection = frettingHandAnchor.position - guitarBodyAnchor.position;
        Quaternion guitarRotation = Quaternion.FromToRotation(oldDirection, newDirection);
        guitarBodyAnchor.rotation *= guitarRotation;*/
        guitarBodyAnchor.LookAt(frettingHandAnchor);
	}

    private IEnumerator DelayedPosSet()
    {
        yield return new WaitForSeconds(5);
        updatePos = false;
    }
}
