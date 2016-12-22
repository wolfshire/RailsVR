using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextColorChange : MonoBehaviour
{ 
    private Renderer _textRenderer;
    public Color TextColor;

	void Start()
	{
        _textRenderer = GetComponent<TextMesh>().GetComponent<Renderer>();
        _textRenderer.material.color = TextColor;
	}
}
