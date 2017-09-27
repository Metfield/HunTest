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
    List<AudioClip> characterDeaths;

    public void Init(Main inMain)
    {
        main        = inMain;
        snd         = new GameObject("Sound");
        audioSource = snd.AddComponent<AudioSource>();

		audioClips = new Dictionary<string, AudioClip>();
		
		AddAudioClip("Gun", "Audio/Gun");
        AddAudioClip("PlayerDeath", "Audio/Character/Death/PlayerDeath");
        
        //Load BGM
        musicSource = snd.AddComponent<AudioSource>();
        musicSource.clip = Resources.Load<AudioClip>("Audio/BGM");
        musicSource.loop = true;
        musicSource.volume = 0.4f;

        LoadCharacterGrunts();
        LoadCharacterDeaths();
    }

    public void PlayAudioClip(string inVer)
    {
        switch(inVer)
        {
            case "Grunt":
                audioSource.PlayOneShot(characterGrunts[Random.Range(0, 7)], 0.7f);
                break;

            case "Death":
                audioSource.PlayOneShot(characterDeaths[Random.Range(0, 6)]);
                break;

            default:
                audioSource.PlayOneShot(audioClips[inVer], 0.4f);
                break;
        }
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

    private void LoadCharacterDeaths()
    {
        characterDeaths = new List<AudioClip>();

        for(int i = 0; i < 8; i++)        
            characterDeaths.Add(Resources.Load<AudioClip>("Audio/Character/Death/Death" + i));
    }

    public void AddAudioClip(string inId, string inAddress)
    {
        AudioClip tClip = Resources.Load<AudioClip>(inAddress);
        audioClips.Add(inId, tClip);
        tClip.LoadAudioData();
    }
}
