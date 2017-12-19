using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{

	public static SoundEffectManager Instance = null;


	[SerializeField] private AudioClip clip_BallContact;
	[SerializeField] private AudioClip clip_MenuClose;
	[SerializeField] private AudioClip clip_MenuOpen;
	[SerializeField] private AudioClip clip_DominoFall;
	[SerializeField] private AudioClip[] clips_CanonShot;

	private AudioSource[] myAudioSources;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;

		myAudioSources = new AudioSource[1];

		GameObject audioSource1 = new GameObject("audioSource_1");
		Utils.AddAudioListener(audioSource1, false, 1.0f, false);
		audioSource1.transform.parent = transform;
		myAudioSources[0] = audioSource1.GetComponent<AudioSource>();

	}


	public void PlayOpenMenu()
	{
		PlaySound(clip_MenuOpen);   
	}

	public void PlayCloseMenu()
	{
		PlaySound(clip_MenuClose);
	}

	public void PlayBallContact()
	{
		PlaySound(clip_BallContact);
	}

	public void PlayDominoFall()
	{
		PlaySound(clip_DominoFall);
	}


	public void PlayCanonShot()
	{
		PlayRandomSound(clips_CanonShot);
	}



	private void PlaySound(AudioClip clip)
	{
		foreach (AudioSource aS in myAudioSources)
		{
			if (aS.playOnAwake == true)
			{
				continue;
			}
			else
			{
				Utils.PlaySound(aS, clip);
			}
		}
	}

	private void PlayRandomSound(AudioClip[] clips)
	{
		foreach (AudioSource aS in myAudioSources)
		{
			if (aS.playOnAwake == true)
			{
				continue;
			}
			else
			{
				Utils.PlayRandomSound(aS, clips);
			}
		}
	}



}
