using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ӵ������
/// </summary>
public class PlayerBulletPoolManager : MonoBehaviour
{
    #region

    #endregion

    #region ����
    Queue<GameObject> playerBulletPool;                   //��������ӵ�����Ķ���
    int poolInitLength = 100;                        //�����ӵ�������е��ӵ���������
    #endregion

    #region ����
    public static PlayerBulletPoolManager instance;       //�����ű�����
    public GameObject bulletPrefab;                 //�ӵ���������
    #endregion

    #region ��ʼ��
    private void Awake()
    {
        instance = this;    //ʵ��������
    }
    private void Start()
    {
        playerBulletPool = new Queue<GameObject>();   //����һ�����У����ڴ���ӵ�����
        GameObject bullet = null;               //����һ���յ��ӵ�����
        for(int i = 0; i < poolInitLength; i++) //����ӵ������
        {
            bullet = Instantiate(bulletPrefab); //ʵ�����ӵ�����
            bullet.SetActive(false);            //�����ӵ����ӵ��ӳ����ó���ʹ��ʱ�ټ��
            playerBulletPool.Enqueue(bullet);         //���ö��е���ӷ��������ӵ���䵽�ӵ��������
            bullet.transform.parent = GameObject.Find("PlayerBulletPool").transform;
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
        if(playerBulletPool.Count > 0)                        //����ӵ�����أ��ӵ����У�ʣ���ӵ���������0������ӵ��������ȡ��һ���ӵ����󷵻�
        {
            GameObject bullet = playerBulletPool.Dequeue();   //���ó��ӷ����Ӷ�����ȡ��һ���ӵ�����
            bullet.SetActive(true);                     //�����ӵ�����
            return bullet;                              //���ӵ����󷵻�
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);//����������˵���ӵ�������Ѿ�û���ӵ������ˣ����³�ʼ��һ���ӵ����󷵻�
            bullet.transform.parent = GameObject.Find("PlayerBulletPool").transform;
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
        playerBulletPool.Enqueue(bullet);                     //���ӵ�������յ����У�����أ���               
    }
    #endregion
}
