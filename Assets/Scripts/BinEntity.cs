﻿using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinEntity : MonoBehaviour {

    public ParticleSystem Particles;
    public AudioSource CollectSound;
    public AudioSource FailSound;
    private readonly HashSet<GameObject> _binCollected = new HashSet<GameObject>();
    //public GameObject StampMaterial;
    public LetterColor LetterColor;
    public LetterTypeEnum LetterType;
    public int WeightLimit;
    public bool WeightLimitGreater;
    public int LetterNumber;
    public int BoxIndexX;
    public int BoxIndexY;
    private bool _gameOver;

    Collider letterCollider;

    // Use this for initialization
    void Start () {

        Helpers.SetStampColor(gameObject, LetterColor);

        letterCollider = gameObject.GetComponent<Collider>();

        Events.instance.AddListener<GameOverEvent>(StopCollecting);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void OnDestroy()
    {
        Events.instance.RemoveListener<GameOverEvent>(StopCollecting);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!_gameOver && !HasCollected(other))
        {
            var letter = TryGetLetter(other);
            if (letter == null)
                return;

            if (letter.TryScore(this))
            {
                //Destroy(other.gameObject);
                Particles.Play();
                CollectSound.Play();
                _binCollected.Add(other.gameObject);
            }
            else 
            {
                FailSound.Play();
            }
        }
    }

    private static LetterEntity TryGetLetter(Collider other)
    {
        return other.gameObject.GetComponent<LetterEntity>();
    }

    private bool HasCollected(Collider other)
    {
        return _binCollected.Contains(other.gameObject);
    }

    private void StopCollecting(GameOverEvent e)
    {
        _gameOver = true;
    }

    public override string ToString()
    {
        return "BIN (" + gameObject.name + ") " + LetterColor + " / " + LetterNumber;
    }

}
