using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kills;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# GameObject")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);  // 플레이어 활성화
        uiLevelUp.Select(playerId % 2); // 기본무기 활성화
        Resume();   // 게임 오버 or 게임 승리시 Stop()이 실행되기 때문에 Resume()으로 timeScale 복구
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        Debug.Log("GameOver");
        isLive = false; // 플레이어 조작, 몬스터 움직임 등 정지

        yield return new WaitForSeconds(0.5f);  // 플레이어 사망 애니메이션 재생용 0.5초

        uiResult.gameObject.SetActive(true);   // 졌을 때 UI
        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        Debug.Log("Victory!");
        isLive = false; // 플레이어 조작, 몬스터 움직임 등 정지
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);  // 적 사망 애니메이션 재생용 0.5초

        uiResult.gameObject.SetActive(true);   // 이겼을 때 UI
        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();  // 시간 다 버티면 승리
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;
        if(exp > nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // 시간정지
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // 2는 2배속, 3은 3배속...
    }
}