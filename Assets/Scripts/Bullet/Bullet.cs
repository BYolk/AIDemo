using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region

    #endregion
    public static Bullet instance;

    #region ����
    public LayerMask bulletLayer;//�ӵ��㼶
    public float speed = 2000;//�ӵ��ٶ�
    public int damage = 5;
    #endregion

    #region ����
    #endregion

    #region ��ʼ��
    private void Awake()
    {
        instance = this;
    }
    #endregion
}
