using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����û�����������ǹ������ͨ��������������ȼ�
/// </summary>
public class Enemy : MonoBehaviour
{
    public static Enemy instance;






    #region ����������ر���������
    //������Ѫ����ħ��ֵ���ƶ��ٶȣ�����ȼ�,������������
    public int hpValue = 100;
    public int magicValue = 100;
    public int currentHPValue = 100;
    public int currentMagicValue = 100;
    public int damage = 5;
    public int consumeMagic = 0;
    public float moveSpeed = 5;
    public float shootSpeed = 1f;
    public int level = 0;
    


    //���ã��������;�������;����ģ�Ͷ���
    GameObject enemy;
    Rigidbody2D rig;
    GameObject model;
    public EnemyState enemyState;//����״̬״̬
    public Transform target;//���˹���Ŀ�꣬�����
    #endregion





    #region ������ر�������
    //����������ٶȣ������ʱ������ǰ��ҿ�ʰȡ����������ǰ����ֳ������Ƿ�Ϊ���������ӵ����㼶�����߼��㼶;����λ��
    public List<GameObject> pickableItems = new List<GameObject>();
    float shootTimer = 0.02f;//�����ʱ������shootTimer����shootSpeedʱ���������һ��     


    //���ã�������������������ʼ����Ԥ���壻��ǰ�ֳ�װ�����ӵ����λ��;�ӵ�Ԥ����
    Transform shootPos;
    int lowestLevelDamage = 5;
    #endregion



    #region �����ƶ���ת��ر���������
    //����������ת���ʱ��������������������������ҷ������
    private float timeValChangeDirection = 0;
    private float horizontal;
    private float vertical;
    #endregion

    /// <summary>
    /// ����״̬
    /// </summary>
    public enum EnemyState
    {
        PATROL,
        ATTACK,
        DEAD,
    }

    #region ��ʼ��
    private void Start()
    {
        instance = this;
        rig = this.GetComponent<Rigidbody2D>();
        enemyState = EnemyState.PATROL;
        target = GameObject.Find("Player").transform;
        shootPos = this.transform.Find("Model").transform.Find("ShootPos").transform;
    }
    #endregion


