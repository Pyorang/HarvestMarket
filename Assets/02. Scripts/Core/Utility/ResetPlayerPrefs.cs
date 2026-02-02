using UnityEngine;
using UnityEditor;

public class ResetPlayerPrefs : MonoBehaviour
{
    [MenuItem("Window/PlayerPrefs √ ±‚»≠")]
    private static void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs has been reset.");
    }
}