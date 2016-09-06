using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BustAI : MonoBehaviour {

	 // class ScreenCapture : MonoBehaviour
	 // {
	 //     public RenderTexture overviewTexture;
	 //     GameObject OVcamera;
	 //     public string path = "test1.png";
	 //     Camera camOV;

		// public ScreenCapture(GameObject g){

		// 	// Debug.Log("Node bein' made yo");
		// 	// this.camOV = bustCamera;
		// 	OVcamera = g;
		// }
	 
	 //     // void Start()
	 //     // {
	 //     //     OVcamera = GameObject.FindGameObjectWithTag("OverviewCamera");
	 //     // }
	 
	 //     // void LateUpdate()
	 //     // {
	         
	 //     //     if (Input.GetKeyDown("f9"))
	 //     //     {
	 //     //         StartCoroutine(TakeScreenShot());
	 //     //     }
	 
	 //     // }
	 
	 //     // return file name
	 //     string fileName(int width, int height)
	 //     {
	 //        return string.Format("screen_{0}x{1}_{2}.png",
	 //                              width, height,
	 //                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	 //     }
	 
	 //     public IEnumerator TakeScreenShot(Camera g)
	 //     {
	 //         yield return new WaitForEndOfFrame();
	 
	 //         camOV = g;
	 
	 //         RenderTexture currentRT = RenderTexture.active;
	 
	 //         RenderTexture.active = camOV.targetTexture;
	 //         camOV.Render();
	 //         Debug.Log(camOV.targetTexture);
	 //         Texture2D imageOverview = new Texture2D(camOV.targetTexture.width, camOV.targetTexture.height, TextureFormat.RGB24, false);
	 //         imageOverview.ReadPixels(new Rect(0, 0, camOV.targetTexture.width, camOV.targetTexture.height), 0, 0);
	 //         imageOverview.Apply();
	 //         RenderTexture.active = currentRT;
	 
	         
	 //         // Encode texture into PNG
	 //         byte[] bytes = imageOverview.EncodeToPNG();
	 
	 //         // save in memory
	 //         string filename = fileName(Convert.ToInt32(imageOverview.width), Convert.ToInt32(imageOverview.height));
	 //         path = Application.persistentDataPath + "/Snapshots/" + filename;
	        
	 //         System.IO.File.WriteAllBytes(path, bytes);
	 //     }
	 // }

	class Condition {

		public string name;
		public string value;

		public Condition(string name, string value){
			this.name = name;
			this.value = value;
		}
	}

	class Node {

    	public List<Node> parentNodes = new List<Node>();
    	public List<Node> childNodes = new List<Node>();


    	//intensity is your +- effect on chain.
    	float intensity;

    	// public Action action;
    	public string action;
    	public IDictionary<string, bool> boolRequirements;
    	public Dictionary<string, int> intVariables;
    	public int id;
    	public int points = 0;
    	public int timer = 1;

    	public string toString(){
    		string childNodeStrings = "";
			foreach (Node child in childNodes){
				childNodeStrings += child.toString();
			}
			return JsonUtility.ToJson(this)+childNodeStrings;
    	}

    	public List<string> getStringList(List<string> stringList){
    		stringList.Add("+nodeStart");
    		// stringList.Add("intensity");
    		// stringList.Add(intensity.ToString("R"));
    		stringList.Add("action");
    		stringList.Add(action);
    		stringList.Add("boolRequirements");
			foreach(KeyValuePair<string, bool> requirement in boolRequirements){
    			stringList.Add(requirement.Key);
    			stringList.Add(requirement.Value.ToString());
			}
    		stringList.Add("intVariables");
			foreach(KeyValuePair<string, int> variable in intVariables){
    			stringList.Add(variable.Key);
    			stringList.Add(variable.Value.ToString());
			}
    		stringList.Add("childNodes");
    		stringList.Add(childNodes.Count.ToString());
			foreach (Node child in childNodes){
				stringList = child.getStringList(stringList);

			}
    		stringList.Add("-nodeEnd");
			return stringList;
    	}

		public Node(float intensity, string action, IDictionary<string, bool> boolRequirements, Dictionary<string, int> intVariables){

			// Debug.Log("Node bein' made yo");
			this.intensity = intensity;
			this.action = action;
			this.boolRequirements = boolRequirements;
			this.intVariables = intVariables;

			// this.id;// = UnityEngine.Random.Range(1000, 9999);
		}

		public int readStringList(List<string> stringList, int pointer){
			id = pointer;
			// Debug.Log("readStringList");

			string lastLine = null;
			string currentR = null;
			// Node newNode = null;
			// IDictionary<string, bool> s = new IDictionary<string, bool>();
			string line;
			for (int x  = pointer; x < stringList.Count; x++){
				line = stringList[x];
				// Debug.Log(line);

				if (line == "nodeStart") {

					Node newNode = new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>());
					// Debug.Log("sending: "+(x+1).ToString());
					int y = newNode.readStringList(stringList, ++x);
					x = y;
					childNodes.Add(newNode);

				} else if (line == "nodeEnd"){

					// Debug.Log("returning: "+(x).ToString());
					return x;
				} else if (line == "intensity" ||
					line == "action" ||
					line == "boolRequirements" ||
					line == "intVariables"){
					lastLine = line;
					//stops trawling through for data.

				} else if (lastLine == "intensity" ||
					lastLine == "action" ||
					lastLine == "boolRequirements" ||
					lastLine == "intVariables"){
					// stringList.RemoveAt(0);

					if (lastLine == "intensity"){
						float val = 1f;
						try {
							val = float.Parse(line);
						} catch {}
						this.intensity = val;
						// lastLine = null;

					} else if (lastLine == "action"){
						this.action = line;
						// lastLine = null;

					} else if (lastLine == "boolRequirements"){
						if (currentR != null){
							if (boolRequirements.ContainsKey(currentR)){
								if (line == "True"){
									boolRequirements[currentR] = true;
								} else {
									boolRequirements[currentR] = false;
								}
							} else {
								if (line == "True"){
									boolRequirements.Add(currentR, true);
								} else {
									boolRequirements.Add(currentR, false);
								}
							}
							currentR = null;
						} else {
							currentR = line;
						}

					} else if (lastLine == "intVariables"){
						if (currentR != null){
							int val = 0;
							try {
								val = int.Parse(line);
							} catch {}
							try {
								intVariables.Add(currentR, val);
							} catch {}
							currentR = null;
						} else {
							currentR = line;
						}
					} 

					// else if (lastLine == "childNodes"){

					// 	// Debug.Log
					// 	// newNode = new Node(1f, null, new Dictionary<string, bool>());
					// 	// List<string> remainder = newNode.readStringList(stringList);
						
					// }

				}

			}

			return 999;
		}

		public void addChild(Node child){
			childNodes.Add(child);
		}

		public Node getNextActionNode(IDictionary<string, bool> conditions, Dictionary<string, int> currentIntVariables, bool skipThisAction){

			// Debug.Log("id");
			// Debug.Log(id);
			// Debug.Log("action");
			// Debug.Log(action);
			// Debug.Log("currentIntVariables");
			// foreach(KeyValuePair<string, int> variable in currentIntVariables){
			// 	// currentIntVariables[variable.Key] = variable.Value;
			// 	Debug.Log(variable.Key);
			// 	Debug.Log(variable.Value);
			// }
			// Debug.Log("intVariables");
			// foreach(KeyValuePair<string, int> variable in intVariables){
			// 	Debug.Log(variable.Key);
			// 	Debug.Log(variable.Value);
			// }
			// if (action == null){
			// 	Debug.Log("is null");
			// }
			// if (action == "Null"){
			// 	Debug.Log("is null string");
			// Dictionary<string, int> currentIntVariables = new Dictionary<string, int>();

			// }
			if (action != null && action != "Null" && skipThisAction == false){

				if (action == "setVariables"){ //Adds node's variables to global variable list

					foreach(KeyValuePair<string, int> variable in intVariables){
						currentIntVariables[variable.Key] = variable.Value;
					}

				} else {

					// Debug.Log("checking boolRequirements");
					bool metRequirements = true;
					foreach(KeyValuePair<string, bool> requirement in boolRequirements)
					{	
						// Debug.Log(requirement.Key);
						// Debug.Log(requirement.Value);

						if (conditions.ContainsKey(requirement.Key) && conditions[requirement.Key] == requirement.Value){
						} else {
							metRequirements = false;
						}
					}
					if (metRequirements == true){
						// Debug.Log("boolRequirements met");
						//combine variables
						foreach(KeyValuePair<string, int> variable in currentIntVariables){
							intVariables[variable.Key] = variable.Value;
						}

						return this;//.doAction(context);
					} else {
						// Debug.Log("boolRequirements failed");
					}
				}

			} 

			if (childNodes.Count > 0){

				foreach (Node child in childNodes){

					// Debug.Log("currentIntVariables2");
					// foreach(KeyValuePair<string, int> variable in currentIntVariables){
					// 	// currentIntVariables[variable.Key] = variable.Value;
					// 	Debug.Log(variable.Key);
					// 	Debug.Log(variable.Value);
					// }
					// if (ac
					Node nextActionNode = child.getNextActionNode(conditions, currentIntVariables, false);
					if (nextActionNode != null){
						return nextActionNode;
					}
				}
			}

			return null;
		}
	}

	// Node alphaNode;
	// Node omegaNode;
	public int currentNode;

	// public string currentAction = "waitAction";

	public bool wallInfrontSense = false;

	IDictionary<string, string> boolConditionNames = new Dictionary<string, string>();
	public IDictionary<string, bool> boolConditions = new Dictionary<string, bool>();
	public IDictionary<string, float> floatConditions = new Dictionary<string, float>();

	Rigidbody myrigidbody;
	// ScreenCapture sc;
	int d;
	public int nodeCounter = 0;
	public bool isRewarded = false;
	public bool isPunished = false;

	public int points1 = 0;
	public int points2 = 0;
	public int points3 = 0;
	public int points4 = 0;
	public int points5 = 0;

	public int timer1 = 0;
	public int timer2 = 0;
	public int timer3 = 0;
	public int timer4 = 0;
	public int timer5 = 0;
	List<Node> nodePopulation = new List<Node>();

	CameraScreenGrab csg;
	GameObject mainCamera;
	Camera mainCameraC;

	void Start () {
		
		myrigidbody = GetComponent<Rigidbody>();
		mainCamera = GameObject.Find("Main Camera");
		mainCameraC = mainCamera.GetComponent<Camera>();

        foreach (Transform child in transform){
        	// Debug.Log("child.name");
        	Debug.Log(child.name);
            if (child.name == "camera"){
        		// Debug.Log("got it");
                 csg = child.GetComponent<CameraScreenGrab>();
            }
        }

		for (int x = 0; x < 5; x++){
			nodePopulation.Add(new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>()));	
		}
		// Node alphaNode = new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>());
		// Node omegaNode = new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>());
		// alphaNode.addChild(new Node(1f, "turnRightAction", new Dictionary<string, bool>(){{"wallInfront", true}}));
		// alphaNode.addChild(new Node(1f, "MoveForwardAction", new Dictionary<string, bool>()));
		List<string> newNodeStringList = nodeStringLoader("Assets\\exampledna.txt");

		for (int x = 0; x < 5; x++){
			nodePopulation[x].readStringList(newNodeStringList, 0);
		}

    	// Debug.Log("nodePopulation");
    	// Debug.Log(nodePopulation.Count);

		// d = alphaNode.readStringList(newNodeStringList, 0);
		// d = omegaNode.readStringList(newNodeStringList, 0);

		// nodePopulation.Add(alphaNode);
		// nodePopulation.Add(omegaNode);


		currentNode = 0;

		breed();

		// Debug.Log("printing start");
		// List<string> fullCycleSL = alphaNode.getStringList(new List<string>());
		// foreach (string s in fullCycleSL){
		// 	Debug.Log(s);
		// }
		// Debug.Log("printing end");

		// sc = new ScreenCapture(transform.gameObject);

		StartCoroutine(beatHeart());
		StartCoroutine(breedingSeason());

	}
	
	void Update () {

	}

	public List<string> makeChild(List<string> loserDNA, List<string> winnerDNA, List<string> childDNA, int favour){

		// List<string> loserDNA = loserNode.getStringList(new List<string>());
		// List<string> winnerDNA = winnerNode.getStringList(new List<string>());

		// // List<bool> selectMotherMap = new List<bool>();
		// List<string> childDNA = new List<string>();

		// int length = UnityEngine.Random.Range(0, loserDNA.Count);
		// int start = UnityEngine.Random.Range(0, loserDNA.Count);

		// for (int x = start; start < loseDNA.Count; x++){

		// 	loseDNA[x] = winnerDNA
		// 	if (x - start > length){
		// 		break;
		// 	}
		// }

		for (int x = 0; x < loserDNA.Count; x++){
			if (UnityEngine.Random.Range(0, favour) == 0){
				childDNA.Add(loserDNA[x]);
			} else {
				childDNA.Add(winnerDNA[x]);
			}
		}

		//mutate
		List<string> options = new List<string>(){
			"nodeStart", 
			"intensity", 
			"1", 
			"action", 
			"Null", 
			"setVariables", 
			"intVariables", 
			"angle", 
			UnityEngine.Random.Range(0, 360).ToString(), 
			"turnRightAction", 
			"boolRequirements", 
			"wallInfront", 
			"True", 
			"nodeEnd", 
			"MoveForwardAction", 
		};
		childDNA[UnityEngine.Random.Range(0, childDNA.Count)] = options[UnityEngine.Random.Range(0, options.Count)];


		// int split = UnityEngine.Random.Range(0, motherDNA.Count);

		// childNode = new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>());
		// int d = childNode.readStringList(childDNA, 0);
		// return childNode;
		return childDNA;
	}

	public List<string> nodeStringLoader(string f){
		List<string> newNodeStringList = new List<string>();
		string line;
		System.IO.StreamReader file = 
		   new System.IO.StreamReader(f);
		while((line = file.ReadLine()) != null)
		{	
			newNodeStringList.Add(line);
		}
		file.Close();
		return newNodeStringList;
	}

	public void punish(){
		// Debug.Log("PUNISH");
		// isPunished = true;
		nodePopulation[currentNode].points += -1;
	}

	public void reward(){
		// Debug.Log("REWARD");
		// isRewarded = true;
		nodePopulation[currentNode].points += 1;
	}

	public void breed(){
		// Debug.Log("breed");
		Debug.Log("breedStart");

    	// isRewarded = false;
    	// isPunished = false;

    	//put in order
		List<Node> orderedPopulation = new List<Node>();
		for (int x = 0; x < 5; x++){
			orderedPopulation.Add(new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>()));	
		}

		int y = 4;
		int addCount = 0;
    	while (addCount < 5){
    		int currentBestPoints = -100;
    		int currentBestNode = 0;
    		bool found = false;
    		for (int x = 0; x < nodePopulation.Count; x++){
    			if (nodePopulation[x].points >= currentBestPoints){
    				currentBestNode = x;
    				currentBestPoints = nodePopulation[x].points;
    				found = true;
    				// Debug.Log("found");
    			}
    		}
    		Debug.Log(nodePopulation.Count);

			List<string> opSL = nodePopulation[currentBestNode].getStringList(new List<string>());
    		orderedPopulation[y].readStringList(opSL, 0);
    		addCount++;
    		// Debug.Log(nodePopulation[currentBestNode].points);
    		orderedPopulation[y].points = nodePopulation[currentBestNode].points+0;
    		orderedPopulation[y].timer = nodePopulation[currentBestNode].timer+0;
    		// orderedPopulation[y].timer = 5-y;
    		y--;
    		// if (y >= 5){
    		// 	y = 0;
    		// }
    		// orderedPopulation.Add(nodePopulation[currentBestNode]);
    		nodePopulation.RemoveAt(currentBestNode);
    	}

    	int high = 5;

    	// orderedPopulation[0].timer = 6;
    	// orderedPopulation[1].timer = 5;
    	// orderedPopulation[2].timer = 4;
    	// orderedPopulation[3].timer = 3;
    	// orderedPopulation[4].timer = 2;

    	// Debug.Log("nodePopulation");
    	// Debug.Log(nodePopulation.Count);

    	// Debug.Log("orderedPopulation");
    	// Debug.Log(orderedPopulation.Count);
    	Debug.Log("orderedPopulation.Count");
    	Debug.Log(orderedPopulation.Count);


    	if (orderedPopulation[3].points < orderedPopulation[4].points){

	    	orderedPopulation[3].timer = high;
	    	orderedPopulation[4].timer = high-1;
    	} else {
	    	orderedPopulation[3].timer = high-1;
	    	orderedPopulation[4].timer = high-1;
    	}

    	if (orderedPopulation[2].points < orderedPopulation[3].points){
    		orderedPopulation[2].timer = orderedPopulation[3].timer + 1;
    	} else {
    		orderedPopulation[2].timer = orderedPopulation[3].timer;
    	}

    	if (orderedPopulation[1].points < orderedPopulation[2].points){
    		orderedPopulation[1].timer = orderedPopulation[2].timer + 1;
    	} else {
    		orderedPopulation[1].timer = orderedPopulation[2].timer;
    	}

    	if (orderedPopulation[0].points < orderedPopulation[1].points){
    		orderedPopulation[0].timer = orderedPopulation[1].timer + 1;
    	} else {
    		orderedPopulation[0].timer = orderedPopulation[1].timer;
    	}

    	timer1 = orderedPopulation[0].timer;
    	timer2 = orderedPopulation[1].timer;
    	timer3 = orderedPopulation[2].timer;
    	timer4 = orderedPopulation[3].timer;
    	timer5 = orderedPopulation[4].timer;

    	points1 = orderedPopulation[0].points;
    	points2 = orderedPopulation[1].points;
    	points3 = orderedPopulation[2].points;
    	points4 = orderedPopulation[3].points;
    	points5 = orderedPopulation[4].points;

    	//make new generation
		List<Node> newPopulation = new List<Node>();
		// newPopulation.Add(new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>()));
		// newPopulation.Add(orderedPopulation[0]);
		for (int x = 0; x < 5; x++){
			newPopulation.Add(new Node(1f, null, new Dictionary<string, bool>(), new Dictionary<string, int>()));	
		}

		List<string> opSL0 = orderedPopulation[0].getStringList(new List<string>());
		List<string> opSL1 = orderedPopulation[1].getStringList(new List<string>());
		List<string> opSL2 = orderedPopulation[2].getStringList(new List<string>());
		List<string> opSL3 = orderedPopulation[3].getStringList(new List<string>());
		List<string> opSL4 = orderedPopulation[4].getStringList(new List<string>());

		newPopulation[0].readStringList(opSL0, 0);
		newPopulation[1].readStringList(opSL1, 0);
		newPopulation[3].readStringList(opSL3, 0);
		newPopulation[2].readStringList(opSL2, 0);
		newPopulation[4].readStringList(opSL4, 0);

		// newPopulation[0].points = orderedPopulation[0].points;
		// newPopulation[1].points = orderedPopulation[1].points;
		// newPopulation[2].points = orderedPopulation[2].points;
		// newPopulation[3].points = orderedPopulation[3].points;
		// newPopulation[4].points = orderedPopulation[4].points;

		// newPopulation[0].timer = orderedPopulation[0].timer;
		// newPopulation[1].timer = orderedPopulation[1].timer;
		// newPopulation[2].timer = orderedPopulation[2].timer;
		// newPopulation[3].timer = orderedPopulation[3].timer;
		// newPopulation[4].timer = orderedPopulation[4].timer;

		// newPopulation[0].readStringList(orderedPopulation[0].getStringList(new List<string>()), 0);
		// newPopulation[1].readStringList(makeChild(
		// 	orderedPopulation[1].getStringList(new List<string>()), 
		// 	orderedPopulation[0].getStringList(new List<string>()),
		// 	new List<string>(), 2), 0);
		// newPopulation[2].readStringList(makeChild(
		// 	orderedPopulation[2].getStringList(new List<string>()), 
		// 	orderedPopulation[0].getStringList(new List<string>()),
		// 	new List<string>(), 2), 0);
		// newPopulation[3].readStringList(makeChild(
		// 	orderedPopulation[3].getStringList(new List<string>()), 
		// 	orderedPopulation[1].getStringList(new List<string>()),
		// 	new List<string>(), 2), 0);
		// newPopulation[4].readStringList(makeChild(
		// 	orderedPopulation[4].getStringList(new List<string>()), 
		// 	orderedPopulation[2].getStringList(new List<string>()),
		// 	new List<string>(), 2), 0);

		nodePopulation = newPopulation;

		Debug.Log("breedComplete");

		// List<string> loserDNA;
		// List<string> winnerDNA;
		// List<string> childDNA = new List<string>();

		// if (nodeCounter <= 10){
		// 	// alphaNode;
		// 	loserDNA = omegaNode.getStringList(new List<string>());
		// 	winnerDNA = winnerNode.getStringList(new List<string>());
			
		// } else {
		// 	// omegaNode;
		// 	loserDNA = alphaNode.getStringList(new List<string>());
		// 	winnerDNA = winnerNode.getStringList(new List<string>());
			
		// }

		// makeChild(loserDNA, winnerDNA, childDNA, 2);
	}

	public void setCenterRed(float centerRed){
		floatConditions["centerRed"] = centerRed;
	}

	IEnumerator beatHeart(){

		//update senses
		RaycastHit objectHit;        
		if (Physics.Raycast(transform.position, transform.forward, out objectHit, 100)) {

	        if (objectHit.distance < 20f){
	        	boolConditions["wallInfront"] = true;

	        } else {
	        	boolConditions["wallInfront"] = false;
	        }
		} else {
        	boolConditions["wallInfront"] = false;
        }
        wallInfrontSense = boolConditions["wallInfront"];

		// if (isRewarded == true){
  //   		nodePopulation[currentNode].points += 1;
		// } 

		// if (isPunished == true) {
		// 	Debug.Log("isPunished");
  //   		nodePopulation[currentNode].points = nodePopulation[currentNode].points - 1;
		// }

        try{

        // Debug.Log(floatConditions["centerRed"]);
        	} catch {}
		// if (csg.get) {

	 //        if (objectHit.distance < 20f){
	 //        	boolConditions["wallInfront"] = true;

	 //        } else {
	 //        	boolConditions["wallInfront"] = false;
	 //        }
		// } else {
  //       	boolConditions["wallInfront"] = false;
  //       }
  //       wallInfrontSense = boolConditions["wallInfront"];

        mainCameraC.enabled = false;
        csg.getScreenShot();
        mainCameraC.enabled = true;


        //check if success
        // if (isRewarded == true){

        // 	breed();
        // 	isRewarded = false;
        // }

        //get next action
        // Debug.Log(currentNode);
		Node nextNode = nodePopulation[currentNode].getNextActionNode(boolConditions, new Dictionary<string, int>(), false);

		//do action
		if (nextNode != null){
			// Debug.Log(nextNode.id);
			// Debug.Log(nextNode.action);
			// Debug.Log(nextNode.childNodes.Count);
			if (nextNode.action == "MoveForwardAction"){

				myrigidbody.AddForce(transform.forward * 120f );
			} else if (nextNode.action == "turnRightAction"){

				// foreach(KeyValuePair<string, int> variable in nextNode.intVariables){
				// 	// intVariables[variable.Key] = variable.Value;
				// 	Debug.Log(variable.Key);
				// 	Debug.Log(variable.Value);
				// }

				if (nextNode.intVariables.ContainsKey("angle")) {

					transform.Rotate(0, nextNode.intVariables["angle"], 0);
				} else {
					transform.Rotate(0, 120, 0);
				}
			}
			// currentNode = -1;
			// nutCounter
		}

		if (nodeCounter > nodePopulation[currentNode].timer) { 
			// Debug.Log("is -1");

			// if (nodeCounter > 10){
			if (nodePopulation[currentNode].timer > 4){
				nodePopulation[currentNode].timer--;
			}

			if (currentNode + 1 >= nodePopulation.Count){
        		// breed();
				currentNode = 0;
			} else {
				currentNode++;
			}
			nodeCounter = 0;
			// }
		}

		nodeCounter++;

		// StartCoroutine(sc.TakeScreenShot(GetComponent<Camera>()));

   		yield return new WaitForSeconds(0.0001f);
		StartCoroutine(beatHeart());
	}

	IEnumerator breedingSeason(){
   		yield return new WaitForSeconds(0.1f);
        breed();
		StartCoroutine(breedingSeason());
	}
}