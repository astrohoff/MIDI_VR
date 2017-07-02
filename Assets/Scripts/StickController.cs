using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour {
    public Transform anchor;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	private void FixedUpdate()
    {
        rb.MovePosition(anchor.position);
        rb.MoveRotation(anchor.rotation);
    }
}
