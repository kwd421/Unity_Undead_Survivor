using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 외부에서 받아온 프리팹(오브젝트) 보관
    public GameObject[] prefabs;

    // 풀 담당 Lists
    List<GameObject>[] pools;

    private void Awake()
    {
        // 프리팹 수만큼 관리 리스트 생성
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }    
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택된 풀의 게임오브젝트에 접근
        foreach(GameObject item in pools[index])
        {
            // 비활성화 된 오브젝트 발견 시 select에 할당, 활성화
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 비활성화 된 오브젝트가 없으면 새로 생성, select 변수에 할당
        if(select == null)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
