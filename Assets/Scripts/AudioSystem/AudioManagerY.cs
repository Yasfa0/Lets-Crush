using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AudioManagerY : MonoBehaviour
{
    public static AudioManagerY Instance { get; private set; }

    private SettingData settingData;


    /* Audio Player Index
     * 0 = BGM
     * 1 = SFX
     * 2 = Dialogue
     */
    [SerializeField] private List<GameObject> audioPlayers = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        CheckVolume();
    }
    
    public void CheckVolume()
    {
        for (int i = 0; i < audioPlayers.Count; i++)
        {
            audioPlayers[i].GetComponent<AudioSource>().volume = GameSetting.LoadSetting().audioVolume[i];
        }
    }

    public void PlayAudio(AudioClip targetClip,int audioType)
    {
        AudioSource audioSource = audioPlayers[audioType].GetComponent<AudioSource>();
        audioSource.clip = targetClip;
        audioSource.loop = false;
        audioSource.volume = GameSetting.LoadSetting().audioVolume[audioType];
        audioSource.Play();
    }

    public void PlayLoopingAudio(AudioClip targetClip, int audioType)
    {

        AudioSource audioSource = audioPlayers[audioType].GetComponent<AudioSource>();
        audioSource.clip = targetClip;
        audioSource.loop = true;
        audioSource.volume = GameSetting.LoadSetting().audioVolume[audioType];
        audioSource.Play();
    }

    public void PlayNewAudio(AudioClip targetClip, int audioType, bool isLooping)
    {
        AudioSource audioSource = audioPlayers[audioType].AddComponent<AudioSource>();
        audioSource.clip = targetClip;
        audioSource.loop = isLooping;
        audioSource.volume = GameSetting.LoadSetting().audioVolume[audioType];
        audioSource.Play();
    }

    public void StopAudioChannel(int audioType)
    {
        foreach (AudioSource source in audioPlayers[audioType].GetComponents<AudioSource>())
        {
            Destroy(source);
        }
    }

    public List<GameObject> GetAudioPlayers()
    {
        if (audioPlayers != null)
        {
            return audioPlayers;
        }
        else
        {
            return null;
        }
    }

    public void ReplaceAudioSlot(AudioClip targetClip,int audioPlayerIndex, int slot)
    {
        List<AudioSource> audioSources = new List<AudioSource>();

        foreach (AudioSource audioPlayer in audioPlayers[audioPlayerIndex].GetComponents<AudioSource>())
        {
            audioSources.Add(audioPlayer);
        }

        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[slot].clip = targetClip;
            audioSources[slot].Play();
        }
    }

    public AudioSource CreateAudioSource(int audioPlayerIndex,float volume)
    {
        AudioSource newSource = audioPlayers[audioPlayerIndex].AddComponent<AudioSource>();
        return newSource;
    }

    public List<AudioSource> CreateSubAudioSource(List<AudioClip> audioClips,int sourceIndex)
    {
        //Bikin list untuk di return di akhir
        List<AudioSource> tempList = new List<AudioSource>();

        //Bikin game object baru, jadiin child audio source dari salah satu 3 index utama
        GameObject subSource = new GameObject();
        subSource.name = "Sub Source";
        subSource.transform.SetParent(audioPlayers[sourceIndex].transform);

        //Bikin Audio Source sebanyak jumlah audio clip sekaligus masukan audio clip.
        for (int i = 0; i < audioClips.Count; i++)
        {
            AudioSource tempSource = subSource.AddComponent<AudioSource>();
            tempSource.clip = audioClips[i];
            tempSource.volume = GameSetting.LoadSetting().audioVolume[sourceIndex];
            //Masukan semua Audio Source itu kedalam list
            tempList.Add(tempSource);
        }
        //Return list Audio Source nya
        return tempList;
    }

}
