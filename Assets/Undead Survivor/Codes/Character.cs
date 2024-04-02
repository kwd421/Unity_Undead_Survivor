using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // GameManager처럼 어디서나 불러올 수 있게 static 사용
    public static float Speed
    {
        get
        {
            return GameManager.instance.playerId == 0 ? 1.1f : 1f;
        }
    }

    public static float WeaponSpeed
    {
        get
        {
            return GameManager.instance.playerId == 1 ? 1.1f : 1f;
        }
    }

    public static float WeaponRate
    {
        get
        {
            return GameManager.instance.playerId == 1 ? 0.9f : 1f;
        }
    }

    public static float Damage
    {
        get
        {
            return GameManager.instance.playerId == 2 ? 1.2f : 1f;
        }
    }

    public static int Count
    {
        get
        {
            return GameManager.instance.playerId == 3 ? 1 : 0;
        }
    }
}
