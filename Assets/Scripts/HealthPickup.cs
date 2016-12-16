using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    private Health _playerHealth;

	void Start()
	{
        _playerHealth = GameObject.Find("Player").GetComponent<Health>();
	}

    public override void OnClick()
    {
        _playerHealth.Heal(1);

        base.OnClick();
    }
}
