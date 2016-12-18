using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour
{
    public int minutes = 0;
    public int seconds = 0;

    private float _secondTimer = 1;

    private Health _health;
    private GameObject _damageUI;
    private GameObject[] _lives;
    private GameObject _gameOver;

    private TextMesh _timeText;
    private GameObject _extraLives;
    private TextMesh _extraLivesText;

    private SteamVR_TrackedController _controller;

    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _controller = transform.GetComponentInParent<SteamVR_TrackedController>();
        _controller.SetDeviceIndex(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost));
        GameObject player = GameObject.Find("Player");
        _health = player.GetComponent<Health>();
        _gameOver = player.transform.FindChild("GameOver").gameObject;
        _gameOver.SetActive(false);

        _damageUI = player.transform.Find("Camera (eye)/DamageUI").gameObject;
        _damageUI.SetActive(false);

        _lives = new GameObject[6];

        Transform status = transform.Find("Status");
        _timeText = status.Find("Time").GetComponent<TextMesh>();
        _extraLives = status.Find("Lives").gameObject;
        _extraLivesText = _extraLives.transform.Find("Count").GetComponent<TextMesh>();

        for (int i = 0; i < 5; i++)
        {
            _lives[i] = status.GetChild(2 + i).gameObject;
        }

        _health.HealthChange += UpdateHealthUI;
        _health.Damaged += () =>
        {
            StartCoroutine(FlashDamageUI());
        };

        UpdateHealthUI(_health.startingHealth);

        _health.Death += () =>
        {
            Time.timeScale = 0.0f;
            _gameOver.SetActive(true);
            FindObjectOfType<Gun>().EnableSafety();
        };
    }

    void Update()
    {
        _secondTimer -= Time.deltaTime;

        if (_secondTimer <= 0)
        {
            _secondTimer += 1;

            seconds--;

            if (seconds < 0)
            {
                minutes--;

                if (minutes >= 0)
                    seconds = 59;
                else
                {
                    seconds = 0;
                    minutes = 0;
                    _health.TakeDamage(1000000);
                }
            }
        }

        if (minutes <= 0 && seconds < 21)
            _timeText.GetComponent<Renderer>().material.color = seconds % 2 == 0 ? Color.red : Color.white;

        _timeText.text = string.Format("{0:00}:{1:00}:{2:00}'", minutes, seconds, _secondTimer * 100);

        if (Input.GetKeyUp(KeyCode.H) && _gameController.DevMode)
            _health.TakeDamage(1);
        if (_controller.gripped && _gameController.DevMode)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateHealthUI(int newHealth)
    {
        if (newHealth > 5)
        {
            _extraLives.SetActive(true);

            for (int i = 0; i < 5; i++)
                _lives[i].SetActive(false);

            _extraLivesText.text = string.Format("x {0}", newHealth);
        }
        else
        {
            _extraLives.SetActive(false);

            for (int i = 0; i < 5; i++)
                _lives[i].SetActive(i < newHealth);
        }
    }

    private IEnumerator FlashDamageUI()
    {
        _damageUI.SetActive(true);

        yield return new WaitForSeconds(0.075f);

        _damageUI.SetActive(false);
    }
}
