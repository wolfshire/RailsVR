using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int minutes;
    public int seconds;

    protected List<GameEvent> _events;

    private GameController _gameController;
    private PhoneUI _phone;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _phone = GameObject.Find("Player").GetComponentInChildren<PhoneUI>(true);

        _events = new List<GameEvent>();
    }

	protected void Init()
    {
        _gameController.Init(_events);
        _phone.minutes = minutes;
        _phone.seconds = seconds;
    }

    protected void AddEvent(EEventType eventType, bool async, params object[] parameters)
    {
        _events.Add(new GameEvent(eventType, async, parameters));
    }
}
