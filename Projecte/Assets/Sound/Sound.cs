using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(.1f, 3f)]
	public float pitch = 1f;

	public bool loop = false;
    public bool dontRepeat = false;

	public AudioSource source; //no cal assignar-li res, el crearem des de l'audio manager

}
