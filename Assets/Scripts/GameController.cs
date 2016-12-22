using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    public GameObject player;
    public AudioClip audioMove;

    private AccelerationMove _playerMove;

    private AudioSource _audioSource;
    private Gun _playerGun;
    private PhoneUI _phone;

    private List<GameEvent> _events;
    private int _eventCounter;
    private int _enemyCount;

    private AsyncOperation asyncOperation;

    public bool DevMode = false;

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
        }
    }
    
    void Start()
    {
        _playerMove = player.GetComponent<AccelerationMove>();
        _playerGun = player.GetComponentInChildren<Gun>(true);
        _phone = player.GetComponentInChildren<PhoneUI>(true);

        _audioSource = GetComponent<AudioSource>();

        _enemyCount = 0;

        _phone.timerEnabled = !DevMode;
    }
    
    void Update()
    { 
        while (!Busy && _eventCounter < _events.Count)
        {
            StartNextEvent();
        }
    }

    private IEnumerator PlayAudio(AudioClip clip, EAudioType audioType, float volume)
    {
        Busy = true;
        float volumeMod = 0;

        switch(audioType)
        {
            case EAudioType.DIALOGUE:
                volumeMod = Options.DialogueVolume;
                break;
            case EAudioType.MUSIC:
                volumeMod = Options.MusicVolume;
                break;
            case EAudioType.SFX:
                volumeMod = Options.FXVolume;
                break;
            case EAudioType.NONE:
                break;
        }

        volumeMod *= Options.MasterVolume;
        _audioSource.PlayOneShot(clip, volume * volumeMod);

        yield return new WaitForSeconds(clip.length);

        Busy = false;
    }

    private void PlayAudioAsync(AudioClip clip, EAudioType audioType, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }

    private IEnumerator MovePlayer(Transform[] targetLocations)
    {
        Busy = true;

        _phone.timerEnabled = false;
        _playerGun.EnableSafety();

        _audioSource.PlayOneShot(audioMove, 1.0f);

        yield return new WaitForSeconds(0.5f);

        _playerMove.StartMove(targetLocations);

        while (!_playerMove.Arrived)
            yield return null;

        _playerGun.DisableSafety();
        _phone.timerEnabled = true;

        Busy = false;
    }

    private IEnumerator MovePlayerAsync(Transform[] targetLocations)
    {
        _phone.timerEnabled = false;
        _playerGun.EnableSafety();

        _audioSource.PlayOneShot(audioMove, 1.0f);

        yield return new WaitForSeconds(0.5f);

        _playerMove.StartMove(targetLocations);

        while (!_playerMove.Arrived)
            yield return null;

        _playerGun.DisableSafety();
        _phone.timerEnabled = true;
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

    private void SpawnEntity(GameObject prefab, Transform spawn, Transform destination)
    {
        GameObject go = Instantiate(prefab, spawn.position, spawn.rotation);
        go.GetComponent<Move>().StartMove(destination);

        Health health = go.GetComponent<Health>();

        if (health != null)
        {
            health.Death += OnEnemyDeath;
            _enemyCount++;
        }
    }

    private void RunGeneric(Action action)
    {
        if (action != null)
            action();
    }

    private IEnumerator Conditional(Func<bool> condition)
    {
        Busy = true;

        while (!condition())
            yield return null;

        Busy = false;
    }

    private IEnumerator LoadLevel(string level)
    {
        Busy = true;

        asyncOperation = SceneManager.LoadSceneAsync(level);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
            yield return null;

        asyncOperation.allowSceneActivation = true;
    }

    private void OnEnemyDeath()
    {
        _enemyCount--;
    }

    private void StartNextEvent()
    {
        GameEvent e = _events[_eventCounter++];

        if(DevMode && (e.EventType == EEventType.AUDIO || e.EventType == EEventType.WAIT) && !e.Async)
        {
            Debug.Log("[" + _eventCounter + "] Skipping " + e.EventType);
            Busy = false;
            return;
        }

        Debug.Log("[" + _eventCounter + "] Starting " + e.EventType);

        switch (e.EventType)
        {
            case EEventType.GENERIC:
                RunGeneric((Action)e.Parameters[0]);
                break;
            case EEventType.CONDITION:
                StartCoroutine(Conditional((Func<bool>)e.Parameters[0]));
                break;
            case EEventType.LOAD_LEVEL:
                StartCoroutine(LoadLevel((string)e.Parameters[0]));
                break;
            case EEventType.AUDIO:
                if (e.Async) PlayAudioAsync((AudioClip)e.Parameters[0], (EAudioType)e.Parameters[1], (float)e.Parameters[2]);
                else StartCoroutine(PlayAudio((AudioClip)e.Parameters[0], (EAudioType)e.Parameters[1], (float)e.Parameters[2])); 
                break;
            case EEventType.MOVE:
                StartCoroutine(MovePlayer(new Transform[] { (Transform)e.Parameters[0] }));
                break;
            case EEventType.MULTI_MOVE:
                if (e.Async) StartCoroutine(MovePlayerAsync((Transform[])e.Parameters));
                else StartCoroutine(MovePlayer((Transform[])e.Parameters));
                break;
            case EEventType.SPAWN:
                SpawnEntity((GameObject)e.Parameters[0], (Transform)e.Parameters[1], (Transform)e.Parameters[2]);
                break;
            case EEventType.AREA_CLEAR:
                StartCoroutine(AreaClear());
                break;
            case EEventType.WAIT:
                StartCoroutine(Wait((float)e.Parameters[0]));
                break;
            case EEventType.SAFETY_ON:
                _playerGun.EnableSafety();
                break;
            case EEventType.SAFETY_OFF:
                _playerGun.DisableSafety();
                break;
            case EEventType.NONE:
                break;
        }
    }

    public void Init(List<GameEvent> events)
    {
        _events = events;
    }

    public void GameOver()
    {
        Infantry[] infantry = FindObjectsOfType<Infantry>();
        for(int i = 0; i < infantry.Length; i++)
        {
            infantry[i].enabled = false;
        }
    }
}
