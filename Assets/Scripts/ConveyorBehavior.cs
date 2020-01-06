using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.Scripts.Enums;
using Assets.Scripts;
using UnityEngine.UI;

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
    public static int CurrentWave = -1;
    private float waveSpawnRate = 1;
    private int waveSpawnsLeft;
    private int waveSpawnsTotal;
    Helpers.WeightedList<LetterTypeEnum> waveTypes = new Helpers.WeightedList<LetterTypeEnum>();
    List<int> validPostNumbers = new List<int>();

    public void Start ()
	{
        CurrentWave = 0;
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

        // Spawn BoxNumberBins
        var baseBox = GameObject.Find("BoxNumberBinFIRST");
        var xTot = -1.76f;
        var yTot = 1.69f - 2.45f;
        var xCount = 9;
        var yCount = 4;
        var boxNum = Random.Range(1, 5);

        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                GameObject obj;
                if (x == 0 && y == 0)
                    obj = baseBox;
                else
                    obj = Instantiate(baseBox, baseBox.transform.position + new Vector3((xTot / (xCount - 1)) * x, (yTot / (yCount - 1)) * y, 0), baseBox.transform.rotation);
                var entity = obj.GetComponent<BinEntity>();
                boxNum += Random.Range(1, 5);
                entity.LetterNumber = boxNum;
                entity.transform.GetComponent<TextMesh>().text = boxNum.ToString();
                validPostNumbers.Add(boxNum);
            }
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

        if (_timeToSpawn < 0 && !spawnPause)
        {
            SpawnLetter();
            //Debug.Log("Wave " + _currentWave + " letter " + waveSpawnsLeft + " / " + waveSpawnsTotal);
            waveSpawnsLeft--;
        }

        if (waveSpawnsLeft <= 0)
        {
            spawnPause = true;
            _timeToUnpause -= Time.deltaTime;

            if (_timeToUnpause < 0)
            {
                spawnPause = false;
                CurrentWave++;
                Events.instance.Raise(new WaveChangeEvent(CurrentWave));
                SetupCurrentWave();

                _timeToUnpause = WavePauseTimer;
            }
        }
	}

    private void SetupCurrentWave() 
    {
        switch (CurrentWave)
        {
            case 0: // Only easy letters
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveSpawnsTotal = waveSpawnsLeft = 5;
                waveSpawnRate = 0.5f;
                break;

            case 1: // A bunch of fast letters
                waveTypes.Clear();
                waveSpawnRate = 1.8f;
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveSpawnsTotal = waveSpawnsLeft = 20;
                break;

            case 2: // some easy packages
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Package, 10);
                waveSpawnRate = 0.4f;
                waveSpawnsTotal = waveSpawnsLeft = 3;
                break;

            case 3: // a few harder packages
                waveSpawnRate = 0.8f;
                waveSpawnsTotal = waveSpawnsLeft = 8;
                break;

            case 4: // some fun chaos bombs
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Illegal, 10);
                waveSpawnRate = 10f;
                waveSpawnsTotal = waveSpawnsLeft = 30;
                break;

            case 5: // lets make a little mix
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveTypes.Add(LetterTypeEnum.Illegal, 3);
                waveTypes.Add(LetterTypeEnum.Package, 3);
                waveSpawnRate = 1;
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            case 6: // lets give a mystery numbered letter
                waveTypes.Clear();
                waveSpawnRate = 1;
                waveTypes.Add(LetterTypeEnum.Numbered, 1);
                waveSpawnsTotal = waveSpawnsLeft = 1;
                break;

            case 7: // a few numbered and a lot of letters
                waveTypes.Clear();
                waveSpawnRate = 1;
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveTypes.Add(LetterTypeEnum.Numbered, 2);
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            case 8: // introduce stamped packages
                waveTypes.Clear();
                waveSpawnRate = 0.5f;
                waveTypes.Add(LetterTypeEnum.Stamped, 10);
                waveSpawnsTotal = waveSpawnsLeft = 3;
                break;

            case 9: // a few more stamped
                waveTypes.Clear();
                waveSpawnRate = 0.8f;
                waveTypes.Add(LetterTypeEnum.Stamped, 10);
                waveSpawnsTotal = waveSpawnsLeft = 8;
                break;

            case 10: // now a big mix
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveTypes.Add(LetterTypeEnum.Illegal, 3);
                waveTypes.Add(LetterTypeEnum.Package, 3);
                waveTypes.Add(LetterTypeEnum.Stamped, 3);
                waveTypes.Add(LetterTypeEnum.Numbered, 1);
                waveSpawnRate = 1;
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            // case 11, 12, two more harder waves

            case 13: // chaos bombs and letters
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Illegal, 7);
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveSpawnRate = 5;
                waveSpawnsTotal = waveSpawnsLeft = 30;
                break;

            case 14: // a harder mix
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveTypes.Add(LetterTypeEnum.Illegal, 3);
                waveTypes.Add(LetterTypeEnum.Package, 6);
                waveTypes.Add(LetterTypeEnum.Stamped, 5);
                waveTypes.Add(LetterTypeEnum.Numbered, 2);
                waveSpawnRate = 1;
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            // harder

            case 18: // lots of black letters
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Numbered, 10);
                waveSpawnRate = 0.4f;
                waveSpawnsTotal = waveSpawnsLeft = 10;
                break;

            case 19: // Just stamped
                waveTypes.Clear();
                waveSpawnRate = 1;
                waveTypes.Add(LetterTypeEnum.Stamped, 10);
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            case 20: // Mega letter chaos
                waveTypes.Clear();
                waveSpawnRate = 6;
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveSpawnsTotal = waveSpawnsLeft = 50;
                break;

            case 21: // Just packages
                waveTypes.Clear();
                waveSpawnRate = 1;
                waveTypes.Add(LetterTypeEnum.Package, 10);
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            case 22: // final mega hard mix mix
                waveTypes.Clear();
                waveTypes.Add(LetterTypeEnum.Letter, 10);
                waveTypes.Add(LetterTypeEnum.Illegal, 2);
                waveTypes.Add(LetterTypeEnum.Package, 6);
                waveTypes.Add(LetterTypeEnum.Stamped, 4);
                waveTypes.Add(LetterTypeEnum.Numbered, 3);
                waveSpawnRate = 1.5f;
                waveSpawnsTotal = waveSpawnsLeft = 15;
                break;

            default: // if nothing is defined, just make a harder version of the last wave
                waveSpawnRate *= 1.2f;
                waveSpawnsTotal = waveSpawnsLeft = (int)(waveSpawnsTotal * 1.2f);
                break;
        }

    }

    List<LetterObj> temp = new List<LetterObj>();
    private LetterObj GetObjFromType(LetterTypeEnum type)
    {
        // Save all of this type to a temp list
        temp.Clear();
        foreach (var item in LetterObjects)
            if (item.Value.Script.LetterType == type)
                temp.Add(item.Value);

        // randomize from that list
        if (temp.Count > 0)
            return temp[Random.Range(0, temp.Count)];

        // it doesnt exist
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

            switch (newLetter.LetterType)
            {
                case LetterTypeEnum.Letter:
                    newLetter.LetterColor = (LetterColor)Random.Range(0, Helpers.LetterColorCount);
                    break;

                case LetterTypeEnum.Package:
                    newLetter.Weight = Random.Range(1, 10);
                    break;

                case LetterTypeEnum.Numbered:
                    var num = validPostNumbers[Random.Range(0, validPostNumbers.Count)];
                    newLetter.LetterNumber = num;
                    newLetter.transform.GetComponent<Text>().text = num.ToString();
                    break;
            }

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