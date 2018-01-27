using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class ConveyorBehavior : MonoBehaviour {

    public static Dictionary<string, LetterObj> LetterObjects = new Dictionary<string, LetterObj>();
    public List<GameObject> letterPrefabs;
    public Transform letterSpawn;
    private float _timeToSpawn;
	public float _randomNess;
	private Bounds _spawnArea;
	private bool _gameOver;

    public void Start ()
	{
		_spawnArea = letterSpawn.GetComponent<Collider>().bounds;
		_timeToSpawn = 2;

        Events.instance.AddListener<GameOverEvent>(SetGameOver);

        foreach (var item in letterPrefabs)
        {
            LetterObjects[item.name] = new LetterObj
            {
                GameObj = item,
                Script = item.GetComponent<LetterEntity>(),
            };
        }
	}

	public void OnDestroy()
	{
		Events.instance.RemoveListener<GameOverEvent>(SetGameOver);
	}

	private void SetGameOver(GameOverEvent gameOverEvent)
	{
		_gameOver = true;
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
        var spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360.0f), 0));
        var randomSpawnTime = Random.Range(_randomNess, _randomNess+1);
        _timeToSpawn = randomSpawnTime;

        //TODO: Add letter choosing logic here instead of just taking random letters
        var list = Enumerable.ToList(LetterObjects.Values);
        var type = list[Random.Range(0, list.Count)];

        var newObj = Instantiate(type.GameObj, spawnPosition, spawnRotation);
        var newLetter = newObj.GetComponent<LetterEntity>();
	    newLetter.LetterColor = (LetterColor)Random.Range(0, Helpers.LetterColorCount);
    }
    
    private Vector3 GetRandomPointInSpawnArea()
    {
        var size = _spawnArea.extents;
        return new Vector3(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y), Random.Range(-size.z, size.z));
    }

    public static bool IsLetterObj(GameObject obj)
    {
        foreach (var item in LetterObjects)
            if (item.Value.GameObj.name == obj.name)
                return true;
        return false;
    }
}

public class LetterObj
{
    public GameObject GameObj;
    public LetterEntity Script;
}