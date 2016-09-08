using UnityEngine;
using System.Collections;

public class SphereSpinner : MonoBehaviour {

	Rigidbody myrigidbody;

	// Use this for initialization
	void Start () {
	
		myrigidbody = GetComponent<Rigidbody>();

		StartCoroutine(rotatethatshit());
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	IEnumerator rotatethatshit(){
   		yield return new WaitForSeconds(0.1f);
		transform.Rotate(0, 0.1f, 0);
		StartCoroutine(rotatethatshit());
	}
}
