using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : Level
{
    public GameObject[] lights;
    public AudioClip[] miscAudio;
    public AudioClip[] dialogue;
    public Transform[] waypoints;
    public GameObject[] enemies;
    public Transform[] spawns;
    public Transform[] destinations;

    private int _dialogueIndex = 0;
    private Gun _gun;

	void Start()
	{
        _gun = GameObject.Find("Player").GetComponentInChildren<Gun>(true);
        
        AddEvent(EEventType.WAIT, false, 4f);
        AddEvent(EEventType.AUDIO, true, miscAudio[0], EAudioType.SFX, 1f);
        AddEvent(EEventType.GENERIC, false, new Action(Spotlight));
        AddEvent(EEventType.WAIT, false, 1f);

        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);
        AddEvent(EEventType.CONDITION, false, new Func<bool>(CheckReloaded));

        AddEvent(EEventType.WAIT, false, 1f);
        AddEvent(EEventType.AUDIO, true, miscAudio[0], EAudioType.SFX, 1f);
        AddEvent(EEventType.GENERIC, false, new Action(Roomlights));
        AddEvent(EEventType.WAIT, false, 1f);

        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);

        //stationary targets
        for (int i = 0; i < 2; i++)
            AddEvent(EEventType.SPAWN, true, enemies[0], spawns[i], spawns[i]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1f);

        //moving targets
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);
        for (int i = 0; i < 2; i++)
            AddEvent(EEventType.SPAWN, true, enemies[0], spawns[i + 2], destinations[i]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1f);

        //move to 2nd room
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);
        AddEvent(EEventType.WAIT, false, 1f);
        AddEvent(EEventType.MOVE, false, waypoints[0]);
        AddEvent(EEventType.SAFETY_ON, false, null);

        //private wilhelm
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);
        AddEvent(EEventType.SPAWN, true, enemies[1], spawns[4], destinations[2]);
        AddEvent(EEventType.WAIT, false, 7.0f);

        //cover
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);
        AddEvent(EEventType.SPAWN, false, enemies[2], spawns[5], destinations[3]);
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);

        AddEvent(EEventType.WAIT, false, 5.0f);

        //shoot back
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);
        AddEvent(EEventType.SAFETY_OFF, false, null);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1.5f);

        //well done
        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);

        AddEvent(EEventType.AUDIO, false, GetNextDialogue(), EAudioType.DIALOGUE, 2.0f);

        AddEvent(EEventType.LOAD_LEVEL, false, "MainMenu");

        Init();
	}

    private AudioClip GetNextDialogue()
    {
        return dialogue[_dialogueIndex++];
    }

    private bool CheckReloaded()
    {
        return _gun.Ammo > 0;
    }

    private void Spotlight()
    {
        lights[0].SetActive(true);
    }

    private void Roomlights()
    {
        lights[0].SetActive(false);
        lights[1].SetActive(true);
        lights[2].SetActive(true);
    }
}
