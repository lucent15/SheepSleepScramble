using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public GameObject playpanel;
    public GameObject rulepanel;
    public GameObject rule1;
    public GameObject rule2;
    public GameObject rule3;
    public GameObject shutdownconfirmpanel;

    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private Slider _slider;

    private void Start() { Time.timeScale = 1.0f; }
    public void StartGame() { SceneManager.LoadScene("SampleScene"); }
    public void LoadNextScene()
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("SampleScene");
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }
    public void OpenRule()
    {
        playpanel.SetActive(false);
        rulepanel.SetActive(true);
        rule1.SetActive(true);
        rule2.SetActive(false);
        rule3.SetActive(false);
    }
    public void Page1()
    {
        rule1.SetActive(true);
        rule2.SetActive(false);
        rule3.SetActive(false);
    }
    public void Page2()
    {
        rule1.SetActive(false);
        rule2.SetActive(true);
        rule3.SetActive(false);
    }
    public void Page3()
    {
        rule1.SetActive(false);
        rule2.SetActive(false);
        rule3.SetActive(true);
    }
    public void OpenHowToPlay()
    {
        rulepanel.SetActive(false);
        playpanel.SetActive(true);
    }
    public void AllClose()
    {
        rulepanel.SetActive(false);
        playpanel.SetActive(false);
        rule1.SetActive(false);
        rule2.SetActive(false);
        rule3.SetActive(false);
        rulepanel.SetActive(false);
        shutdownconfirmpanel.SetActive(false);
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShutDownConfirmOpen()
    {
        AllClose();
        shutdownconfirmpanel.SetActive(true);
    }
    public void ShutDown() { UnityEngine.Application.Quit(); }
}
