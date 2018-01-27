using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Scripts.Enums;

public class ConveyorBehavior : MonoBehaviour {

    public static Dictionary<string, LetterObj> LetterObjects = new Dictionary<string, LetterObj>();
    public List<GameObject> LetterPrefabs;
    public Transform LetterSpawn;
    public int WaveLengthConstant;
    public int WavePauseTimer;
    public float Randomness;
    
    private float _timeToSpawn;
    private float _timeToNextWave;
    private float _timeToUnpause;
    private bool spawnPause;
	private Bounds _spawnArea;
	private bool _gameOver;
    private WavesEnum _currentWave;
    private List<LetterObj> _letterList;

    public void Start ()
	{
        _currentWave = WavesEnum.wave1;
        _timeToNextWave = 10;
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
        _timeToNextWave -= Time.deltaTime;

        if (_timeToSpawn < 0 && !spawnPause)
        {
            SpawnLetter();            
        }

        if (_timeToNextWave < 0)
        {
            spawnPause = true;
            _timeToUnpause -= Time.deltaTime;

            if (_timeToUnpause < 0)
            {
                spawnPause = false;
                var currentWaveInt = (int)_currentWave;
                if (_currentWave < WavesEnum.wave4)
                {
                    ++currentWaveInt;
                    _currentWave = (WavesEnum)currentWaveInt;

                    SetupCurrentWave();
                }

                _timeToNextWave = (int)(_currentWave + 1) * WaveLengthConstant;
                _timeToUnpause = WavePauseTimer;
            }
        }
	}

    private void SetupCurrentWave() 
    {
        var allowedLetterTypes = new List<LetterTypeEnum>(); 
        switch (_currentWave)
        {
            case WavesEnum.wave1:
                allowedLetterTypes.Add(LetterTypeEnum.LetterSmall);
                break;
            case WavesEnum.wave2:
                allowedLetterTypes.AddRange(new List<LetterTypeEnum> { LetterTypeEnum.LetterSmall, LetterTypeEnum.LetterBig });
                break;
            case WavesEnum.wave3:
                allowedLetterTypes.AddRange(new List<LetterTypeEnum> { LetterTypeEnum.LetterSmall, LetterTypeEnum.LetterBig, LetterTypeEnum.PackageSmall });
                break;
            case WavesEnum.wave4:
                allowedLetterTypes.AddRange(new List<LetterTypeEnum> { LetterTypeEnum.LetterSmall, LetterTypeEnum.LetterBig, LetterTypeEnum.PackageSmall, LetterTypeEnum.PackageBig });
                break;
        }

        _letterList = LetterObjects.Values.Where(x => allowedLetterTypes.Contains(x.Script.LetterType)).ToList();
    }
    
    private void SpawnLetter()
    {
        var spawnPosition = LetterSpawn.position + GetRandomPointInSpawnArea();
        var spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360.0f), 0));
        var randomSpawnTime = Random.Range(Randomness, Randomness + 1);
        _timeToSpawn = randomSpawnTime;

        var letterListIndex = Random.Range(0, _letterList.Count);
        var type = _letterList[letterListIndex];

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