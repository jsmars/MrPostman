using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Scripts.Enums;
using Assets.Scripts;

public class ConveyorBehavior : MonoBehaviour {

    public static Dictionary<string, LetterObj> LetterObjects = new Dictionary<string, LetterObj>();
    public List<GameObject> LetterPrefabs;
    public Transform LetterSpawn;
    public int WavePauseTimer;
    public float Randomness;
    
    private float _timeToSpawn;
    private float _timeToUnpause;
    private bool spawnPause;
	private Bounds _spawnArea;
	private bool _gameOver;
    private int _currentWave = -1;
    private float waveSpawnRate = 1;
    private int waveSpawnsLeft;
    private int waveSpawnsTotal;
    Helpers.WeightedList<LetterTypeEnum> waveTypes = new Helpers.WeightedList<LetterTypeEnum>();

    public void Start ()
	{
        _currentWave = 0;
        _timeToUnpause = WavePauseTimer;
        _timeToSpawn = 2;

        _spawnArea = LetterSpawn.GetComponent<Collider>().bounds;

        Events.instance.AddListener<GameOverEvent>(SetGameOver);

        foreach (var item in LetterPrefabs)
        {
            LetterObjects[item.name] = new LetterObj
            {
                GameObj = item,
                Script = item.GetComponent<LetterEntity>(),
            };
        }

        SetupCurrentWave();
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

        if (_timeToSpawn < 0 && !spawnPause)
        {
            SpawnLetter();
            Debug.Log("Wave " + _currentWave + " letter " + waveSpawnsLeft + " / " + waveSpawnsTotal);
            waveSpawnsLeft--;
        }

        if (waveSpawnsLeft <= 0)
        {
            spawnPause = true;
            _timeToUnpause -= Time.deltaTime;

            if (_timeToUnpause < 0)
            {
                spawnPause = false;
                _currentWave++;
                waveSpawnRate *= 1.2f;
                Events.instance.Raise(new WaveChangeEvent(_currentWave));
                SetupCurrentWave();

                _timeToUnpause = WavePauseTimer;
            }
        }
	}

    private void SetupCurrentWave() 
    {
        switch (_currentWave)
        {
            case 0:
                waveTypes.Add(LetterTypeEnum.LetterSmall, 10);
                //Uncomment to test all types equially
                //waveTypes.Add(LetterTypeEnum.Illegal, 10);
                //waveTypes.Add(LetterTypeEnum.LetterBig, 10);
                //waveTypes.Add(LetterTypeEnum.Numbered, 10);
                //waveTypes.Add(LetterTypeEnum.PackageBig, 10);
                //waveTypes.Add(LetterTypeEnum.PackageSmall, 10);
                //waveTypes.Add(LetterTypeEnum.Stamped, 10);
                waveSpawnsTotal = waveSpawnsLeft = 10;
                break;
            case 1:
                waveTypes.Add(LetterTypeEnum.LetterBig, 5);
                waveSpawnsTotal = waveSpawnsLeft = 10;
                break;
            case 2:
                waveTypes.Add(LetterTypeEnum.PackageSmall, 5);
                waveTypes.Add(LetterTypeEnum.Illegal, 2);
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;
            case 3:
                waveTypes.Add(LetterTypeEnum.PackageBig, 5);
                waveTypes.Add(LetterTypeEnum.Numbered, 3);
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;
            case 4:
                waveTypes.Add(LetterTypeEnum.Numbered, 2);
                waveSpawnsTotal = waveSpawnsLeft = 20;
                break;
            case 5:
                waveTypes.Add(LetterTypeEnum.Stamped, 1);
                waveSpawnsTotal = waveSpawnsLeft = 20;
                break;
        }

    }

    private static bool AnyOf(LetterObj x)
    {
        return x.Script.LetterType == LetterTypeEnum.LetterSmall || x.Script.LetterType == LetterTypeEnum.LetterBig;
    }

    private LetterObj GetObjFromType(LetterTypeEnum type)
    {
        foreach (var item in LetterObjects)
            if (item.Value.Script.LetterType == type)
                return item.Value;
        return null;
    }
    
    private void SpawnLetter()
    {
        var spawnPosition = LetterSpawn.position + GetRandomPointInSpawnArea();
        var spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360.0f), 0));
        var randomSpawnTime = Random.Range(Randomness, Randomness + 1) / waveSpawnRate;
        _timeToSpawn = randomSpawnTime;

        var t = waveTypes.RandomElement();
        var type = GetObjFromType(t);
        if (type == null)
            Debug.LogError("No letter type added with type: " + t);
        else
        {
            var newObj = Instantiate(type.GameObj, spawnPosition, spawnRotation);
            var newLetter = newObj.GetComponent<LetterEntity>();
            newLetter.LetterColor = (LetterColor)Random.Range(0, Helpers.LetterColorCount);
        }
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