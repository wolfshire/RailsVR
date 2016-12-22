using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1 : Level
{
    public GameObject[] spawnPrefabs;
    public Transform[] spawnLocations;
    public Transform[] spawnDestinations;
    public Transform[] waypoints;

	void Start()
    {
        AddEvent(EEventType.WAIT, false, 1f);

        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[0], spawnDestinations[0]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[1], spawnDestinations[1]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[2], spawnDestinations[2]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1f);
        AddEvent(EEventType.MULTI_MOVE, true, new Transform[] { waypoints[0], waypoints[1] });
        AddEvent(EEventType.WAIT, false, 1.5f);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[3], spawnDestinations[3]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[4], spawnDestinations[4]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1f);
        
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[5], spawnDestinations[5]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[6], spawnDestinations[6]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1f);
        AddEvent(EEventType.MULTI_MOVE, false, new Transform[] { waypoints[2], waypoints[3] });
        AddEvent(EEventType.WAIT, false, 1f);

        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[7], spawnDestinations[7]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[8], spawnDestinations[8]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[9], spawnDestinations[9]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[10], spawnDestinations[10]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        AddEvent(EEventType.WAIT, false, 1f);
        AddEvent(EEventType.MULTI_MOVE, false, new Transform[] { waypoints[4], waypoints[5], waypoints[6] });
        AddEvent(EEventType.WAIT, false, 1f);

        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[11], spawnDestinations[11]);
        AddEvent(EEventType.SPAWN, true, spawnPrefabs[0], spawnLocations[12], spawnDestinations[12]);
        AddEvent(EEventType.AREA_CLEAR, false, null);

        Init();
    }
}
