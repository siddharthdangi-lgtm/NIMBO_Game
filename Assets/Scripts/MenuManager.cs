using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class menuScreenManager : MonoBehaviour
{
    public static menuScreenManager instance;

    public GameObject homeScreen;
    public GameObject menuScreen;
    public GameObject loadingScreen;
    public GameObject gameOverScreen;
    public GameObject gameScreen;
    public GameObject finishScreen;
    public GameObject pauseScreen;
    public Slider progressBar;
    public TMP_Text progressText;
    public Button startButton;
    public Button quitButton;
    public Button next;
    public Button retry;
    public Button back;
    public Button menuBtn;
    public Button backwardBtn;
    public Button forwardBtn;
    public Button jumpBtn;
    public Button pause;
    public Button resume;
    public List<Button> levels;
    public List<GameObject> hearts;
    public Sprite emptyHeartImage;
    public Sprite heartImage;
    private List<GameObject> myUI;
    private Data_Saver saver;
    public bool isJump;
    public int direction;

    public void Forward() => direction = 1;
    public void Backward() => direction = -1;

    public void Stoping() => direction = 0;
    public void IsJump() => isJump = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void UIManager(GameObject trueValue)
    {
        foreach (GameObject ui in myUI) ui.SetActive(ui == trueValue);
    }
    void Start()
    {
        saver = new Data_Saver();
        myUI = new List<GameObject> {homeScreen, gameOverScreen, finishScreen, loadingScreen, menuScreen, gameScreen, pauseScreen};
        instance.UIManager(homeScreen);

        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        next.onClick.AddListener(Next);
        retry.onClick.AddListener(Retry);
        menuBtn.onClick.AddListener(Menu);
        back.onClick.AddListener(Back);
        jumpBtn.onClick.AddListener(IsJump);
        pause.onClick.AddListener(PauseGame);
        resume.onClick.AddListener(Resume);
        

        AddPointerEvents(forwardBtn, Forward, Stoping);
        AddPointerEvents(backwardBtn, Backward, Stoping);


        foreach (Button level in levels)
        {
            Button capturedButton = level;
            capturedButton.onClick.AddListener(() => LevelOpen(capturedButton));
        }
    }

    public void LevelOpen()
    {
        Dictionary<string, bool> dictionary = saver.Load();
        foreach(KeyValuePair<string, bool> kvp in dictionary)
        {
            foreach(var level in levels)
            {
                string text = level.transform.GetChild(0).GetComponent<TMP_Text>().text;
                if (text[text.Length - 1] == kvp.Key[kvp.Key.Length - 1])
                {
                    level.transform.GetChild(1).gameObject.SetActive(false);
                    level.GetComponent<Button>().interactable = false;
                }
                                      
            }
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void StartGame()
    {
        instance.UIManager(menuScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }

    public void Menu()
    {
        instance.UIManager(menuScreen);
    }

    public void Resume()
    {
        instance.UIManager(gameScreen);
    }

    public void Next()
    {
        int curr_index = SceneManager.GetActiveScene().buildIndex;
        int next_index = curr_index + 1;

        if(next_index < SceneManager.sceneCountInBuildSettings - 1)
        {
            StartCoroutine(LoadSceneAsync(next_index));
        }
    }

    public void Back()
    {
        instance.UIManager(homeScreen);
    }

    public void PauseGame()
    {
        instance.UIManager(pauseScreen);
    }

    public void LevelOpen(Button level)
    {
        if (levels.IndexOf(level) > SceneManager.sceneCountInBuildSettings - 1)
        {
            
        }
        else
        {
            int sceneIndx = levels.IndexOf(level);
            StartCoroutine(LoadSceneAsync(sceneIndx));
        }
            
    }
    IEnumerator LoadSceneAsync(int sceneIndx)
    {
        instance.UIManager(loadingScreen);

        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndx);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = progress + "%";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                instance.UIManager(null);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }


    }

    public void StartHearts()
    {
        foreach (GameObject heart in hearts)
        {
            if (heart.GetComponent<Image>() == null)
            {
                heart.GetComponent<Image>().sprite = heartImage;
            }
        }
    }

    public void HeartMechanism(int idx)
    {
        hearts[idx].GetComponent<Image>().sprite = emptyHeartImage;
    }

    private void AddPointerEvents(Button btn, System.Action onDown, System.Action onUp)
    {
        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { onDown();});
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { onUp(); });
        trigger.triggers.Add(entryUp);

    }
}
