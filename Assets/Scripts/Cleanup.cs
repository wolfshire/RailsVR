using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    public float decayTime = 1;

	void Start()
	{
        Destroy(gameObject, decayTime);	
	}
}
