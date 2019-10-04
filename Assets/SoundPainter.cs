using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPainter : MonoBehaviour
{
    public AudioSource startEndAudioSource;
    public AudioSource loopAudioSource;
    public AudioClip[] audioClipSequence;
    private bool playingSound;
    private float yAxisStartMouse=-1f;
    private IEnumerator coroutine;
    public GameObject debugObject;

    // Start is called before the first frame update
    void Start()
    {
        startEndAudioSource.clip = audioClipSequence[0];
        loopAudioSource.clip = audioClipSequence[1];
        HideDebug();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!playingSound)
            {
                coroutine = PlaySound();
                StartCoroutine(coroutine);
                ShowDebug();
            }
           
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (playingSound)
            {
                StopSound();
                Invoke("HideDebug",0.25f);
            }
        }

        if (playingSound && loopAudioSource.isPlaying && Input.GetMouseButton(0))
        {
                ChangeLoopPitch(Input.GetAxis("Mouse Y"));
                MoveDebug(Input.GetAxis("Mouse Y"));
        }


    }
    void ChangeLoopPitch(float pitchAdj)
    {
            loopAudioSource.pitch += pitchAdj*0.01f;
    }


    void ShowDebug()
    {
        debugObject.SetActive(true);

    }
    void HideDebug()
    {
        debugObject.SetActive(false);
        ResetDebug();
    }
    void MoveDebug(float y)
    {
        debugObject.transform.position = new Vector3(debugObject.transform.position.x, debugObject.transform.position.y + y, debugObject.transform.position.z);
    }
    void ResetDebug()
    {
        debugObject.transform.position = Vector3.zero;
    }
    IEnumerator PlaySound()
    {
        playingSound = true;
        startEndAudioSource.clip = audioClipSequence[0];
        startEndAudioSource.time = 0;
        startEndAudioSource.pitch = 1;
        startEndAudioSource.Play();
        yield return new WaitForSeconds(audioClipSequence[0].length);
        loopAudioSource.time = 0;
        loopAudioSource.pitch = 1;
        loopAudioSource.Play();
        loopAudioSource.loop = true;

    }
    void StopSound()
    {
        if (playingSound) { 
            if (startEndAudioSource.isPlaying)
            {
                StopCoroutine(coroutine);
                startEndAudioSource.Stop();
            }
            startEndAudioSource.clip = audioClipSequence[2];
            startEndAudioSource.time = 0;
            startEndAudioSource.pitch = loopAudioSource.pitch;
            coroutine = null;
            if (loopAudioSource.isPlaying) { 
                loopAudioSource.Stop();
            }
            startEndAudioSource.Play();
            playingSound = false;
        
        }
    }
}
