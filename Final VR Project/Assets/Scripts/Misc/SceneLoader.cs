using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    public bool UseSceenFader = true;

    public float ScreenFadeTime = 0.5f;

    private ScreenFader sf;

    private string _loadSceneName = string.Empty;

    public void LoadScene(string SceneName)
    {
        _loadSceneName = SceneName;

        if (UseSceenFader)
        {
            StartCoroutine("FadeThenLoadScene");
        }
        else
        {
            SceneManager.LoadScene(_loadSceneName, loadSceneMode);
        }
    }

    public IEnumerator FadeThenLoadScene()
    {

        if (UseSceenFader)
        {
            if (sf == null)
            {
                sf = FindObjectOfType<ScreenFader>();
                if (sf != null)
                {
                    sf.DoFadeIn();
                }
            }
        }

        if (ScreenFadeTime > 0)
        {
            yield return new WaitForSeconds(ScreenFadeTime);
        }

        SceneManager.LoadScene(_loadSceneName, loadSceneMode);
    }
}