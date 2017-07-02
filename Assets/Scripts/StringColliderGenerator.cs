using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringColliderGenerator : MonoBehaviour {
    public Transform stringStart, stringEnd;
	public VelocityManager strikerVelocityManager;
    public float diameter = 0.002f;
	private GameObject colliderObj;
	private BoxCollider collider;
	void Start () {
        colliderObj = new GameObject("Collider");
        Vector3 midPoint = (stringStart.position + stringEnd.position) / 2;
        colliderObj.transform.position = midPoint;
        colliderObj.transform.LookAt(stringEnd);
        collider = colliderObj.AddComponent<BoxCollider>();
        float length = Vector3.Distance(stringStart.position, stringEnd.position);
        collider.size = new Vector3(diameter, diameter, length);
        colliderObj.transform.parent = transform;
    }

	private void FixedUpdate(){
		float spatialResolution = strikerVelocityManager.speed * Time.fixedDeltaTime;
		Vector3 colliderSize = collider.size;
		if (diameter < spatialResolution) {			
			float safeDiameter = spatialResolution;
			colliderSize.x = safeDiameter;
			colliderSize.y = safeDiameter;
			collider.size = colliderSize;
		} else {
			colliderSize.x = diameter;
			colliderSize.y = diameter;
			collider.size = colliderSize;
		}
	}
}
