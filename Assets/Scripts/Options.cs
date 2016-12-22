using UnityEngine;

public class Options
{
    private const string KEY_AUTO_TUTORIAL = "auto-tut";
    private static bool _autoTutorial;

    //gameplay
    private const string KEY_ACCEL = "accel";
    private static int _accel;
    private const int DEF_ACCEL = 1;

    private const string KEY_LEFT_HAND = "left-handed";
    private static bool _leftHanded;
    private const bool DEF_LEFT_HAND = false;

    //audio
    private const string KEY_MASTER_VOLUME = "master-volume";
    private static float _masterVolume;
    private const float DEF_MASTER_VOLUME = 0.5f;

    private const string KEY_DIALOGUE_VOLUME = "dialgue-volume";
    private static float _dialogueVolume;
    private const float DEF_DIALOGUE_VOLUME = 0.5f;

    private const string KEY_MUSIC_VOLUME = "music-volume";
    private static float _musicVolume;
	private const float DEF_MUSIC_VOLUME = 0.5f;

    private const string KEY_FX_VOLUME = "effects-volume";
    private static float _fxVolume;
	private const float DEF_FX_VOLOUME = 0.5f;

    private const string KEY_USE_SUBTITLES = "subtitles";
    private static bool _useSubtitles;
    private const bool DEF_USE_SUBTITLES = false;

	#region Game Options

    public static bool AutoTutorial
    {
        get { return _autoTutorial; }
    }

    public static int Acceleration
    {
        get { return _accel; }
    }

    public static bool LeftHanded
    {
        get { return _leftHanded; }
    }

    public static float MasterVolume
    {
        get { return _masterVolume; }
    }

    public static float DialogueVolume
    {
        get { return _dialogueVolume; }
    }

    public static float MusicVolume
	{
		get { return _musicVolume; }
	}

	public static float FXVolume
	{
		get { return _fxVolume; }
	}

    public static bool UseSubtitles
    {
        get { return _useSubtitles; }
    }

    public static void SetAutoTutorial(bool auto)
    {
        _autoTutorial = auto;

        PlayerPrefs.SetInt(KEY_AUTO_TUTORIAL, _autoTutorial ? 1 : 0);
    }

    public static void SetAcceleration(int acceleration)
    {
        _accel = acceleration;

        PlayerPrefs.SetInt(KEY_ACCEL, _accel);
    }

    public static void SetLeftHanded(bool left)
    {
        _leftHanded = left;

        PlayerPrefs.SetInt(KEY_LEFT_HAND, _leftHanded ? 1 : 0);
    }

    public static void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp(volume, 0, 1);

        PlayerPrefs.SetFloat(KEY_MASTER_VOLUME, _masterVolume);
    }

    public static void SetDialogueVolume(float volume)
    {
        _dialogueVolume = Mathf.Clamp(volume, 0, 1);

        PlayerPrefs.SetFloat(KEY_DIALOGUE_VOLUME, _dialogueVolume);
    }

    public static void SetMusicVolume(float volume)
	{
		_musicVolume = Mathf.Clamp(volume, 0, 1);

		PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, _musicVolume);
	}

	public static void SetFXVolume(float volume)
	{
		_fxVolume = Mathf.Clamp(volume, 0, 1);

		PlayerPrefs.SetFloat(KEY_FX_VOLUME, _fxVolume);
	}

    public static void SetUseSubtitles(bool use)
    {
        _useSubtitles = use;

        PlayerPrefs.SetInt(KEY_USE_SUBTITLES, _useSubtitles ? 1 : 0);
    }

	#endregion
	
	public static void Save()
	{
		PlayerPrefs.Save();
	}

	public static void Load()
	{
        _autoTutorial = Mathf.Clamp(PlayerPrefs.GetInt(KEY_AUTO_TUTORIAL, 1), 0, 1) == 1;

        _accel = Mathf.Clamp(PlayerPrefs.GetInt(KEY_ACCEL, DEF_ACCEL), 0, 2);
        _leftHanded = Mathf.Clamp(PlayerPrefs.GetInt(KEY_LEFT_HAND, 0), 0, 1) == 1;

        _masterVolume = Mathf.Clamp(PlayerPrefs.GetFloat(KEY_MASTER_VOLUME, DEF_MASTER_VOLUME), 0, 1);
        _dialogueVolume = Mathf.Clamp(PlayerPrefs.GetFloat(KEY_DIALOGUE_VOLUME, DEF_DIALOGUE_VOLUME), 0, 1);
        _musicVolume = Mathf.Clamp(PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, DEF_MUSIC_VOLUME), 0, 1);
		_fxVolume = Mathf.Clamp(PlayerPrefs.GetFloat(KEY_FX_VOLUME, DEF_FX_VOLOUME), 0, 1);
        _useSubtitles = Mathf.Clamp(PlayerPrefs.GetInt(KEY_USE_SUBTITLES, 0), 0, 1) == 1;
    }

	/// <summary>
	/// Resets all options to defaults
	/// </summary>
	public static void Reset()
	{
        _accel = DEF_ACCEL;
        _leftHanded = DEF_LEFT_HAND;

        _masterVolume = DEF_MASTER_VOLUME;
        _dialogueVolume = DEF_DIALOGUE_VOLUME;
        _musicVolume = DEF_MUSIC_VOLUME;
		_fxVolume = DEF_FX_VOLOUME;
        _useSubtitles = DEF_USE_SUBTITLES;
	}
}
