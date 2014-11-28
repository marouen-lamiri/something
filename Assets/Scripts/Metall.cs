using UnityEngine;
using System.Collections;

public class Metall : MonoBehaviour {

	int health;

	// Use this for initialization
	void Start () {
		health = 2;
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
	
	void applyDamage(int i){
		health -= i;
		if (health < 1){
			Vector3 newPosition = new Vector3 (-100, 0, 0);
			transform.position = newPosition;
		}
	}
}
