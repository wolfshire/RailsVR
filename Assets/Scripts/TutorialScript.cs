using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public AudioClip[] dialogue;
    public Transform[] waypoints;
    public GameObject[] enemies;
    public Transform[] spawns;
    public Transform[] destinations;

    private Gun _gun;

    private List<GameEvent> _events;

	void Start()
	{
        _gun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>(true);

        _events = new List<GameEvent>();

        AddEvent(EEventType.AUDIO, false, dialogue[0], 8.0f);

        AddEvent(EEventType.AUDIO, false, dialogue[1], 8.0f);

        AddEvent(EEventType.CONDITION, false, new Func<bool>(CheckReloaded));

        for (int i = 0; i < 2; i++)
            AddEvent(EEventType.SPAWN, true, enemies[0], spawns[i], spawns[i]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1.0f);

        for (int i = 0; i < 2; i++)
            AddEvent(EEventType.SPAWN, true, enemies[0], spawns[i + 2], destinations[i]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.MOVE, false, waypoints[0]);

        for (int i = 0; i < 2; i++)
            AddEvent(EEventType.SPAWN, true, enemies[1], spawns[i + 4], destinations[i + 2]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.LOAD_LEVEL, false, "MainMenu");

        FindObjectOfType<GameController>().Init(_events);
	}

    private bool CheckReloaded()
    {
        return _gun.Ammo > 0;
    }

    public void AddEvent(EEventType eventType, bool async, params object[] parameters)
    {
        _events.Add(new GameEvent(eventType, async, parameters));
    }
}
