using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BustAI : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
	}

	// class Context {

	// 	List<Condition> conditions = new List<Condition>();
	// 	public Rigidbody myrigidbody;

	// 	public Context(Rigidbody rigidbody){
	// 		// this.myrigidbody = rigidbody;
	// 	}

	// 	public Rigidbody getRigidBody(){
	// 		return myrigidbody;
	// 	}

	// 	// public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force){
	// 	// 	myrigidbody.AddForce(transform.forward * 500f );
	// 	// }

	// }

	class Condition {

		public string name;
		public string value;

		public Condition(string name, string value){
			this.name = name;
			this.value = value;
		}
	}

	// class Action {

	// 	public Action(){
	// 	}

	// 	public virtual void doAction(Context context){
	// 		Debug.Log("empty action");
	// 	}
	// }

	// class MoveForwardAction : Action {

	// 	public MoveForwardAction(){
	// 	}

	// 	public override void doAction(Context context){
	// 		Debug.Log("move forward action");

	// 		Vector3 velocity = new Vector3();
	// 		velocity.x = 15f;
	// 		// Rigidbody r = context.getRigidBody();
	// 		context.myrigidbody.AddForce(transform.forward * 500f );
	// 		// context.AddForce(transform.forward * 500f );
	// 	}
	// }

	class Node {

    	public List<Node> parentNodes = new List<Node>();
    	public List<Node> childNodes = new List<Node>();
    	// Context context;

    	// List<Condition> context = new List<Condition>();


    	//intensity is your +- effect on chain.
    	float intensity;

    	// public Action action;
    	public string action;
    	public IDictionary<string, bool> requirements;

    	public string toString(){
    		string childNodeStrings = "";
			foreach (Node child in childNodes){
				childNodeStrings += child.toString();
			}
			return JsonUtility.ToJson(this)+childNodeStrings;
    	}

    	public List<string> getStringList(List<string> stringList){
    		stringList.Add("intensity");
    		stringList.Add(intensity.ToString("R"));
    		stringList.Add("action");
    		stringList.Add(action);
    		stringList.Add("requirements");
				foreach(KeyValuePair<string, bool> requirement in requirements){
	    			stringList.Add(requirement.Key);
	    			stringList.Add(requirement.Value.ToString());
				}
    		stringList.Add("childNodes");
			foreach (Node child in childNodes){
				stringList = child.getStringList(stringList);
			}
			return stringList;
    	}

		public Node(float intensity, string action, IDictionary<string, bool> requirements){

			this.intensity = intensity;
			this.action = action;
			this.requirements = requirements;
		}

		public void addChild(Node child){
			childNodes.Add(child);
		}

		public Node getNextActionNode(IDictionary<string, bool> conditions, bool skipThisAction){

			// Debug.Log(action);
			// Debug.Log(skipThisAction);
			
			// foreach(KeyValuePair<string, bool> condition in conditions)
			// {
			// 	// Debug.Log("condition: "+condition.Key);
			// }
			// Debug.Log(conditions.Count);

			// foreach(KeyValuePair<string, bool> requirement in requirements)
			// {
			// 	// Debug.Log("requirement: "+requirement.Key);
			// }
			// Debug.Log(requirements.Count);

			if (action != null && skipThisAction == false){

				// Debug.Log("checking requirements");
				bool metRequirements = true;
				foreach(KeyValuePair<string, bool> requirement in requirements)
				{
					if (conditions.ContainsKey(requirement.Key) && conditions[requirement.Key] == requirement.Value){
					} else {
						metRequirements = false;
					}
				}
				if (metRequirements == true){
					// Debug.Log("returning action");
					return this;//.doAction(context);
				}

			} 

			if (childNodes.Count > 0){

				foreach (Node child in childNodes){

					Node nextActionNode = child.getNextActionNode(conditions, false);
					if (nextActionNode != null){
						return nextActionNode;
					}

					// if (requirements.Count == 0){
						
					// 	Debug.Log("no requirements needed");

					// 	return child.getNextActionNode(conditions, false);

					// } else {
					// 	Debug.Log("checking requirements");
					// 	foreach(KeyValuePair<string, bool> requirement in requirements)
					// 	{

					// 		if (conditions.ContainsKey(requirement.Key) && conditions[requirement.Key] == requirement.Value){
					// 			Debug.Log("requirement satisfied: "+requirement.Key);
					// 			Debug.Log("required value");
					// 			Debug.Log(requirement.Value);
					// 			Debug.Log("given value");
					// 			Debug.Log(conditions[requirement.Key]);

					// 			return child.getNextActionNode(conditions, false);
					// 		}
					// 	}
					// }
				}
			}
			// } else {

				return null;
			// }


			// if (action != null){
			// 	return this;//action.doAction(context);
			// } else {
			// 	return null;//childnodes...
			// }
		}
	}

	Node rootNode;
	Node currentNode;

	public string currentAction = "waitAction";

	public bool wallInfrontSense = false;

	// List<Condition> conditions = new List<Condition>();
	IDictionary<string, string> boolConditionNames = new Dictionary<string, string>();
	public IDictionary<string, bool> boolConditions = new Dictionary<string, bool>();

	// public bool isThinking = false;

	// Context context;
	Rigidbody myrigidbody;

	void Start () {
		
		myrigidbody = GetComponent<Rigidbody>();

		// context = new Context(myrigidbody);
		// Rigidbody myrigidbody = GetComponent<Rigidbody>();
		// StartCoroutine(getNextAction()); //old system

		rootNode = new Node(1f, null, new Dictionary<string, bool>());
		rootNode.addChild(new Node(1f, "turnRightAction", new Dictionary<string, bool>(){{"wallInfront", true}}));
		rootNode.addChild(new Node(1f, "MoveForwardAction", new Dictionary<string, bool>()));
		currentNode = rootNode;

		// Debug.Log(String.Join("\n", rootNode.getStringList(new List<string>())));
		// rootNode.getStringList(new List<string>()).ForEach(Debug.Log);

		List<string> newNodeString = new List<string>();
		string line;
		System.IO.StreamReader file = 
		   new System.IO.StreamReader("Assets\\exampledna.txt");
		while((line = file.ReadLine()) != null)
		{	
			newNodeString.Add(line);
		   // Debug.Log (line);
		   // counter++;
		}
		file.Close();

		// StartCoroutine(wallInfrontSensor);
		StartCoroutine(beatHeart());

	}

	IEnumerator beatHeart(){
		// Debug.Log("beatHeart");
		
		// bool hasEndingNode = true;
		// Node endingNode;
		// while (hasEndingNode == true){

		RaycastHit objectHit;        
		if (Physics.Raycast(transform.position, transform.forward, out objectHit, 50)) {

			// Debug.Log("Raycast hitted to: " + objectHit.collider);
	        // print("Distance to other: " + objectHit.distance);

	        if (objectHit.distance < 20f){
	        	boolConditions["wallInfront"] = true;

	        } else {
	        	boolConditions["wallInfront"] = false;
	        }
		} else {
        	boolConditions["wallInfront"] = false;
        }
        wallInfrontSense = boolConditions["wallInfront"];

		// Debug.Log("start");
		currentNode = rootNode;

		Node nextNode = currentNode.getNextActionNode(boolConditions, false);

		// // if (currentNode == null){
		// // 	currentNode = rootNode;
		// // }
		// currentNode = nextNode;

		// if (nextNode != null && nextNode.action != null){
			// Debug.Log("final action: "+nextNode.action);
			if (nextNode.action == "MoveForwardAction"){

				myrigidbody.AddForce(transform.forward * 120f );
			} else if (nextNode.action == "turnRightAction"){

				transform.Rotate(0, 45, 0);
			}
			currentNode = nextNode;
		// }

		// if (currentNode == null){
		// 	currentNode = rootNode;
		// }
		// }

   		yield return new WaitForSeconds(0.2f);
		StartCoroutine(beatHeart());
	}

	// IEnumerator getNextAction(){
	// 	Debug.Log("getNextAction");
	// 	StartCoroutine(wallInfrontSensor());
 //   		yield return new WaitForSeconds(0.2f);

	// 	switch (currentAction) {
	// 		case "waitAction":
	// 			if (wallInfrontSense == true){
	// 				StartCoroutine(turnRightAction());
	// 			} else {
	// 				StartCoroutine(goForwardAction());
	// 			}
	// 			break;

	// 		case "goForwardAction":
	// 			StartCoroutine(waitAction());
	// 			break;

	// 		case "turnRightAction":
	// 			StartCoroutine(waitAction());
	// 			break;

	// 	}
	// }

 //   IEnumerator waitAction(){
 //   		yield return new WaitForSeconds(0.1f);
 //   		currentAction = "waitAction";

	// 	yield return new WaitForSeconds(1f);
	// 	StartCoroutine(getNextAction());
	// }

 //   IEnumerator goForwardAction(){
 //   		yield return new WaitForSeconds(0.1f);
 //   		currentAction = "goForwardAction";

	// 	Vector3 velocity = new Vector3();
	// 	velocity.x = 15f;
	// 	// myrigidbody.AddForce(transform.forward * 500f );

	// 	StartCoroutine(getNextAction());

	// }

 //   IEnumerator turnRightAction(){
 //   		yield return new WaitForSeconds(0.1f);
 //   		currentAction = "turnRightAction";

	// 	transform.Rotate(0, 90, 0);

	// 	StartCoroutine(getNextAction());

	// }

 //   IEnumerator wallInfrontSensor(){
 //   		yield return new WaitForSeconds(0.1f);
	// 		// Debug.Log("wallInfrontSensor");

	// 	// Vector3 fwd = transform.TransformDirection(Vector3.forward);
	// 	// Debug.DrawRay(transform.position, fwd * 50, Color.green);

	// 	RaycastHit objectHit;        
	// 	// Shoot raycast
	// 	if (Physics.Raycast(transform.position, transform.forward, out objectHit, 50)) {
	// 		Debug.Log("Raycast hitted to: " + objectHit.collider);
	// 		// targetEnemy = objectHit.collider.gameObject;
	//         // float dist = Vector3.Distance(objectHit.transform.position, transform.position);
	//         print("Distance to other: " + objectHit.distance);
	//         if (objectHit.distance < 5f){
	//         	wallInfrontSense = true;
	//         } else {
	//         	wallInfrontSense = false;
	//         }
	// 	} else {
 //        	wallInfrontSense = false;
 //        }

	// 	StartCoroutine(wallInfrontSensor());

	// }
}