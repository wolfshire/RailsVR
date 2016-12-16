using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkUIText : MonoBehaviour
{
    public float blinkDelay = 1f;
    public float blinkSpeed = 0.2f;

    private bool _blinking = false;
    private Text _text;

    void Start()
    {
        _text = GetComponent<Text>();

        StartCoroutine(Blink());
    }

    void Update()
    {
        if (!_blinking)
            StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        _blinking = true;

        yield return new WaitForSeconds(blinkDelay);

        _text.enabled = false;

        yield return new WaitForSeconds(blinkSpeed);

        _text.enabled = true;
        _blinking = false;
    }
}
