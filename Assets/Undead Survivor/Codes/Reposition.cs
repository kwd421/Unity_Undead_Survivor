using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))  // 플레이어 주위 Area
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; // 타일맵의 위치 (transform은 현재 오브젝트의 것)
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;   // 정규화때문에 대각선은 x, y값이 1보다 작음
        float dirX = playerDir.x < 0 ? -1 : 1;  // 이것으로 타일맵 좌우 이동 판단
        float dirY = playerDir.y < 0 ? -1 : 1;  // 타일맵 상하 판단

        switch (transform.tag)  // 오브젝트의 태그값
        {
            case "Ground":
                if(diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);  // 값만큼 위치이동
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));

                }
                break;


        }
    }
}
