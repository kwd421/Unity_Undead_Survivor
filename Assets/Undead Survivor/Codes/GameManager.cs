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

        player.gameObject.SetActive(true);  // �÷��̾� Ȱ��ȭ
        uiLevelUp.Select(playerId % 2); // �⺻���� Ȱ��ȭ
        Resume();   // ���� ���� or ���� �¸��� Stop()�� ����Ǳ� ������ Resume()���� timeScale ����
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        Debug.Log("GameOver");
        isLive = false; // �÷��̾� ����, ���� ������ �� ����

        yield return new WaitForSeconds(0.5f);  // �÷��̾� ��� �ִϸ��̼� ����� 0.5��

        uiResult.gameObject.SetActive(true);   // ���� �� UI
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
        isLive = false; // �÷��̾� ����, ���� ������ �� ����
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);  // �� ��� �ִϸ��̼� ����� 0.5��

        uiResult.gameObject.SetActive(true);   // �̰��� �� UI
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
            GameVictory();  // �ð� �� ��Ƽ�� �¸�
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
        Time.timeScale = 0; // �ð�����
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // 2�� 2���, 3�� 3���...
    }
}