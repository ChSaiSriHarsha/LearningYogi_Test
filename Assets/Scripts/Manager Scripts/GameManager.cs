using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header ("Astroid Related")]
    public int astroidCount = 0;

    [Header ("References")]
    [SerializeField] private Environment_AstroidController astroidControllerPrefab;
    [SerializeField] private Environment_AstroidScriptableObject astroidScriptableObject;

    [Header ("Texts")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI startGameText;

    [Header ("Transforms")]
    [SerializeField] private Transform menuObject;

    [Header ("Buttons")]
    [SerializeField] private Button restartButton;

    [Header ("Images")]
    [SerializeField] private Image bgImage;

    internal int playerScore = 0;

    private int level = 0;

    public static GameManager SharedInstance;

    #region INITIALIZATION

    private void Awake ()
    {
        SharedInstance = this;

        Player_SpaceshipController.SharedInstance.enabled = false;
        startGameText.DOFade (0f, 0f);
    }

    private void Start ()
    {
        currentScoreText.text = "Score: " + playerScore;

        menuObject.localScale = Vector3.zero;

        bgImage.DOFade (0f, 0f);

        playerScore = 0;

        Invoke ("EnablePlayerController", 2.5f);
    }

    private void EnablePlayerController ()
    {
        Player_SpaceshipController.SharedInstance.enabled = true;

        startGameText.DOFade (1f, 0.5f).SetEase (Ease.Linear).OnComplete (() =>
        {
            Invoke ("DisableText", 1f);
        });
    }

    private void DisableText ()
    {
        startGameText.DOFade (0f, 1f).SetEase (Ease.Linear);
    }

    #endregion

    #region LISTENERS

    private void OnEnable ()
    {
        restartButton.onClick.AddListener (OnRestartClicked);
    }

    private void OnDisable ()
    {
        restartButton.onClick.RemoveListener (OnRestartClicked);
    }

    #endregion

    private void Update ()
    {
        if (Player_SpaceshipController.SharedInstance.gameStarted)
            SetAstroids ();
    }

    #region SETTING ASTROIDS

    private void SetAstroids ()
    {
        //We check if there are no astroids left, if there are none, we spawn more
        if (astroidCount == 0)
        {
            //We increase difficulty
            level++;

            //We spawn astroids according to levels
            int noOfAstroids = astroidScriptableObject.noOfAstroids + (2 * level);

            for (int i = 0; i < noOfAstroids; i++)
            {
                SpawnAstroids ();
            }
        }
    }

    #endregion

    #region SPAWNING ASTROIDS

    private void SpawnAstroids ()
    {
        //Set offset
        float offset = Random.Range (0f, 1f);

        Vector2 viewportSpawnPosition = Vector2.zero;

        int setEdge = Random.Range (0, 4);

        if (setEdge == 0)
            viewportSpawnPosition = new Vector2 (offset, 0);
        else if (setEdge == 1)
            viewportSpawnPosition = new Vector2 (offset, 1);
        else if (setEdge == 2)
            viewportSpawnPosition = new Vector2 (0, offset);
        else if (setEdge == 3)
            viewportSpawnPosition = new Vector2 (1, offset);

        //Now we create Astroid
        Vector2 spawnPosition = Camera.main.ViewportToWorldPoint (viewportSpawnPosition);
        Environment_AstroidController astroid = Instantiate (astroidControllerPrefab, spawnPosition, Quaternion.identity);
        astroid.gameManager = this;
    }

    #endregion

    #region PLAYER SCORE HANDLER

    public void SetPlayerScore ()
    {
        playerScore++;
        currentScoreText.text = "Score: " + playerScore.ToString ();
    }

    #endregion

    #region HANDLE GAME OVER

    public void GameOver ()
    {
        finalScoreText.text = "Final Score: " + playerScore;

        bgImage.DOFade (0.7f, 0.5f).SetEase (Ease.Linear).OnComplete (() =>
        {
            menuObject.DOScale (1f, 0.25f).SetEase (Ease.OutBack);
        });
    }

    private void OnRestartClicked ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    #endregion
}
