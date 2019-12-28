using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    bool canPlay = true;
    public AudioSource audio_source;

    public void PlayAudio(AudioClip clip, AudioSource audio_source) {
        if (canPlay) {
            canPlay = false;
            audio_source.PlayOneShot(clip);
            StartCoroutine(Reset());
        }
    }

    public void PlayAudio(AudioSource audio_source) {
        PlayAudio(audio_source.clip, audio_source);
    }

    public void PlayAudio(AudioClip clip) {
        PlayAudio(clip, audio_source);
    }

    IEnumerator Reset() {
        yield return new WaitForSeconds(.2f);
        canPlay = true;
    }

    /*
    public AudioClip clip;
    public void Update() {
        PlayAudio(clip, test);
    }
    */
}
