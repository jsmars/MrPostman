using UnityEngine;

public class ConveyorBehavior : MonoBehaviour {

    public GameObject letterPrefab;
    public Transform letterSpawn;
    private float _timeToSpawn;
	public float _randomNess;
	private Bounds _spawnArea;

	public void Start ()
    {
        _spawnArea = letterSpawn.GetComponent<Collider>().bounds;
        _timeToSpawn = 2;
	}
	
	public void Update () 
    {
        _timeToSpawn -= Time.deltaTime;
        if (_timeToSpawn < 0)
        {
            SpawnLetter();            
        }
	}


    void SpawnLetter()
    {
        var spawnPosition = letterSpawn.position + GetRandomPointInSpawnArea();// new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-1.0f, 1.0f), 0);
        var spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360.0f), 0));

        var letter = (GameObject)Instantiate(letterPrefab, spawnPosition, spawnRotation);
        var randomSpawnTime = Random.Range(_randomNess, _randomNess+1);
        _timeToSpawn = randomSpawnTime;
    }

	private Vector3 GetRandomPointInSpawnArea()
	{
		var size = _spawnArea.extents;
		return new Vector3(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y), Random.Range(-size.z, size.z));
	}
}
