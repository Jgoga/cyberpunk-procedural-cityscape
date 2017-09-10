using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Mathf.Abs( Input.GetAxis ("Horizontal"))>0)
			transform.Translate (Input.GetAxis ("Horizontal")/5, 0, 0);

		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
	}
}
