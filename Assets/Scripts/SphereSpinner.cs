using UnityEngine;
using System.Collections;

public class SphereSpinner : MonoBehaviour {

	Rigidbody myrigidbody;

	// Use this for initialization
	void Start () {
	
		myrigidbody = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.Rotate(0, 0.01f, 0);
		
	}
}
