using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ʒ�������ű����ڴ˽ű��д�ų�����������Ʒ
/// </summary>
public class ItemManager : MonoBehaviour
{
    #region ����
    public static ItemManager instance;
    #endregion

    #region ��������Ʒ��ر���
    public GameObject player;
    public GameObject[] guns;
    public GameObject[] potions;
    public GameObject[] boxes;
    public GameObject[] walls;
    public GameObject[] obstacles;
    public GameObject bush;
    #endregion
    #region ��ⳡ������Ʒ��ز���
    public float updateRate;//��������
    private float elapsedTime;//������ʱ��
    #endregion
    #region ��ʼ��
    private void Awake()
    {
        instance = this;
        updateRate = 1.0f;//��ʼ����������Ϊ1s��ÿ��1s������Ʒ����
        GetItemsInScene();
    }
    #endregion
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= updateRate)
        {
            GetItemsInScene();
        }
    }
    private void GetItemsInScene()
    {
        elapsedTime = 0;
        FindGameObjects();
    }

    private void FindGameObjects()
    {
        player = GameObject.Find("Player");
        guns = GameObject.FindGameObjectsWithTag("EnvironmentWeapon");
        potions = GameObject.FindGameObjectsWithTag("Potion");
        boxes = GameObject.FindGameObjectsWithTag("Boxes");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        bush = GameObject.FindGameObjectWithTag("Bush");
    }



}
