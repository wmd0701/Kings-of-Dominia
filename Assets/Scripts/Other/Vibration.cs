using UnityEngine;

public static class Vibration
{
    //----------------------------------------
    //Deklariere nur falls Android-Device
    //----------------------------------------
    #if UNITY_ANDROID && !UNITY_EDITOR
	public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    #else
    public static AndroidJavaClass unityPlayer;
	public static AndroidJavaObject currentActivity;
	public static AndroidJavaObject vibrator;
	#endif

    /// <summary>
    /// Einmalige Vibration
    /// </summary>
    /// <param name="pi_Milliseconds">Länge in ms</param>
	public static void Vibrate(long pi_Milliseconds){
        //----------------------------------------
        //Vibriere falls Android-Device
        //----------------------------------------
        if (isAndroid())
			vibrator.Call("vibrate", pi_Milliseconds);
        else
        {
            #if !UNITY_STANDALONE && !UNITY_EDITOR
			Handheld.Vibrate();
            #endif
        }
	}

    /// <summary>
    /// Vibriere in einem Muster
    /// </summary>
    /// <param name="pi_Pattern">Muster [Pause in ms, Vibration in ms,..]</param>
    /// <param name="pi_RepeatStep">Wiederholungsindex im Muster</param>
	public static void Vibrate(long []pi_Pattern , int pi_RepeatStep){
        //----------------------------------------
        //Vibriere falls Android-Device
        //----------------------------------------
        if (isAndroid())
			vibrator.Call("vibrate", pi_Pattern, pi_RepeatStep);
        else
        {
            #if !UNITY_STANDALONE && !UNITY_EDITOR
			Handheld.Vibrate();
            #endif
        }
    }

    /// <summary>
    /// Bricht aktuelle Vibration ab
    /// </summary>
	public static void Cancel(){
		if (isAndroid())
			vibrator.Call("cancel");
	}

    /// <summary>
    /// Bool ob Gerät ein Android-Device ist
    /// </summary>
	private static bool isAndroid()
	{
        #if UNITY_ANDROID && !UNITY_EDITOR
		return true;
        #else
		return false;
        #endif
	}
}