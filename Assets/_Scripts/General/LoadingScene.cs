using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI ProgressText;
    public Image ProgressBar;

    private int _loadProgress = 0;

    void Start()
    {
        StartCoroutine(DisplayLoadingScreen(PlayerPrefs.GetInt("SceneToLoad")));
    }

    IEnumerator DisplayLoadingScreen(int level)
    {
        ProgressText.text = "Загрузка... " + _loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);
        while (!async.isDone)
        {
            _loadProgress = (int)(async.progress * 100);
            ProgressText.text = "Загрузка... " + _loadProgress + "%";
            ProgressBar.fillAmount = async.progress;
            yield return null;
        }
    }
}