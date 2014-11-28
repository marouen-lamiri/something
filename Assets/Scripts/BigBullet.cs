using UnityEngine;
using System.Collections;

public class BigBullet : MonoBehaviour {

	// Use this for initialization
	Animator animator;
	float deathTime;
	int value;
	void Start () {
		Invoke ("selfDestruct", 1.5f);
		deathTime = 0.3f;
		value = 3;
	}
	void Awake(){
		animator = GetComponent<Animator>();
		animator.SetBool ("dead" ,false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag != "Bullet"){
			animator.SetBool ("dead"  , true);
			rigidbody2D.velocity = new Vector2(0,0);
			Invoke ( "selfDestruct", deathTime);
		}
		
		if(col.gameObject.tag == "Enemy"){
			col.SendMessage("applyDamage" , value);
			animator.SetBool ("dead"  , true);
			rigidbody2D.velocity = new Vector2(0,0);
			Invoke ( "selfDestruct", deathTime);
		}
	}
	void selfDestruct(){
		Destroy (gameObject);
	}
}
