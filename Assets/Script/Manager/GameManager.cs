using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public LivingEntity playerEntity;
	public LivingEntity monsterEntity;

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
		Debug.Log("Game Over!");
	}

	private void Win()
	{
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
