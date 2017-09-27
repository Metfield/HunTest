using UnityEngine;
using System.Collections.Generic;

public class Snd : MonoBehaviour
{
    public Dictionary<string, AudioClip> audioClips;

    Main main;
    GameObject snd;
    AudioSource audioSource;
    AudioSource musicSource;

    List<AudioClip> characterGrunts;

    public void Init(Main inMain)
    {
        main        = inMain;
        snd         = new GameObject("Sound");
        audioSource = snd.AddComponent<AudioSource>();

		audioClips = new Dictionary<string, AudioClip>();
		
		AddAudioClip("Gun", "Audio/Gun");

        musicSource = snd.AddComponent<AudioSource>();
        musicSource.clip = Resources.Load<AudioClip>("Audio/BGM");
        musicSource.loop = true;
        musicSource.volume = 0.6f;

        LoadCharacterGrunts();
    }

    public void PlayAudioClip(string inVer)
    {
        if (inVer == "Grunt")
            audioSource.PlayOneShot(characterGrunts[Random.Range(0, 7)]);
        else
            audioSource.PlayOneShot(audioClips[inVer], 0.6f);
    }

    public void PlayBGM()
    {
        musicSource.Play();
    }

    private void LoadCharacterGrunts()
    {
        characterGrunts = new List<AudioClip>();

        for(int i = 0; i < 8; i++)        
            characterGrunts.Add(Resources.Load<AudioClip>("Audio/Character/Grunts/pain" + i));
    }

    public void AddAudioClip(string inId, string inAddress)
    {
        AudioClip tClip = Resources.Load<AudioClip>(inAddress);
        audioClips.Add(inId, tClip);
        tClip.LoadAudioData();
    }
}
