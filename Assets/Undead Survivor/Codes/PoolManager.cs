using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �ܺο��� �޾ƿ� ������(������Ʈ) ����
    public GameObject[] prefabs;

    // Ǯ ��� Lists
    List<GameObject>[] pools;

    private void Awake()
    {
        // ������ ����ŭ ���� ����Ʈ ����
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }    
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ���õ� Ǯ�� ���ӿ�����Ʈ�� ����
        foreach(GameObject item in pools[index])
        {
            // ��Ȱ��ȭ �� ������Ʈ �߰� �� select�� �Ҵ�, Ȱ��ȭ
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ��Ȱ��ȭ �� ������Ʈ�� ������ ���� ����, select ������ �Ҵ�
        if(select == null)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
