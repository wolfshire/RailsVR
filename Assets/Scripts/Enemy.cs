using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
public class Enemy : Shootable
{
    private Health _health;

	void Start()
	{
        _health = GetComponent<Health>();
	}

	void Update()
	{
		
	}

    public override void OnClick()
    {
        _health.TakeDamage(1);
        Destroy(gameObject);
    }
}
