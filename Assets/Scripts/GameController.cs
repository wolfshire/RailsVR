﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
    public GameObject player;

    private PlayerMove _playerMove;

    private AudioSource _audioSource;
    private Gun _playerGun;
    private List<GameEvent> _events;
    private int _eventCounter;
    private int _enemyCount;

    private bool _busy = false;
    public bool Busy
    {
        get
        {
            return _busy;
        }
        private set
        {
            _busy = value;

            if (!_busy && _events.Count > 0)
                _events.RemoveAt(0);
        }
    }
    
    void Start()
    {
        _playerMove = player.GetComponent<PlayerMove>();
        _playerGun = player.GetComponentInChildren<Gun>(true);

        _audioSource = GetComponent<AudioSource>();

        _enemyCount = 0;
    }
    
    void Update()
    {
        while (!Busy && _events.Count > 0)
        {
            StartNextEvent();
        }
    }

    private IEnumerator PlayAudio(AudioClip clip, float volume)
    {
        Busy = true;

        _audioSource.PlayOneShot(clip, volume);

        yield return new WaitForSeconds(clip.length);

        Busy = false;
    }

    private IEnumerator MovePlayer(Transform targetLocation)
    {
        Busy = true;

        _playerGun.EnableSafety();
        _playerMove.Move(targetLocation);

        while (!_playerMove.Arrived)
            yield return null;

        _playerGun.DisableSafety();
        Busy = false;
    }

    private IEnumerator AreaClear()
    {
        Busy = true;

        while (_enemyCount > 0)
            yield return null;

        Busy = false;
    }

    private IEnumerator Wait(float seconds)
    {
        Busy = true;

        yield return new WaitForSeconds(seconds);

        Busy = false;
    }

    private void SpawnEnemy(GameObject prefab, Transform spawn, Transform destination)
    {
        GameObject go = Instantiate(prefab, spawn.position, spawn.rotation);
        go.GetComponent<EnemyMovement>().SetDestination(destination);
        go.GetComponent<Health>().Death += OnEnemyDeath;
        _enemyCount++;

        _events.RemoveAt(0);
    }

    private IEnumerator Conditional(Func<bool> condition)
    {
        Busy = true;

        while (!condition())
            yield return null;

        Busy = false;
    }

    private void OnEnemyDeath()
    {
        _enemyCount--;
    }

    private void StartNextEvent()
    {
        GameEvent e = _events[0];
        Debug.Log("Starting event " + e.EventType + " " + _eventCounter++);
        switch (e.EventType)
        {
            case EEventType.CONDITION:
                StartCoroutine(Conditional((Func<bool>)e.Parameters[0]));
                break;
            case EEventType.AUDIO:
                if(e.Async) PlayAudio((AudioClip)e.Parameters[0], (float)e.Parameters[1]);
                else StartCoroutine(PlayAudio((AudioClip)e.Parameters[0], (float)e.Parameters[1])); 
                break;
            case EEventType.MOVE:
                StartCoroutine(MovePlayer((Transform)e.Parameters[0]));
                break;
            case EEventType.SPAWN:
                SpawnEnemy((GameObject)e.Parameters[0], (Transform)e.Parameters[1], (Transform)e.Parameters[2]);
                break;
            case EEventType.AREA_CLEAR:
                StartCoroutine(AreaClear());
                break;
            case EEventType.WAIT:
                StartCoroutine(Wait((float)e.Parameters[0]));
                break;
            case EEventType.NONE:
                break;
        }
    }

    public void Init(List<GameEvent> events)
    {
        _events = events;
    }
}
