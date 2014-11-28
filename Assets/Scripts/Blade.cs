using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		GameObject collisionObject = col.gameObject;
		if (collisionObject.tag == "Player")
		{
			rigidbody2D.isKinematic = true;
			collisionObject.SendMessage ("applyDamage");
		}
		rigidbody2D.isKinematic = false;
	}
}
