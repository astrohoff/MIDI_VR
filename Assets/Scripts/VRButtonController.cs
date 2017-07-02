using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRButtonController : MonoBehaviour {
	private Button button;
	private int framesSinceFocusRegister = 90;
	private bool hadFocus = false;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button> ();
		StartCoroutine (AddColliderRoutine());
	}
		
	// Update is called once per frame
	void Update () {
		if (framesSinceFocusRegister == 0 && !hadFocus) {
			OnRecieveFocus ();
			hadFocus = true;
		} else if (framesSinceFocusRegister > 2) {
			OnLostFocus ();
			hadFocus = false;
		}
		framesSinceFocusRegister++;
	}

	public void RegisterFocus(){
		framesSinceFocusRegister = 0;
	}

	public void RegisterClick(){
		button.onClick.Invoke ();
	}

	private void OnRecieveFocus(){
		button.Select ();
	}

	private void OnLostFocus(){
		if (EventSystem.current.currentSelectedGameObject == gameObject) {
			EventSystem.current.SetSelectedGameObject (null);
		}
	}

	private IEnumerator AddColliderRoutine(){
		yield return null;
		RectTransform rt = GetComponent<RectTransform> ();
		BoxCollider bc = gameObject.AddComponent<BoxCollider> ();
		Vector3 size = new Vector3 (rt.rect.width, rt.rect.height, 0.01f);
		bc.size = size;
	}
}
