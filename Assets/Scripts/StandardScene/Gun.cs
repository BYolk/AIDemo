using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    #region

    #endregion


    #region ����
    public string weaponName;
    public int level = 0;                   //�����ȼ���0Ϊ��ͣ�5Ϊ���
    public float cooling = 0.02f;           //������ȴʱ�䣬ֵԽС������ٶ�Խ��
    public int damage = 5;                  //�����˺�
    public int consumeMagic = 0;
    public Vector3 weaponPos;               //����λ��
    #endregion






    #region ����
    public static Gun instance;
    Button pickButton;
    BoxCollider2D weaponCollider;
    #endregion







    #region ��ʼ��
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        weaponCollider = this.gameObject.GetComponent<BoxCollider2D>();
        if (weaponCollider == null)
        {
            weaponCollider = this.gameObject.AddComponent<BoxCollider2D>();
            weaponCollider.isTrigger = true;
        }
    }
    #endregion





    #region �����������˳���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������û�и����壬˵������װ�������ϵ�����,��Ϊ�������������������Ҳ������ײ����Ҳ��͵��ϵ�����������������������Ҫ�ж�ֻ��Player���ϵĴ��������Բ��봥��
        if (this.transform.parent == null && collision.gameObject.tag == "Player") 
        {

            collision.gameObject.GetComponent<Player>().pickableItems.Add(this.gameObject);
        }
        if (this.transform.parent == null && collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().pickableItems.Add(this.gameObject);
            }
            else if (collision.gameObject.GetComponent<FsmEnemy>())
            {
                collision.gameObject.GetComponent<FsmEnemy>().pickableItems.Add(this.gameObject);
            }
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.transform.parent == null && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().pickableItems.Remove(this.gameObject);
        }
        if (this.transform.parent == null && collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().pickableItems.Remove(this.gameObject);
            }
            else if (collision.gameObject.GetComponent<FsmEnemy>())
            {
                collision.gameObject.GetComponent<FsmEnemy>().pickableItems.Remove(this.gameObject);
            }
        }
    }
    #endregion
}
