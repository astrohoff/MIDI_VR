using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringColliderGenerator : MonoBehaviour {
    public Transform stringStart, stringEnd;
    public float diameter = 0.002f;

	void Start () {
        GameObject colliderObj = new GameObject("Collider");
        Vector3 midPoint = (stringStart.position + stringEnd.position) / 2;
        colliderObj.transform.position = midPoint;
        colliderObj.transform.LookAt(stringEnd);
        BoxCollider collider = colliderObj.AddComponent<BoxCollider>();
        float length = Vector3.Distance(stringStart.position, stringEnd.position);
        collider.size = new Vector3(diameter, diameter, length);
        colliderObj.transform.parent = transform;
    }
}
