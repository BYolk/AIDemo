using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���٣����գ�����ű�
/// </summary>
public class DestroyBullet : MonoBehaviour
{

    #region ����
    float activeTime = 3;//�ӵ����ʱ��
    #endregion

    #region ����
    GameObject bullet;//Ҫ���յ���Ϸ����
    Rigidbody2D rig;//�ӵ�����ĸ���:���������ӵ��ٶ�
    #endregion

    #region ��ʼ��
    private void Start()
    {
        bullet = this.gameObject;
        rig = this.GetComponent<Rigidbody2D>();
    }
    #endregion

    #region ����
    private void Update()
    {
        activeTime -= Time.deltaTime;
        if(activeTime < 0)
        {
            if (this.gameObject.CompareTag("PlayerBullet"))
            {
                PlayerBulletPoolManager.instance.recoverBullet(bullet);
            }
            if (this.gameObject.CompareTag("EnemyBullet"))
            {
                EnemyBulletPoolManager.instance.recoverBullet(bullet);
            }
        }
    }
    #endregion

    /// <summary>
    /// �����ӵ�����󣬸��ӵ�����ͱ����ã��ٶ�Ϊ0��
    /// </summary>
    private void OnDisable()
    {
        rig.velocity = Vector3.zero;
    }

    /// <summary>
    /// �����ӵ����󣺴��ӵ��������ȡ���ӵ����󣬼�����ӵ�����,Ϊ�ӵ�����ֵ����Ӹ���
    /// </summary>
    private void OnEnable()
    {
        if(rig == null)
        {
            bullet = this.gameObject;
            rig = GetComponent<Rigidbody2D>();
        }
        activeTime = 3;
    }
}
