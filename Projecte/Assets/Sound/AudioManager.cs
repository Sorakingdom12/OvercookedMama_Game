using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject); //per tal devitar que hi hagi mes dun audio
		}
		else
		{
			instance = this;
			//DontDestroyOnLoad(gameObject); //no es borra quan canviem d'escena
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.pitch = s.pitch;
			s.source.volume = s.volume;
		}
	}


	public void Play(string sound)
	{
		Sound s = null;
		foreach (Sound t in sounds)
        {
			if (t.name == sound) s = t;
        }
		if (s == null)
		{
			Debug.Log("Sound: " + sound + " not found!");
			return;
		}
		s.source.pitch = s.pitch;
		s.source.volume = s.volume;

        if ((!s.source.isPlaying && s.dontRepeat) || !s.dontRepeat) s.source.Play();
	}


    public void Stop(string sound)
    {
		Sound s = null;
		foreach (Sound t in sounds)
		{
			if (t.name == sound) s = t;
		}
		if (s == null)
        {
            Debug.Log("Sound: " + sound + " not found!");
            return;
        }
		 s.source.Stop();
    }
}
