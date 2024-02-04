using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public LivingEntity playerEntity;
	public LivingEntity monsterEntity;

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

	private void Start()
	{
		if (playerEntity != null)
		{
			playerEntity.OnDeath += OnPlayerDeath;
		}
		if (monsterEntity != null)
		{
			monsterEntity.OnDeath += OnMonsterDeath;
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
		Debug.Log("Game Over!");
	}

	private void Win()
	{
		isGameOver = true;
		Debug.Log("You Win!");
	}

	private void OnDestroy()
	{
		if (playerEntity != null)
		{
			playerEntity.OnDeath -= OnPlayerDeath;
			playerEntity = null;
		}

		if (monsterEntity != null)
		{
			monsterEntity.OnDeath -= OnMonsterDeath;
			monsterEntity = null;
		}
	}
}
