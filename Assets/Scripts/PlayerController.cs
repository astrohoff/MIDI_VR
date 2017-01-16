using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class PlayerController : MonoBehaviour {
    public float normalMoveSpeed = 1;
    public float dashMultiplier = 1.5f;
    public float dashThreshold = 0.5f;
    float snapTurnSize = 45;
    public Transform leftHandAnchor;
    private CharacterController characterController;

	// Use this for initialization
	void Start () {
        characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        // Recenter.
        bool recenter1 = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) && OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        bool recenter2 = OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) && OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        if (recenter1 || recenter2)
        {
            InputTracking.Recenter();
        }
        // Movement.
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 moveForwardAxis = leftHandAnchor.forward;
        moveForwardAxis.y = 0;
        moveForwardAxis.Normalize();
        Vector3 moveForward = moveForwardAxis * moveInput.y;
        Vector3 moveRightAxis = leftHandAnchor.right;
        moveRightAxis.y = 0;
        moveRightAxis.Normalize();
        Vector3 moveRight = moveRightAxis * moveInput.x;
        float moveSpeed = normalMoveSpeed;
        if(moveInput.magnitude > dashThreshold)
        {
            moveSpeed *= dashMultiplier;
        }
        Vector3 movement = (moveForward + moveRight).normalized * moveSpeed;
        characterController.SimpleMove(movement);
        // Rotation.
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft))
        {
            transform.Rotate(transform.up, -snapTurnSize);
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight))
        {
            transform.Rotate(transform.up, snapTurnSize);
        }
	}
}
