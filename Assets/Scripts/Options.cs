using UnityEngine;

public class Options
{
    private const string KEY_AUTO_TUTORIAL = "auto-tut";
    private static bool _autoTutorial;

    //gameplay
    private const string KEY_LEFT_HAND = "left-handed";
    private static bool _leftHanded;
    private const bool DEF_LEFT_HAND = false;

	//audio
	private const string KEY_MUSIC_VOLUME = "music-volume";
	private const string KEY_FX_VOLUME = "effects-volume";

	private static float _musicVolume;
	private const float DEF_MUSIC_VOLUME = 0.5f;

	private static float _fxVolume;
	private const float DEF_FX_VOLOUME = 0.5f;

	#region Game Options

    public static bool AutoTutorial
    {
        get { return _autoTutorial; }
    }

    public static bool LeftHanded
    {
        get { return _leftHanded; }
    }

	public static float MusicVolume
	{
		get { return _musicVolume; }
	}

	public static float FXVolume
	{
		get { return _fxVolume; }
	}

    public static void SetAutoTutorial(bool auto)
    {
        _autoTutorial = auto;

        PlayerPrefs.SetInt(KEY_AUTO_TUTORIAL, _autoTutorial ? 1 : 0);
    }

    public static void SetLeftHanded(bool left)
    {
        _leftHanded = left;

        PlayerPrefs.SetInt(KEY_LEFT_HAND, _leftHanded ? 1 : 0);
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

	#endregion

	#region Save Data

	

	#endregion
	
	public static void Save()
	{
		PlayerPrefs.Save();
	}

	public static void Load()
	{
        _autoTutorial = PlayerPrefs.GetInt(KEY_AUTO_TUTORIAL, 1) == 1;

        _leftHanded = PlayerPrefs.GetInt(KEY_LEFT_HAND, 0) == 1;

		_musicVolume = Mathf.Clamp(PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, DEF_MUSIC_VOLUME), 0, 1);
		_fxVolume = Mathf.Clamp(PlayerPrefs.GetFloat(KEY_FX_VOLUME, DEF_FX_VOLOUME), 0, 1);
	}

	/// <summary>
	/// Resets all options to defaults
	/// </summary>
	public static void Reset()
	{
        _leftHanded = DEF_LEFT_HAND;

        _musicVolume = DEF_MUSIC_VOLUME;
		_fxVolume = DEF_FX_VOLOUME;
	}

	public static void Apply()
	{
		
	}
}
