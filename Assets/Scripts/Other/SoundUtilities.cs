using UnityEngine;
using UnityEngine.Audio;

public class SoundUtilities : MonoBehaviour
{
	/// <summary>
	/// Zufälliger Index basierend auf Länge
	/// </summary>
	public static int GetRndIndex(int arrayLength)
	{
		return Random.Range(0, arrayLength);
	} 

    /// <summary>
    /// Zufällige Rotation um Y
    /// </summary>
	public static Quaternion GetRndStandRotation()
	{
		return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
	}

	/// <summary>
	/// Spielt zufälligen Clip
	/// </summary>
	public static void PlayRandomSound(AudioSource source, AudioClip[] clips)
	{
		if (clips.Length != 0)
		{
			if (source != null)
			{
				source.clip = clips[GetRndIndex(clips.Length)];
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
    /// Spielt einen Clip
    /// </summary>
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
    /// Stoppt Clip
    /// </summary>
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
    /// Gibt Distanz zwischen zwei Positionen
    /// </summary>
	public static float GetRelativeDistance(Vector3 position1, Vector3 position2)
	{
		Vector3 relativePosition1 = new Vector3
			(
				position1.x,
				position2.y,
				position1.z
			);

		return Vector3.Distance(relativePosition1, position2);
	}

    /// <summary>
    /// Gibt Winkel eines Vektors
    /// </summary>
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

    #region Add source overrides

    /// <summary>
    /// Fügt AudioSource zu einem GameObject und gibt sie zurück
    /// </summary>
    /// <param name="toGameObject">GameObject</param>
    public static AudioSource AddSourceToObject(GameObject toGameObject)
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
    /// Fügt AudioSource zu einem GameObject und gibt sie zurück
    /// </summary>
    /// <param name="toGameObject">GameObject</param>
    /// <param name="is3D">Bool ob 3D Sound</param>
	public static AudioSource AddSourceToObject(GameObject toGameObject, bool is3D)
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
    /// Fügt AudioSource zu einem GameObject und gibt sie zurück
    /// </summary>
    /// <param name="toGameObject">GameObject</param>
    /// <param name="is3D">Bool ob 3D Sound</param>
    /// <param name="volume">Lautstärke</param>
	public static AudioSource AddSourceToObject(GameObject toGameObject, bool is3D, float volume)
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
    /// Fügt AudioSource zu einem GameObject und gibt sie zurück
    /// </summary>
    /// <param name="toGameObject">GameObject</param>
    /// <param name="is3D">Bool ob 3D Sound</param>
    /// <param name="volume">Lautstärke</param>
    /// <param name="isLoop">Bool ob loopend</param>
	public static AudioSource AddSourceToObject(GameObject toGameObject, bool is3D, float volume, bool isLoop)
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
    /// Fügt AudioSource zu einem GameObject und gibt sie zurück
    /// </summary>
    /// <param name="toGameObject">GameObject</param>
    /// <param name="is3D">Bool ob 3D Sound</param>
    /// <param name="volume">Lautstärke</param>
    /// <param name="isLoop">Bool ob loopend</param>
    /// <param name="audioMixerGroup">Audiomixter Gruppe</param>
	public static AudioSource AddSourceToObject(GameObject toGameObject, bool is3D, float volume, bool isLoop, AudioMixerGroup audioMixerGroup)
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