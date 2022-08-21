using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPoolManager : MonoBehaviour
{
    #region

    #endregion

    #region ����
    Queue<GameObject> enemyBulletPool;                   //��������ӵ�����Ķ���
    int poolInitLength = 100;                        //�����ӵ�������е��ӵ���������
    #endregion

    #region ����
    public static EnemyBulletPoolManager instance;       //�����ű�����
    public GameObject bulletPrefab;                 //�ӵ���������
    #endregion

    #region ��ʼ��
    private void Awake()
    {
        instance = this;    //ʵ��������
    }
    private void Start()
    {
        enemyBulletPool = new Queue<GameObject>();   //����һ�����У����ڴ���ӵ�����
        GameObject bullet = null;               //����һ���յ��ӵ�����
        for (int i = 0; i < poolInitLength; i++) //����ӵ������
        {
            bullet = Instantiate(bulletPrefab); //ʵ�����ӵ�����
            bullet.SetActive(false);            //�����ӵ����ӵ��ӳ����ó���ʹ��ʱ�ټ��
            enemyBulletPool.Enqueue(bullet);         //���ö��е���ӷ��������ӵ���䵽�ӵ��������
            bullet.transform.parent = GameObject.Find("EnemyBulletPool").transform;
        }
    }
    #endregion

    #region ʹ��������ӵ�����

    /// <summary>
    /// ��ȡ�ӵ�����
    /// ����Ӷ���ػ�ȡ�ӵ����ٶȴ��ڻ��յ��ٶȣ���ô��Ҫ���³�ʼ���ӵ�����
    /// </summary>
    /// <returns>����һ���ӵ�Ԥ�������</returns>
    public GameObject getBullet()
    {
        if (enemyBulletPool.Count > 0)                        //����ӵ�����أ��ӵ����У�ʣ���ӵ���������0������ӵ��������ȡ��һ���ӵ����󷵻�
        {
            GameObject bullet = enemyBulletPool.Dequeue();   //���ó��ӷ����Ӷ�����ȡ��һ���ӵ�����
            bullet.SetActive(true);                     //�����ӵ�����
            return bullet;                              //���ӵ����󷵻�
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);//����������˵���ӵ�������Ѿ�û���ӵ������ˣ����³�ʼ��һ���ӵ����󷵻�
            bullet.transform.parent = GameObject.Find("EnemyBulletPool").transform;
            return bullet;
        }
    }

    /// <summary>
    /// �����ӵ����󷽷�
    /// </summary>
    /// <param name="bullet">��Ҫ���յ��ӵ�����</param>
    public void recoverBullet(GameObject bullet)
    {
        bullet.SetActive(false);                        //���ӵ��������
        enemyBulletPool.Enqueue(bullet);                     //���ӵ�������յ����У�����أ���               
    }
    #endregion

}
