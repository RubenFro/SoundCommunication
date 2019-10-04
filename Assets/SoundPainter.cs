using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPainter : MonoBehaviour
{
    public AudioSource startEndAudioSource;
    public AudioSource loopAudioSource;

    // 0 not playing, 1 play start, 2 in loop, 3 in end
    public int playingPhase = 0;
    private IEnumerator coroutine;
    public SoundLoop soundLoop;

    // Start is called before the first frame update
    void Start()
    {
        startEndAudioSource.clip = soundLoop.start;
        loopAudioSource.clip = soundLoop.loop;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (playingPhase == 3)
            {
                StopCoroutine(coroutine);
                Reset();

            }
            if (playingPhase == 0) { 
                coroutine = PlaySound();
                StartCoroutine(coroutine);
            }
           

        }
        if (Input.GetMouseButton(0) && playingPhase == 2)
        {
                ChangeLoopPitch(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        }


    }
    void ChangeLoopPitch(float pitchAdj,float panAdj)
    {
        loopAudioSource.pitch += pitchAdj*0.1f;
        loopAudioSource.panStereo += panAdj * 0.1f;
    }

 
    IEnumerator PlaySound()
    {
        PlayStart();
        yield return new WaitForSeconds(soundLoop.start.length);

        PlayLoop();
        yield return new WaitWhile(() => Input.GetMouseButton(0));

        PlayEnd();
        yield return new WaitForSeconds(soundLoop.end.length);
        Reset();
    }
    void Reset()
    {
        playingPhase = 0;
        startEndAudioSource.clip = soundLoop.start;
        startEndAudioSource.time = 0;
        startEndAudioSource.pitch = 1;
        loopAudioSource.volume = 1;
        loopAudioSource.loop = true;
        loopAudioSource.panStereo = 0;
    }
    void PlayStart()
    {
        playingPhase = 1;
        startEndAudioSource.clip = soundLoop.start;
        startEndAudioSource.time = 0;
        startEndAudioSource.pitch = 1;
        startEndAudioSource.Play();
    }
    void PlayLoop()
    {
        playingPhase = 2;
        loopAudioSource.time = 0;
        loopAudioSource.pitch = 1;
        loopAudioSource.Play();
        loopAudioSource.loop = true;
    }
    void PlayEnd()
    {
        StartCoroutine(FadeAudioVolume(loopAudioSource, loopAudioSource.volume, 0, 1));
        startEndAudioSource.clip = soundLoop.end;
        startEndAudioSource.time = 0;
        startEndAudioSource.pitch = loopAudioSource.pitch;
        startEndAudioSource.Play();
    }


    
    IEnumerator FadeAudioVolume(AudioSource audioSource, float startValue, float toValue, float aTime)
    {
        audioSource.volume = (startValue);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            audioSource.volume = (Mathf.Lerp(startValue, toValue, t));
            yield return null;
        }
        audioSource.volume = toValue;
        audioSource.loop = false;
        audioSource.Stop();
        playingPhase = 3;
    }




}

[System.Serializable]
public struct SoundLoop
{
    public AudioClip start;
    public AudioClip loop;
    public AudioClip end;
}
