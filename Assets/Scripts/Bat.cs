using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour {


	int health;
	// Use this for initialization
	void Start () {
		health = 3;
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.AddForce(new Vector2 (-10.0f, 0.0f));
		CheckIfOffScreen();
	}
	
	void CheckIfOffScreen()
	{
		var cam = Camera.main;
		var viewportPosition = cam.WorldToViewportPoint (transform.position);
		var newPosition = transform.position;
		
		if (viewportPosition.x < 0)
			Destroy (gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		GameObject collisionObject = col.gameObject;
		if (collisionObject.tag == "Player")
		{
			collisionObject.SendMessage ("applyDamage");
		}
	}

	void applyDamage(int i){
		health -= i;
		if (health < 1){
			Destroy (gameObject);
		}
	}
}
