using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    private Button[] _accelerationButtons = new Button[3];
    private Button[] _handednessButtons = new Button[2];

    private Button[] _masterButtons = new Button[2];
    private Slider _masterSlider;
    private Text _masterText;
    private Button[] _dialogueButtons = new Button[2];
    private Slider _dialogueSlider;
    private Text _dialogueText;
    private Button[] _musicButtons = new Button[2];
    private Slider _musicSlider;
    private Text _musicText;
    private Button[] _sfxButtons = new Button[2];
    private Slider _sfxSlider;
    private Text _sfxText;

    private Button[] _subtitleButtons = new Button[2];

    void Awake()
    {
        Options.Load();
    }

    void OnApplicationQuit()
    {
        Options.Save();
    }

    void Start()
    {
        FindElements();
        InitValues();

        gameObject.SetActive(false);
    }

    private void FindElements()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransform gameplay = rectTransform.Find("GameplayPanel").GetComponent<RectTransform>();
        _accelerationButtons[0] = gameplay.Find("Slow").GetComponent<Button>();
        _accelerationButtons[1] = gameplay.Find("Med").GetComponent<Button>();
        _accelerationButtons[2] = gameplay.Find("Fast").GetComponent<Button>();

        _handednessButtons[0] = gameplay.Find("Lefty").GetComponent<Button>();
        _handednessButtons[1] = gameplay.Find("Righty").GetComponent<Button>();

        RectTransform audio = rectTransform.Find("AudioPanel").GetComponent<RectTransform>();
        _masterButtons[0] = audio.Find("MasterVolume/Minus").GetComponent<Button>();
        _masterButtons[1] = audio.Find("MasterVolume/Plus").GetComponent<Button>();
        _masterSlider = audio.Find("MasterVolume/Slider").GetComponent<Slider>();
        _masterText = audio.Find("MasterVolume/Value").GetComponent<Text>();

        _dialogueButtons[0] = audio.Find("DialogueVolume/Minus").GetComponent<Button>();
        _dialogueButtons[1] = audio.Find("DialogueVolume/Plus").GetComponent<Button>();
        _dialogueSlider = audio.Find("DialogueVolume/Slider").GetComponent<Slider>();
        _dialogueText = audio.Find("DialogueVolume/Value").GetComponent<Text>();

        _musicButtons[0] = audio.Find("MusicVolume/Minus").GetComponent<Button>();
        _musicButtons[1] = audio.Find("MusicVolume/Plus").GetComponent<Button>();
        _musicSlider = audio.Find("MusicVolume/Slider").GetComponent<Slider>();
        _musicText = audio.Find("MusicVolume/Value").GetComponent<Text>();

        _sfxButtons[0] = audio.Find("SFXVolume/Minus").GetComponent<Button>();
        _sfxButtons[1] = audio.Find("SFXVolume/Plus").GetComponent<Button>();
        _sfxSlider = audio.Find("SFXVolume/Slider").GetComponent<Slider>();
        _sfxText = audio.Find("SFXVolume/Value").GetComponent<Text>();

        _subtitleButtons[0] = audio.Find("Subtitles/Off").GetComponent<Button>();
        _subtitleButtons[1] = audio.Find("Subtitles/On").GetComponent<Button>();
    }

    private void InitValues()
    {
        _accelerationButtons[Options.Acceleration].interactable = false;
        _handednessButtons[Options.LeftHanded ? 0 : 1].interactable = false;

        _masterSlider.value = Options.MasterVolume;
        _masterText.text = string.Format("{0}", (int)(Options.MasterVolume * 100));

        _dialogueSlider.value = Options.DialogueVolume;
        _dialogueText.text = string.Format("{0}", (int)(Options.DialogueVolume * 100));

        _musicSlider.value = Options.MusicVolume;
        _musicText.text = string.Format("{0}", (int)(Options.MusicVolume * 100));

        _sfxSlider.value = Options.FXVolume;
        _sfxText.text = string.Format("{0}", (int)(Options.FXVolume * 100));

        _subtitleButtons[Options.UseSubtitles ? 1 : 0].interactable = false;
    }

    public void SetAcceleration(int accel)
    {
        _accelerationButtons[Options.Acceleration].interactable = true;

        Options.SetAcceleration(accel);

        _accelerationButtons[Options.Acceleration].interactable = false;
    }

    public void SetHandedness(bool lefty)
    {
        _handednessButtons[Options.LeftHanded ? 0 : 1].interactable = true;

        Options.SetLeftHanded(lefty);

        _handednessButtons[Options.LeftHanded ? 0 : 1].interactable = false;

        FindObjectOfType<MenuGun>().SwapHandedness();
    }

    public void IncrementMasterVolume()
    {
        Options.SetMasterVolume(Options.MasterVolume + 0.1f);

        _masterSlider.value = Options.MasterVolume;
        _masterText.text = string.Format("{0}", Mathf.RoundToInt(Options.MasterVolume * 100));
    }

    public void DecrementMasterVolume()
    {
        Options.SetMasterVolume(Options.MasterVolume - 0.1f);

        _masterSlider.value = Options.MasterVolume;
        _masterText.text = string.Format("{0}", Mathf.RoundToInt(Options.MasterVolume * 100));
    }

    public void IncrementDialogueVolume()
    {
        Options.SetDialogueVolume(Options.DialogueVolume + 0.1f);

        _dialogueSlider.value = Options.DialogueVolume;
        _dialogueText.text = string.Format("{0}", Mathf.RoundToInt(Options.DialogueVolume * 100));
    }

    public void DecrementDialogueVolume()
    {
        Options.SetDialogueVolume(Options.DialogueVolume - 0.1f);

        _dialogueSlider.value = Options.DialogueVolume;
        _dialogueText.text = string.Format("{0}", Mathf.RoundToInt(Options.DialogueVolume * 100));
    }

    public void IncrementMusicVolume()
    {
        Options.SetMusicVolume(Options.MusicVolume + 0.1f);

        _musicSlider.value = Options.MusicVolume;
        _musicText.text = string.Format("{0}", Mathf.RoundToInt(Options.MusicVolume * 100));
    }

    public void DecrementMusicVolume()
    {
        Options.SetMusicVolume(Options.MusicVolume - 0.1f);

        _musicSlider.value = Options.MusicVolume;
        _musicText.text = string.Format("{0}", Mathf.RoundToInt(Options.MusicVolume * 100));
    }

    public void IncrementSFXVolume()
    {
        Options.SetFXVolume(Options.FXVolume + 0.1f);

        _sfxSlider.value = Options.FXVolume;
        _sfxText.text = string.Format("{0}", Mathf.RoundToInt(Options.FXVolume * 100));
    }

    public void DecrementSFXVolume()
    {
        Options.SetFXVolume(Options.FXVolume - 0.1f);

        _sfxSlider.value = Options.FXVolume;
        _sfxText.text = string.Format("{0}", Mathf.RoundToInt(Options.FXVolume * 100));
    }

    public void SetUseSubtitles(bool use)
    {
        _subtitleButtons[Options.UseSubtitles ? 1 : 0].interactable = true;

        Options.SetUseSubtitles(use);

        _subtitleButtons[Options.UseSubtitles ? 1 : 0].interactable = false;
    }
}
