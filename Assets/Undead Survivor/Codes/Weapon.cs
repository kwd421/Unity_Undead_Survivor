using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            // 근접
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            // 원거리
            default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)    // 근접무기 경우
            Place();

        // Weapon 레벨업하면 데미지, 카운트가 매개값으로 설정되기 때문에 설정된값에 업그레이드 적용
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;
        // 무기를 플레이어의 자식으로 넘기고 플레이어 기준으로 무기 위치 설정
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            // 근접
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Place();
                break;

            // 원거리
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Hands Active
        Hand hand = player.hands[(int)data.itemType];   // enum은 index가 자동으로 들어가는 것 이용
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // 새로 생긴 무기에도 업그레이드 적용(Init시 player의 Children들에게 ApplyGear 적용
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 근접무기 배치
    public void Place()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;

            // 기존에 있던 것 재활용 -> 원래 1개(childCount) 있었으면 1개는 GetChild, 4개는 Get
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index); // 재활용이므로 부모는 이미 Weapon 0
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  // bullet의 부모를 PoolManager에서 Weapon 0으로 변경
            }

            //bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1은 무한관통
            
        }
    }

    void Fire()
    {
        // 타겟이 없으면 Fire하지 않음
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
