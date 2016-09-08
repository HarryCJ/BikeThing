using UnityEngine;
using System.Collections;

public class GoodBallScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    void OnTriggerStay(Collider other)
    {

		GameObject go = other.gameObject;
		BustAI bAI = go.GetComponent<BustAI>();
		bAI.reward();
    }

	// Set up a list to keep track of targets
	// public System.Collections.Generic.List<GameObject> targets = new System.Collections.Generic.List<GameObject>();

	// If a new enemy enters the trigger, add it to the list of targets
	void OnTriggerEnter(Collider other){


		// if (other.tag.Contains("phys") || other.tag.Contains("env")){

		// 	GameObject go = other.gameObject;
		// 	if(!targets.Contains(go)){

		// 		targets.Add(go);
		// 		parent.setGrounded(true);
		// 		// parent.isGrounded = true;
		// 		parent.isBoosting = false;

		//         if(other.tag.Contains("enemy")){

		// 			Enemy myEnemy = go.GetComponent<Enemy>();
		// 			if (myEnemy.isBouncy == true){

		// 				parent.tryBoost(myEnemy.jumpLevels[parent.environment_ws.upgrades[myEnemy.enemyType+"_boost"]], myEnemy.addToMultiplier);
		// 			}
		//         } else if (other.tag.Contains("env")){

		// 			parent.resetBoostCombo();
		// 		}
		// 	}
	 //    }

	}

	// When an enemy exits the trigger, remove it from the list
	void OnTriggerExit(Collider other){
	// 	if (other.tag.Contains("phys") || other.tag.Contains("env")){
	// 		GameObject go = other.gameObject;
	// 		if(targets.Contains(go)){
	// 			targets.Remove(go);

	// 			targets.RemoveAll(item => item == null);
	// 			if (targets.Count == 0){
	// 				parent.setGrounded(false);
	// 				// parent.isGrounded = false;
	// 			}
	// 		}
	//     }
	}
}
