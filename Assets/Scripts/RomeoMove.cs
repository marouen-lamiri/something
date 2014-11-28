using UnityEngine;
using System.Collections;

public class RomeoMove : MonoBehaviour {

	GameObject camera;

	void Awake()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	

	// Use this for initialization
	void Start () {
		transform.parent = camera.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
