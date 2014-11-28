using UnityEngine;
using System.Collections;

public class RomeoManager : MonoBehaviour {

	public GameObject romeo;
	Transform playerTransform;
	public static bool romeoSpawned = false;
	int spawnPosition = 0;
	int spawnDistanceNeeded = 100;

	void Awake()
	{
		var o = GameObject.FindGameObjectWithTag("Player");
		playerTransform = o.transform;;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ((int)Runner.distanceTraveled % spawnDistanceNeeded == 0 && romeoSpawned == false && (int)Runner.distanceTraveled > spawnPosition)
		{
			spawnPosition = (int)Runner.distanceTraveled;
			SpawnRomeo ();
		}
	}
	
	void SpawnRomeo()
	{
		if (romeoSpawned == false)
		{
			romeoSpawned = true;
		var position = playerTransform.position;
		position.x += 20.0f;
		position.y += Random.Range(-2.0f, 2.0f);
		position.z = -1.0f;
		Instantiate(romeo, position, Quaternion.identity);
		}
	}
	
}
