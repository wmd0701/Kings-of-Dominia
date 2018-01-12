using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    //------------------------------------------------------
    //Singleton
    //------------------------------------------------------
    public static SoundEffectManager Instance = null;
    //------------------------------------------------------
    //Soundeffekte
    //------------------------------------------------------
    [SerializeField] private AudioClip m_CanonDominoContact;
	[SerializeField] private AudioClip m_MenuClose;
	[SerializeField] private AudioClip m_MenuOpen;
	[SerializeField] private AudioClip m_DominoFall;
	[SerializeField] private AudioClip m_DominoSetting;
	[SerializeField] private AudioClip[] m_CannonShots;
	[SerializeField] private AudioClip m_GateClose;
	[SerializeField] private AudioClip m_GateOpen;
	[SerializeField] private AudioClip m_LeverContact;
    [SerializeField] private AudioClip m_WinSound;
    //------------------------------------------------------
    //Liste mit Sources
    //------------------------------------------------------
    private List<AudioSource> m_AddedSources = new List<AudioSource>();

    private void Awake()
	{
        //------------------------------------------------------
        //Erstelle eine Instanz falls nicht vorhanden
        //------------------------------------------------------
        if (Instance == null)
        {
            Instance = this;
        }
        //------------------------------------------------------
        //Oder lösche falls diese Instanz nicht die Instanz ist
        //------------------------------------------------------
        else if (Instance != this)
            Destroy(this);
        //------------------------------------------------------
        //Füge eine Standartsource hinzu
        //------------------------------------------------------
        m_AddedSources.Add(SoundUtilities.AddSourceToObject(gameObject, false, 1.0f, false));
        //------------------------------------------------------
        //Starte Cleanup Routine
        //------------------------------------------------------
        StartCoroutine(CleanUp());
    }

    #region Play Sound Functions

    public void PlayOpenMenu()
	{
		PlaySound(m_MenuOpen);   
	}

	public void PlayCloseMenu()
	{
		PlaySound(m_MenuClose);
	}

	public void PlayCanonDominoContact()
	{
		PlaySound(m_CanonDominoContact);
	}

	public void PlayDominoFall()
	{
		PlaySound(m_DominoFall, 0.7f, false);
	}

	public void PlayDominoSetting()
	{
		PlaySound(m_DominoSetting);
	}

	public void PlayCannonShot()
	{
        PlayRandomSound(m_CannonShots);
	}

	public void PlayGateClose()
	{
		PlaySound(m_GateClose);
	}

	public void PlayGateOpen()
	{
        PlaySound(m_GateOpen);
	}

	public void PlayLeverContact()
	{
		PlaySound(m_LeverContact);
	}

    public void PlayWinSound()
    {
        PlaySound(m_WinSound);
    }

    #endregion

    /// <summary>
    /// Spielt einen Clip
    /// </summary>
    /// <param name="pi_Clips">Clip</param>
    /// <param name="pi_Volume">Optional Lautstärke</param>
    /// <param name="pi_HasToPlay">Optional muss gespielt werden</param>
    private void PlaySound(AudioClip pi_Clip, float pi_Volume = 1.0f, bool pi_HasToPlay = true)
    {
        //------------------------------------------------------
        //Versuche eine Source zu finden die nicht benutzt wird
        //------------------------------------------------------
        foreach (AudioSource b_Source in m_AddedSources)
        {
            if (!b_Source.isPlaying)
            {
                //------------------------------------------------------
                //Spiele ggf.
                //------------------------------------------------------
                float l_OldVol = b_Source.volume;
                b_Source.volume = pi_Volume;
                SoundUtilities.PlaySound(b_Source, pi_Clip);
                b_Source.volume = l_OldVol;
                return;
            }
        }
        //------------------------------------------------------
        //Falls der Sound wichtig ist
        //------------------------------------------------------
        if (pi_HasToPlay)
        {
            //------------------------------------------------------
            //Füge neue Source hinzu
            //------------------------------------------------------
            m_AddedSources.Add(SoundUtilities.AddSourceToObject(gameObject, false, 1.0f, false));
            //------------------------------------------------------
            //Versuche erneut
            //------------------------------------------------------       
            PlaySound(pi_Clip, pi_Volume);
        }
	}

    /// <summary>
    /// Spielt einen zufälligen Clip
    /// </summary>
    /// <param name="pi_Clips">Clips</param>
    /// <param name="pi_Volume">Optional Lautstärke</param>
    /// <param name="pi_HasToPlay">Optional muss gespielt werden</param>
    private void PlayRandomSound(AudioClip[] pi_Clips, float pi_Volume = 1.0f, bool pi_HasToPlay = true)
    {
        //------------------------------------------------------
        //Versuche eine Source zu finden die nicht benutzt wird
        //------------------------------------------------------
        foreach (AudioSource b_Source in m_AddedSources)
        {
            if (!b_Source.isPlaying)
            {
                //------------------------------------------------------
                //Spiele ggf.
                //------------------------------------------------------
                float l_OldVol = b_Source.volume;
                b_Source.volume = pi_Volume;
                SoundUtilities.PlayRandomSound(b_Source, pi_Clips);
                b_Source.volume = l_OldVol;
                return;
            }
        }
        //------------------------------------------------------
        //Falls der Sound wichtig ist
        //------------------------------------------------------
        if (pi_HasToPlay)
        {
            //------------------------------------------------------
            //Füge neue Source hinzu
            //------------------------------------------------------
            m_AddedSources.Add(SoundUtilities.AddSourceToObject(gameObject, false, 1.0f, false));
            //------------------------------------------------------
            //Versuche erneut
            //------------------------------------------------------
            PlayRandomSound(pi_Clips, pi_Volume);
        }       
    }

    /// <summary>
    /// Bereinigt unbenutzte AudioSources in regelmäßigen Abständen
    /// </summary>
    private IEnumerator CleanUp()
    {
        while (true)
        {
            //------------------------------------------------------
            //Entferne inaktive Sources (bei mehr als einer Source)
            //------------------------------------------------------
            if (m_AddedSources.Count > 1)
            {
                for(int i = m_AddedSources.Count - 1; i > 0; i--)
                {
                    if (!m_AddedSources[i].isPlaying)
                    {
                        Destroy(m_AddedSources[i]);
                        m_AddedSources.RemoveAt(i);
                    }
                }
            }
            //------------------------------------------------------
            //Warte eine bestimmte Zeit bis zum nächsten Sweep
            //------------------------------------------------------
            yield return new WaitForSeconds(10.0f);
        }
    }
}