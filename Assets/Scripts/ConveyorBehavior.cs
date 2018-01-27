using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBehavior : MonoBehaviour {

    public GameObject letterPrefab;
    public Transform letterSpawn;
    private float _timeToSpawn;
	public float _randomNess;

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
        var spawnPosition = letterSpawn.position + new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-1.0f, 1.0f), 0);
        var spawnRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(110.0f, 130.0f)));

        var letter = (GameObject)Instantiate(letterPrefab, spawnPosition, spawnRotation);
        var randomSpawnTime = Random.Range(_randomNess, _randomNess+1);
        _timeToSpawn = randomSpawnTime;
    }
}
