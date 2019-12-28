using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    private static Utils m_Instance;
    private delegate void Del(GameObject o);

    private static Utils instance {
        get {
            if (m_Instance == null) {
                GameObject o = new GameObject();
                m_Instance = o.AddComponent<Utils>();
            }
            return m_Instance;
        }
    }

	public static bool LOCAL {
        get {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TestLocal";
        }
    }

    private IEnumerator wait(float t) {
        yield return new WaitForSeconds(t);
    }

    public static IEnumerator sleep(float t) {
        yield return new WaitForSeconds(t);
    }
    
    public static void PhotonDestroy(GameObject o, float t) {
        m_Instance.StartCoroutine(m_Instance._PhotonDestroy(o, t));
    }

    private IEnumerator _PhotonDestroy(GameObject o, float t) {
        yield return new WaitForSeconds(t);
        PhotonNetwork.Destroy(o);
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
