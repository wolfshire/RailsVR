using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchUI : MonoBehaviour
{
    private Health _health;
    private GameObject[] _lives;

    private GameObject _multiply;
    private Text _multiplyText;

	void Start()
    {
        Transform trans = transform;
        _health = trans.GetComponentInParent<Health>();

        _lives = new GameObject[6];

        Transform health = trans.Find("Health");
        _multiply = health.Find("Multiply").gameObject;
        _multiplyText = _multiply.GetComponent<Text>();

        for (int i = 0; i < 6; i++)
        {
            _lives[i] = health.GetChild(1 + i).gameObject;
        }

        _health.HealthChange += UpdateHealthUI;

        UpdateHealthUI(_health.startingHealth);
	}

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
            _health.TakeDamage(1);
    }

    private void UpdateHealthUI(int newHealth)
    {
        if (newHealth > 6)
        {
            _multiply.SetActive(true);

            for (int i = 1; i < 6; i++)
                _lives[i].SetActive(false);

            _multiplyText.text = string.Format("x {0}", newHealth);
        }
        else
        {
            _multiply.SetActive(false);

            for (int i = 0; i < 6; i++)
                _lives[i].SetActive(i < newHealth);
        }
    }
}
