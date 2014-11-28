using UnityEngine;
using System.Collections;

public class FlyingEnemyManager : MonoBehaviour
{
	public GameObject flyingRat;
	public float ratSpawnRate = 4f;
	float ratSpawnTimer = 1f;
	Transform playerTransform;
	
	void Awake()
	{
		var o = GameObject.FindGameObjectWithTag("Player");
		playerTransform = o.transform;;
	}
	
	void Update()
	{
		ratSpawnTimer -= Time.deltaTime;
		if (ratSpawnTimer <= 0)
		{
			ratSpawnTimer = ratSpawnRate;
			var position = playerTransform.position;
			position.x += 30f;
			position.y += Random.Range(-5.0f, 5.0f);
			position.z = -1.0f;
			var o = (GameObject)Instantiate(flyingRat, position, Quaternion.identity);
        }
    }
}
