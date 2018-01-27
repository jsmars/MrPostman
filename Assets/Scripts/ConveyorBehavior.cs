using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBehavior : MonoBehaviour {

    public GameObject letterPrefab;
    public Transform letterSpawn;
    private float _timeToSpawn;

	// Use this for initialization
	void Start () 
    {
        _timeToSpawn = 2;
	}
	
	// Update is called once per frame
	void Update () 
    {
        _timeToSpawn -= Time.deltaTime;
        if (_timeToSpawn < 0)
        {
            SpawnLetter();            
        }
	}

    void SpawnLetter()
    {
        var x = letterSpawn.position.x;
        var y = letterSpawn.position.y;
        var z = letterSpawn.position.z;
        var spawnPosition = new Vector3(Random.Range(x - 0.7f, x + 0.7f), Random.Range(y - 1.0f, y + 5.0f), 0);
        var spawnRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(110.0f, 130.0f)));

        var letter = (GameObject)Instantiate(letterPrefab, spawnPosition, spawnRotation);
        var randomSpawnTime = Random.Range(1.5f, 2.5f);
        _timeToSpawn = randomSpawnTime;
    }
}
