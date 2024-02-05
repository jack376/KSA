using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public LivingEntity playerEntity;
	public LivingEntity monsterEntity;

	public GameObject titlePanel;
	public GameObject continuePanel;

	private bool isGameOver = false;

	public bool IsGameOver { get => isGameOver; }

	private void Awake()
	{
        if (Instance == null)
		{
            Instance = this;
        }
        else
		{
            Destroy(gameObject);
        }
    }

	private void OnEnable()
	{
        titlePanel.gameObject.SetActive(false);
		continuePanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerEntity.OnDeath += OnPlayerDeath;
        monsterEntity.OnDeath += OnMonsterDeath;
    }

    private void Update()
	{
        if (isGameOver)
		{
            if (Input.GetKeyDown(KeyCode.R))
			{
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

	private void OnPlayerDeath()
	{
		GameOver();
	}

	private void OnMonsterDeath()
	{
		Win();
	}

	private void GameOver()
	{
		isGameOver = true;
        continuePanel.gameObject.SetActive(true);
    }

	private void Win()
	{
		isGameOver = true;
		titlePanel.gameObject.SetActive(true);
	}
}
