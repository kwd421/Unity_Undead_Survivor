using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achieve { UnlockPotato, UnlockBean }
    Achieve[] achieves;
    WaitForSecondsRealtime wait;    // Realtime: isLive �����־(deltaTime���帣�� �ص�) �����ð����� ����

    private void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // ������ ����
        PlayerPrefs.SetInt("MyData", 1);
        // Init�� ��� ���� �ʱ�ȭ
        foreach(Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach(Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch(achieve)
        {
            // ���ڳ�� �ر�����
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kills >= 10;
                break;

            // ���� �ر�����
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // ���� ���� �޼� �� PlayerPrefs�� Ű���� �����Ͱ� 0�϶�
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for(int i=0; i<uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
