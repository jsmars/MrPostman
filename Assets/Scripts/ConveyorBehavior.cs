using UnityEngine;

public class ConveyorBehavior : MonoBehaviour {

    public GameObject letterPrefab;
    public Transform letterSpawn;
    private float _timeToSpawn;
	public float _randomNess;
	private Bounds _spawnArea;
	private bool _gameOver;

	public void Start ()
	{
		_spawnArea = letterSpawn.GetComponent<Collider>().bounds;
		_timeToSpawn = 2;

		Events.instance.AddListener<GameOverEvent>(e => _gameOver = true);
	}
	
	public void Update () 
    {
	    if (_gameOver)
	    {
		    return;
	    }

        _timeToSpawn -= Time.deltaTime;
        if (_timeToSpawn < 0)
        {
            SpawnLetter();            
        }
	}

    void SpawnLetter()
    {
	    var spawnPosition = letterSpawn.position + GetRandomPointInSpawnArea();
        var spawnRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(110.0f, 130.0f)));

        Instantiate(letterPrefab, spawnPosition, spawnRotation);
        var randomSpawnTime = Random.Range(_randomNess, _randomNess+1);
        _timeToSpawn = randomSpawnTime;
    }

	private Vector3 GetRandomPointInSpawnArea()
	{
		var size = _spawnArea.extents;
		return new Vector3(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y), Random.Range(-size.z, size.z));
	}
}
