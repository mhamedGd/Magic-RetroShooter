using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfxHandler : MonoBehaviour
{
    [SerializeField] AudioClip[] footstepsSFX;
    [SerializeField] AudioSource footstepsAudioSource;

    [SerializeField] AudioClip[] jumpSFX;
    [SerializeField] AudioSource jumpAudioSource;

    public void PlayFootStep()
    {
        footstepsAudioSource.pitch = Random.Range(.8f, 1.2f);
        footstepsAudioSource.PlayOneShot(footstepsSFX[Random.Range(0, footstepsSFX.Length)]);
    }

    public void PlaySprintStep()
    {
        footstepsAudioSource.pitch = Random.Range(1.2f, 1.4f);
        footstepsAudioSource.PlayOneShot(footstepsSFX[Random.Range(0, footstepsSFX.Length)]);
    }

    public void PlayCrouchStep()
    {
        footstepsAudioSource.pitch = Random.Range(.6f, .8f);
        footstepsAudioSource.PlayOneShot(footstepsSFX[Random.Range(0, footstepsSFX.Length)]);
    }

    public void PlayJumpAudio()
    {
        jumpAudioSource.pitch = Random.Range(.8f, 1.2f);
        jumpAudioSource.PlayOneShot(jumpSFX[Random.Range(0, jumpSFX.Length)]);
    }
}
