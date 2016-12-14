using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTable : MonoBehaviour
{
    public float RotationTime;

    private TextMesh[] _menuLabels;
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private float _elapsedTime;
    private bool _flipping = false;
    private bool _toggled = false;

    void Awake()
    {
        Options.Load();

        _menuLabels = transform.GetComponentsInChildren<TextMesh>();

        for (int i = 3; i < 6; i++)
            _menuLabels[i].gameObject.SetActive(false);
    }

    void Update()
    {
        if (_flipping)
        {
            _elapsedTime += Time.deltaTime;
            float current = _elapsedTime / RotationTime;
            if (current >= 1)
            {
                transform.rotation = _endRotation;
                _flipping = false;

                _toggled = false;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, current);

                if (!_toggled && current >= 0.5f)
                    ToggleLabels();
            }
        }
    }

    public void Flip()
    {
        if (!_flipping)
        {
            _flipping = true;
            _startRotation = transform.rotation;
            _endRotation = _startRotation * Quaternion.Euler(180, 0, 0);
            _elapsedTime = 0;
        }
    }

    private void ToggleLabels()
    {
        _toggled = true;

        foreach (TextMesh label in _menuLabels)
            label.gameObject.SetActive(!label.gameObject.activeSelf);
    }
}
