using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ӵ����ϰ��﷢����ײ�������ӵ�
/// </summary>
public class Obstacle : MonoBehaviour
{
    #region

    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet" || collision.gameObject.tag == "EnemyBullet")//14��15�㼶�ֱ��Ӧ����ӵ��͵����ӵ�
        {
            Destroy(collision.gameObject);
        }
    }
}
