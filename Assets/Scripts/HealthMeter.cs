using UnityEngine;
using System.Collections;

public class HealthMeter : MonoBehaviour {
	public Runner player;
	public GameObject heart;
	public int health;
	public GameObject[] hearts;
	public float offSet;
	public Vector2 position;

	// Use this for initialization
	void Start () {
		hearts = new GameObject[25];
		offSet = 1.0f;
		health = 0;
		position = transform.position;
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if(playerObj != null){
			player = playerObj.GetComponent<Runner>();
		}

		while(health < player.health){
			addHeart();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void addHeart(){
		GameObject instance = Instantiate(heart, position, Quaternion.identity) as GameObject;
		instance.transform.parent = this.transform;
		position.x = position.x + offSet;
		hearts[health] = instance;
		health++;
	}
	public void removeHeart(){
		health--;
		Destroy (hearts[health]);
		position.x = position.x - offSet;

	}
}