    #region ����
    private void Update()
    {
        FindTarget();
        switch (enemyState)//��BOSS��ͬ״̬���ò�ͬ״̬����
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.ATTACK:
                if (shootTimer >= shootSpeed)
                {
                    Attack();
                }
                else
                {
                    shootTimer += Time.deltaTime;
                }
                break;
            case EnemyState.DEAD:
                Dead();
                break;
            default:
                break;
        }
    }
    #endregion







    #region Ѳ�߷���
    /// <summary>
    /// ����Ĭ��ΪѲ��״̬������Ԥ��ȷ���õĵ���·��Ѳ�ߣ�����ҽ�������Ӷȷ�Χ���л�Ѳ��״̬Ϊ����״̬
    /// </summary>
    private void Patrol()
    {
        Move();
    }

    /// <summary>
    /// ����Ѳ��״̬���ƶ�����
    /// </summary>
    private void Move()
    {
        if (timeValChangeDirection >= 4)
        {
            // ת��ʱ���ʱ������4��
            int num = Random.Range(0, 8);
            if (num >= 5)
            {
                //�����ߣ�����Ϊ�˷�֮3
                vertical = -1;
                horizontal = 0;
            }
            else if (num == 0)
            {
                //�����ߣ�����Ϊ�˷�֮һ
                vertical = 1;
                horizontal = 0;
            }
            else if (num > 0 && num <= 2)
            {
                //������
                horizontal = -1;
                vertical = 0;
            }
            else if (num > 2 && num <= 4)
            {
                //������
                horizontal = 1;
                vertical = 0;
            }
            //��ת��timeValChangeDirectionҪ����
            timeValChangeDirection = 0;
        }
        else
        {
            //ת��ʱ���ʱ����δ����4��
            timeValChangeDirection += Time.deltaTime;
        }

        //�������������ת
        if (horizontal < 0)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontal > 0)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        rig.velocity = new Vector2(horizontal, vertical) * Time.deltaTime * moveSpeed * 100;
    }
    
    


    /// <summary>
    /// ����Ѳ��״̬Ѱ�����Ƿ���:
    /// ��ȡ����λ�����꣬�������Ǻ͵�ǰ������������н�
    /// </summary>
    private void FindTarget()
    {  
        Vector3 pos = target.position;
        float angle = 0;
        if(this.transform.rotation.y == 180)//������ﳯ�ҿ�
        {
            
            angle = Vector3.Angle(-this.transform.right, pos - this.transform.position);//����������ǰ��������˺���������ļн�
        }
        else if(this.transform.rotation.y == 0)//������ﳯ����
        {
            angle = Vector3.Angle(-this.transform.right, pos - this.transform.position);
        }
        
        if (angle < 60 && Vector3.Distance(this.transform.position, target.position) < 10)//���Ƕ�С��60�㣬����С��10��˵��Ŀ���������Ӿ���Χ
        {
            enemyState = EnemyState.ATTACK;//���ĵ���״̬Ϊ����״̬
        }
        else
        {
            Debug.Log("Ѳ��״̬");
            enemyState = EnemyState.PATROL;
        }
        
    }
    #endregion








    /// <summary>
    /// ֻ�����������������������˲Ż�ӹ���״̬�л���Ѳ��״̬��������״̬
    /// ������˹�������ֵС�ڵ�ǰħ��ֵ������й�������֮�����˹���ģʽ�л�����ȼ�Ϊ0��������ͬ��
    /// </summary>
    #region ��������
    private void Attack()
    {
        shootTimer = 0;
        Vector3 shootDir = (target.position - this.transform.position).normalized;
        if (consumeMagic <= currentMagicValue)//������˺���ֵ���ã�ʹ�õ�ǰ����
        {
            GetBullet(shootDir);
            Bullet.instance.damage = damage;  //��ȡ�����˺�ֵ��ֵ���ӵ�
            currentMagicValue -= consumeMagic;
        }
        else//������˺���ֵ������ʹ�ó�������
        {
            GetBullet(shootDir);
            Bullet.instance.damage = lowestLevelDamage;  //��ȡ�����˺�ֵ��ֵ���ӵ�
        }

    }

    /// <summary>
    /// ��ȡ�ӵ�����
    /// </summary>
    /// <param name="shootDir">�ӵ�Ҫ����ķ���</param>
    private void GetBullet(Vector3 shootDir)
    {
        GameObject bullet = EnemyBulletPoolManager.instance.getBullet();
        bullet.transform.position = shootPos.position;
        if (shootDir.y > 0)
        {
            bullet.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(shootDir, new Vector2(1, 0)));
        }
        else
        {
            bullet.transform.eulerAngles = new Vector3(0, 0, 360 - Vector3.Angle(shootDir, new Vector2(1, 0)));
        }
        bullet.GetComponent<Rigidbody2D>().AddForce(shootDir * Bullet.instance.speed);
    }
    #endregion




    #region ��������
    /// <summary>
    /// ������Ѫ�����ڻ����0���л�����״̬Ϊ����״̬
    /// </summary>
    private void Dead()
    {
        Destroy(this.gameObject);
    }
    #endregion
    #region ��Ѫ������ײǽ�ı��ƶ����򷽷�
    /// <summary>
    /// ������˺�����ӵ�������ײ�����ٵ���Ѫ�����������Ѫ������0�������״̬����Ϊ����
    /// ������˺�ǽ������ײ����ȡ���˵�ǰ�ƶ����򲢸ı���˷���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            currentHPValue -= collision.gameObject.GetComponent<Bullet>().damage;
            if(currentHPValue <= 0)
            {
                currentHPValue = 0;
                enemyState = EnemyState.DEAD;
            }
        }
        else if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Boxes" || collision.gameObject.tag == "Obstacle")
        {
            EnemyHitWallChangeDir();
        }
    }

    /// <summary>
    /// �������ײǽ���ı���˷���
    /// </summary>
    private void EnemyHitWallChangeDir()
    {
        int num = Random.Range(0, 3);
        if (horizontal > 0)//������������ҹ���ײǽ�����ת��
        {
            if (num == 0)
            {
                horizontal = -1;
            }
            else if (num == 1)
            {
                horizontal = 0;
                vertical = 1;
            }
            else
            {
                horizontal = 0;
                vertical = -1;
            }

        }
        else if (horizontal < 0)//����������������ײǽ�����ת��
        {
            if (num == 0)
            {
                horizontal = 1;
            }
            else if (num == 1)
            {
                horizontal = 0;
                vertical = 1;
            }
            else
            {
                horizontal = 0;
                vertical = -1;
            }
        }
        else if (vertical > 0)//������������Ϲ���ײǽ�����ת��
        {
            if (num == 0)
            {
                vertical = -1;
            }
            else if (num == 1)
            {
                horizontal = 1;
                vertical = 0;
            }
            else
            {
                horizontal = -1;
                vertical = 0;
            }
        }
        else if (vertical < 0)//������������¹���ײǽ�����ת��
        {
            if (num == 0)
            {
                vertical = 1;
            }
            else if (num == 1)
            {
                horizontal = 1;
                vertical = 0;
            }
            else
            {
                horizontal = -1;
                vertical = 0;
            }
        }

        //���ݵ��˷�����ת����
        if (horizontal < 0)
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontal > 0)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        rig.velocity = new Vector2(horizontal, vertical) * Time.deltaTime * moveSpeed * 100;
        timeValChangeDirection = 0;
    }
    #endregion
}
