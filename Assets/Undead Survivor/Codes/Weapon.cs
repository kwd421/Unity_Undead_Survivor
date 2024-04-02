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
            // ����
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            // ���Ÿ�
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

        if (id == 0)    // �������� ���
            Place();

        // Weapon �������ϸ� ������, ī��Ʈ�� �Ű������� �����Ǳ� ������ �����Ȱ��� ���׷��̵� ����
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;
        // ���⸦ �÷��̾��� �ڽ����� �ѱ�� �÷��̾� �������� ���� ��ġ ����
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
            // ����
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Place();
                break;

            // ���Ÿ�
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Hands Active
        Hand hand = player.hands[(int)data.itemType];   // enum�� index�� �ڵ����� ���� �� �̿�
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // ���� ���� ���⿡�� ���׷��̵� ����(Init�� player�� Children�鿡�� ApplyGear ����
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // �������� ��ġ
    public void Place()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;

            // ������ �ִ� �� ��Ȱ�� -> ���� 1��(childCount) �־����� 1���� GetChild, 4���� Get
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index); // ��Ȱ���̹Ƿ� �θ�� �̹� Weapon 0
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  // bullet�� �θ� PoolManager���� Weapon 0���� ����
            }

            //bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1�� ���Ѱ���
            
        }
    }

    void Fire()
    {
        // Ÿ���� ������ Fire���� ����
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
