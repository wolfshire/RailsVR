using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Move))]
public class Enemy : Shootable
{
    private Health _health;

	void Start()
	{
        _health = GetComponent<Health>();
        _health.Death += () =>
        {
            Destroy(gameObject);
        };
	}

    public override void OnClick()
    {
        _health.TakeDamage(1);
    }
}
