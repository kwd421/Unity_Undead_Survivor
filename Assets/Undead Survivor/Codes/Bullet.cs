using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int pen;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int pen, Vector3 dir)
    {
        this.damage = damage;
        this.pen = pen;

        if(pen > -1)
        {
            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || pen == -1)
            return;

        pen--;

        if(pen == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);    // 오브젝트 풀링으로 관리하기 때문에 Destroy X
        }
    }
}
