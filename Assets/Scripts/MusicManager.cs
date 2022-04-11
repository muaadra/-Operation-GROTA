using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles the background music, UI sounds, and playing other common "one shot" sounds 
/// </summary>
public class MusicManager : MonoBehaviour
{
    public AudioSource BGMusicAS; //background music
    public AudioClip dieExplosion;
    public List<AudioClip> clicks;
    public AudioClip constructionStarted;
    public AudioClip constructionCompleted;
    private AudioSource audioSrc;
    public static MusicManager manager;

    private void Awake() {
        manager = this;
    }

    private void Start() {
        audioSrc = GetComponent<AudioSource>();
    }

    /// <summary>
    /// playes one of the clicking sounds
    /// </summary>
    /// <param name="idx"></param> index of the sound
    public void playClick(int idx) {
        audioSrc.PlayOneShot(clicks[idx],0.7f);
    }

    /// <summary>
    /// plays the sound for when construction started
    /// </summary>
    public void playConstructionStarted() {
        audioSrc.PlayOneShot(constructionStarted);
    }

    /// <summary>
    /// plays the sound for when construction completed
    /// </summary>
    public void playConstructionCompleted() {
        audioSrc.PlayOneShot(constructionCompleted, 0.8f);
    }

    /// <summary>
    /// plays a given sound
    /// </summary>
    /// <param name="clip"></param> the clip to play
    /// <param name="volume"></param> at what volume
    public void playOneShot(AudioClip clip, float volume) {
        audioSrc.PlayOneShot(clip, volume);
    }
}
