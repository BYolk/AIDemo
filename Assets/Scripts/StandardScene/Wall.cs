using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ǽ���ӵ�������ײ�������ӵ�
/// </summary>
public class Wall : MonoBehaviour
{
    #region

    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 15 || collision.gameObject.layer == 14)//14��15�㼶�ֱ��Ӧ����ӵ��͵����ӵ�
        {
            Destroy(collision.gameObject);
        }
    }
}
