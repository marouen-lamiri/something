using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Transform center;
	float radius = 5;
	float angle = 0;
	float speed;

	// Use this for initialization
	void Start () {
		speed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (center != null)
		{
		angle += speed * Time.deltaTime;
		float x = Mathf.Cos(angle)*radius + center.transform.position.x;
		float y = Mathf.Sin (angle)*radius + center.transform.position.y;
		transform.position = new Vector2 (x,y);
		}
	}
}
