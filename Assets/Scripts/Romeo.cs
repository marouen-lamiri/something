using UnityEngine;
using System.Collections;

public class Romeo : MonoBehaviour {

	int health;
	float fireRate = 4f;
	public GameObject fireball;
	public Transform fireballPosition;
	public AudioClip fireballSound;
	
	// Use this for initialization
	void Start () {
		health = 20;
	}
	
	// Update is called once per frame
	void Update () {
		fireRate -= Time.deltaTime;
		if (fireRate < 0)
		{
			Shoot ();
			fireRate = 4.0f;
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		GameObject collisionObject = col.gameObject;
		if (collisionObject.tag == "Player")
		{
			rigidbody2D.isKinematic = true;
			collisionObject.SendMessage ("applyDamage");
		}
	}
	
	void applyDamage(int i){
		health -= i;
		if (health < 1){
			RomeoManager.romeoSpawned = false;
			Destroy (gameObject);
		}
	}
	
	void Shoot()
	{
		audio.PlayOneShot(fireballSound, 1.0f);
		GameObject instance = Instantiate(fireball, fireballPosition.position, Quaternion.identity) as GameObject;
		float fireballSpeed = -200.0f;
		instance.rigidbody2D.AddForce (transform.right * fireballSpeed);
	}
}
