using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject canvas;
    public GameObject home;
    public GameObject menu;
    public GameObject loadingScreenUI;
    public GameObject gameOver;
    public GameObject finish;
    public Slider progressBar;
    public Button startButton;
    public Button quitButton;
    public Button next;
    public Button retry;
    public Button quit;
    public Button back;
    public Button menuBtn;
    public List<Button> levels;
    private List<GameObject> myUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void UIManager(GameObject trueValue)
    {
        foreach(GameObject ui in myUI)
        {
            if (ui == trueValue)
            {
                ui.SetActive(true);
                break;
            }
            else
            {
                ui.SetActive(false);
            }
        }
    }
    void Start()
    {
        instance.UIManager(home);
        myUI = new List<GameObject> {home, menu, gameOver, finish, canvas, loadingScreenUI};

        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        next.onClick.AddListener(Next);
        retry.onClick.AddListener(Retry);
        menuBtn.onClick.AddListener(Menu);
        quit.onClick.AddListener(QuitGame);


        foreach (Button level in levels)
        {
            Button capturedButton = level;
            capturedButton.onClick.AddListener(() => LevelOpen(capturedButton));
        }
    }

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(canvas);
        }
        else
        {
            Destroy(canvas);
        }
    }

    // Update is called once per frame
    public void StartGame()
    {
        instance.UIManager(menu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name));
    }

    public void Menu()
    {
        instance.UIManager(menu);
    }

    public void Next()
    {
        int curr_index = SceneManager.GetActiveScene().buildIndex;
        int next_index = curr_index + 1;

        if(next_index < SceneManager.sceneCountInBuildSettings)
        {
            string sceneName = SceneManager.GetSceneByBuildIndex(next_index).name;
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    public void Back()
    {
        instance.UIManager(home);
    }

    public void LevelOpen(Button level)
    {
        string sceneName = $"Level{level.GetComponentInChildren<TMP_Text>().text}";

        StartCoroutine(LoadSceneAsync(sceneName));


    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        instance.UIManager(loadingScreenUI);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            if(operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }
}
