using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;

    public SoundType soundType;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume = 1f;
    [Range(0.1f,3f)]
    public float pitch = 1f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public enum SoundType
{
    MASTER,
    MUSIC,
    SOUND
}

public class AudioManager : MonoBehaviour
{
    private AudioSource _as;
    
    public Sound[] sounds;

    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup soundMixer;

    public static AudioManager Instance;

    private void Awake()
	{
		if (Instance != null)
		{
			if (Instance != this)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this);
            foreach(Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.outputAudioMixerGroup = s.soundType switch
                {
                    SoundType.MASTER => masterMixer,
                    SoundType.MUSIC => musicMixer,
                    SoundType.SOUND => soundMixer,
                    _ => s.source.outputAudioMixerGroup
                };
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
		}
	}

    public bool Play (string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) 
        {
            Debug.LogError($"Sound {soundName} not found.");
            return false;
        }

        if (s.source.isPlaying)
            return false;
            
        s.source.Play();
        return true;
    }

    public void PlayAt(string soundName, Vector3 location)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogError($"Sound {soundName} not found.");
            return;
        }
        AudioSource.PlayClipAtPoint(s.clip, location);
    }
}
