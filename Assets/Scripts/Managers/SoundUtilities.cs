using UnityEngine;
using UnityEngine.Audio;

public class SoundUtilities : MonoBehaviour {

	/// <summary>
	/// Gets a random index for a given array
	/// </summary>
	public static int GetRndIndex(int arrayLength)
	{
		return Random.Range(0, arrayLength);
	} 

    /// <summary>
    /// Get random rotation around y-axis
    /// </summary>
	public static Quaternion GetRndStandRotation()
	{
		return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
	}

	/// <summary>
	/// Plays a random clip from a given clip soundbank on a given AudioSource component
	/// </summary>
	public static void PlayRandomSound(AudioSource source, AudioClip[] clips)
	{
		if (clips.Length != 0)
		{
			if (source != null)
			{
				source.clip = clips[SoundUtilities.GetRndIndex(clips.Length)];
				source.Play();
			}
			else
			{
				Debug.LogWarning("No AudioSource attached!");
			}

		}
		else
		{
			Debug.LogWarning("No audio clip attached!");
		}
	}

    /// <summary>
    /// Plays given sound on given source
    /// </summary>
    /// <param name="source">Audiosource</param>
    /// <param name="clip">Audioclip</param>
	public static void PlaySound(AudioSource source, AudioClip clip)
	{
		if (source != null)
		{
			source.clip = clip;
			source.Play();
		}
		else
		{
			Debug.LogWarning("No AudioSource attached!");
		}
	} 

    /// <summary>
    /// Stops source from playing audio
    /// </summary>
    /// <param name="source">Audiosource</param>
	public static void StopSound(AudioSource source)
	{
		if (source != null)
		{
			source.Stop();
		}
		else
		{
			Debug.LogWarning("No AudioSource attached!");
		}
	} 


	/// <summary>
	/// Gets the distance between two objects, set a the same height.
	/// </summary>
	public static float GetRelativeDistance(Vector3 position1, Vector3 position2)
	{
		// set the height of pos1 as for pos2
		Vector3 relativePosition1 = new Vector3
			(
				position1.x,
				position2.y,
				position1.z
			);

		return Vector3.Distance(relativePosition1, position2);
	}

	/// <summary>
	/// Gets the angle in (0, 360) of the given vector to the X axis of the world coordinate
	/// </summary>
	/// <param name="vec1"></param>
	public static float GetAbsoluteAngle(Vector2 vec1)
	{
		float aCos = Mathf.Acos(vec1.x) / Mathf.PI;
		float aSin = Mathf.Asin(vec1.y) / Mathf.PI;
		if (aSin < 0.0f)
		{
			aCos = 1.0f + (1 - aCos);
		}
		return aCos * 180;
	}

	#region AddAudioListener Overrides

    /// <summary>
    /// Adds audio listener to a gameobject
    /// </summary>
    /// <param name="toGameObject">Gameobject</param>    
	public static AudioSource AddAudioListener(GameObject toGameObject)
	{
		AudioSource aS = toGameObject.AddComponent<AudioSource>();
		aS.playOnAwake = false;
		if (SoundEffectManager.Instance != null)
		{
			//     aS.outputAudioMixerGroup = GetMasterGroup();
		}
		return aS;
	}

    /// <summary>
    /// Adds audio listener to a gameobject
    /// </summary>
    /// <param name="toGameObject">Gameobject</param>
    /// <param name="is3D">Bool if is for 3D sound</param>
	public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D)
	{
		AudioSource aS = toGameObject.AddComponent<AudioSource>();
		aS.playOnAwake = false;
		if (SoundEffectManager.Instance != null)
		{
			//      aS.outputAudioMixerGroup = GetMasterGroup();
		}
		if (is3D == true)
		{
			aS.spatialBlend = 1.0f;
		}
		else
		{
			aS.spatialBlend = 0.0f;
		}
		return aS;
	}

    /// <summary>
    /// Adds audio listener to gameobject
    /// </summary>
    /// <param name="toGameObject">Gameobject</param>
    /// <param name="is3D">Bool if is 3D sound</param>
    /// <param name="volume">Volume</param>
	public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D, float volume)
	{
		AudioSource aS = toGameObject.AddComponent<AudioSource>();
		aS.playOnAwake = false;
		if (SoundEffectManager.Instance != null)
		{
			//     aS.outputAudioMixerGroup = GetMasterGroup();
		}
		if (is3D == true)
		{
			aS.spatialBlend = 1.0f;
		}
		else
		{
			aS.spatialBlend = 0.0f;
		}
		aS.volume = volume;
		return aS;
	}

    /// <summary>
    /// Adds audio listener to gameobject
    /// </summary>
    /// <param name="toGameObject">Gameobject</param>
    /// <param name="is3D">Bool if is 3D sound</param>
    /// <param name="volume">Volume</param>
    /// <param name="isLoop">Bool if is looping</param>
	public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D, float volume, bool isLoop)
	{
		AudioSource aS = toGameObject.AddComponent<AudioSource>();
		aS.playOnAwake = false;
		if (SoundEffectManager.Instance != null)
		{
			//    aS.outputAudioMixerGroup = GetMasterGroup();
		}
		if (is3D == true)
		{
			aS.spatialBlend = 1.0f;
		}
		else
		{
			aS.spatialBlend = 0.0f;
		}
		aS.volume = volume;
		aS.loop = isLoop;
		return aS;
	}

    /// <summary>
    /// Adds audio listener to gameobject
    /// </summary>
    /// <param name="toGameObject">Gameobject</param>
    /// <param name="is3D">Bool if is 3D sound</param>
    /// <param name="volume">Volume</param>
    /// <param name="isLoop">Bool if is looping</param>
    /// <param name="audioMixerGroup">Audiomixtergroup</param>
	public static AudioSource AddAudioListener(GameObject toGameObject, bool is3D, float volume, bool isLoop, AudioMixerGroup audioMixerGroup)
	{
		AudioSource aS = toGameObject.AddComponent<AudioSource>();
		aS.playOnAwake = false;
		aS.outputAudioMixerGroup = audioMixerGroup;
		if (is3D == true)
		{
			aS.spatialBlend = 1.0f;
		}
		else
		{
			aS.spatialBlend = 0.0f;
		}
		aS.volume = volume;
		aS.loop = isLoop;
		return aS;
	}

	#endregion

}