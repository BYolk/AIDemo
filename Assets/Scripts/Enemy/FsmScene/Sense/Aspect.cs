using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��֪ϵͳ��Aspect��
/// </summary>
public class Aspect : MonoBehaviour
{
    /// <summary>
    /// ��֪ϵͳ��Ҫ��֪����Ϸ����
    /// </summary>
    public enum aspect
    {
        PLAYER,
        GUN,
        POTION,
        BOX,
        WALL,
        OBSTACLE,
        BUSH
    }

    public aspect aspectName;
}
