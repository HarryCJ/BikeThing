using UnityEngine;
using System.Collections;
using System;

//Attach this to a camera
public class CameraScreenGrab : MonoBehaviour {
		
	//how chunky to make the screen
	public int pixelSize = 4;
	public FilterMode filterMode = FilterMode.Point;
	public Camera[] otherCameras;
	private Material mat;
	Texture2D tex;

	float centerRed = -1f;
	int width = -2;
	int height = -3;

	BustAI parent;
	
	void Start () {
		parent = transform.parent.gameObject.GetComponent<BustAI>();
		GetComponent<Camera>().pixelRect = new Rect(0,0,Screen.width/pixelSize,Screen.height/pixelSize);
		for (int i = 0; i < otherCameras.Length; i++)
			otherCameras[i].pixelRect = new Rect(0,0,Screen.width/pixelSize,Screen.height/pixelSize);
	}
	
	void OnGUI()
	{
  //       GetComponent<Camera>().Render();
		// if (Event.current.type == EventType.Repaint)
		// 	Graphics.DrawTexture(new Rect(0,0,Screen.width, Screen.height), tex);
	}
	

	public void getScreenShot()
	{	
		// Debug.Log("bla");
		if(!mat) {
			Shader shader = Shader.Find("Hidden/SetAlpha");
			mat = new Material(shader);
		}
		// Draw a quad over the whole screen with the above shader
		GL.PushMatrix ();
		GL.LoadOrtho ();
		for (var i = 0; i < mat.passCount; ++i) {
			mat.SetPass (i);
			GL.Begin( GL.QUADS );
			GL.Vertex3( 0, 0, 0.1f );
			GL.Vertex3( 1, 0, 0.1f );
			GL.Vertex3( 1, 1, 0.1f );
			GL.Vertex3( 0, 1, 0.1f );
			GL.End();
		}
		GL.PopMatrix ();	
		
		
		DestroyImmediate(tex);
        GetComponent<Camera>().Render();

		tex = new Texture2D(Mathf.FloorToInt(GetComponent<Camera>().pixelWidth), Mathf.FloorToInt(GetComponent<Camera>().pixelHeight));
		tex.filterMode = filterMode;
		// tex.ReadPixels(new Rect(0, 0, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight), 0, 0);
		// tex.Apply(false);
		Color TheColorPicked = tex.GetPixel(15, 15);
		// Debug.Log(TheColorPicked.ToString());
		parent.setCenterRed(1f);
		width = tex.width;
		height = tex.height;
	}

	public int getWidth(){
		return width;
	}

	public int getHeight(){
		return height;
	}

	public float getCenterRed(){

		return centerRed;

		// if(!mat) {
		// 	Shader shader = Shader.Find("Hidden/SetAlpha");
		// 	mat = new Material(shader);
		// }
		// // Draw a quad over the whole screen with the above shader
		// GL.PushMatrix ();
		// GL.LoadOrtho ();
		// for (var i = 0; i < mat.passCount; ++i) {
		// 	mat.SetPass (i);
		// 	GL.Begin( GL.QUADS );
		// 	GL.Vertex3( 0, 0, 0.1f );
		// 	GL.Vertex3( 1, 0, 0.1f );
		// 	GL.Vertex3( 1, 1, 0.1f );
		// 	GL.Vertex3( 0, 1, 0.1f );
		// 	GL.End();
		// }
		// GL.PopMatrix ();	
		
		
		// DestroyImmediate(tex);
  //       GetComponent<Camera>().Render();

		// tex = new Texture2D(Mathf.FloorToInt(GetComponent<Camera>().pixelWidth), Mathf.FloorToInt(GetComponent<Camera>().pixelHeight));
		// tex.filterMode = filterMode;
		// // tex.ReadPixels(new Rect(0, 0, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight), 0, 0);
		// tex.Apply(false);
	 //     // RenderTexture.active = null;
	 //     GetComponent<Camera>().targetTexture = null;
		// centerRed = tex.GetPixel(15, 15).grayscale * 5f;
		// width = tex.width;
		// height = tex.height;
		// return centerRed;
	}

}