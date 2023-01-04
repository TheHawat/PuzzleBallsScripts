using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IAudioManager : IInitializable
{
    new void Initialize();
    void PlayJazz();
    void PlayTheSoundOf(string input);
    void Start();
}
public class AudioManager : MonoBehaviour, IAudioManager
{
    private AudioSource[] _audioData;
    private AudioSource _megaphone;
    private Dictionary<string, int> _soundPath;
    public void Initialize() { }
    public void PlayJazz() {
        int SoundIndex = _soundPath["Jazz"];
        _audioData[SoundIndex].loop = true;
        _audioData[SoundIndex].Play();
    }
    public void PlayTheSoundOf(string input) {
        int SoundIndex = _soundPath[input];
        AudioClip ClipToPlay = _audioData[SoundIndex].clip;
        _megaphone.PlayOneShot(ClipToPlay);
    }
    public void Start() {
        _megaphone = GetComponent<AudioSource>();
        _audioData = GetComponents<AudioSource>();
        _soundPath = new() {
                { "Jazz", 0 },
                { "Ceiling", 1 },
                { "Pop", 2 },
                { "Bounce", 3 },
                { "Swing", 4 },
                { "Shoot", 5 }
            };
    }
}