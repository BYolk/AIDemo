using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ѳ��״̬����Ѳ��״̬��Ѱ����ң����ʶ����ң��ж������в�ȼ���
///     1������������������ȼ��߳�3�����������״̬����֮���빥��״̬
///     2�������Ҵ���Rage״̬(Player.cs�ű���Rage����ΪTrue�������������״̬����֮���빥��״̬
/// </summary>
public class PatrolState : EnemyBaseState
{
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion

    #region ������ת�ƶ���ر�������
    //����
    static float timeValChangeDirection;//ת���ʱ��
    static float vertical;//�����жϵ������·�����ת
    static float horizontal;//�����жϵ������ҷ�����ת
    static float moveSpeed;//�����ƶ��ٶ�

    //����
    static Rigidbody2D rig;
    Transform target;
    static Transform enemyTransform;
    #endregion

    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fsmEnemy">����Ѳ��״̬ʱ����Ҫ����һ�����˵Ľű�����FsmEnemy</param>
    public PatrolState(Fsm fsm)
    {
        Debug.Log("����Ѳ��״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();
        moveSpeed = fsmEnemy.moveSpeed;
        rig = fsmEnemy.rig;
        target = fsmEnemy.target;
        enemyTransform = fsmEnemy.transform;
    }
    #endregion

    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        //FindTarget();//�������---�÷����ڸ�֪ϵͳ��ʵ��
        Patrol();//Ѳ��
    }
    public void Update()
    {
        timeValChangeDirection += Time.deltaTime;
    }
    #endregion

    #region Ѱ����ҷ���
    /// <summary>
    /// ����Ѳ��״̬Ѱ�����Ƿ���:��Ѳ��״̬���ҵ����ǽ��빥��״̬
    /// </summary>
    private void FindTarget()
    {
        Vector3 pos = target.position;
        float angle = 0;
        if (enemyTransform.rotation.y == 180)//������ﳯ�ҿ�
        {
            angle = Vector3.Angle(-enemyTransform.right, pos - enemyTransform.position);//����������ǰ��������˺���������ļн�
        }
        else if (enemyTransform.rotation.y == 0)//������ﳯ����
        {
            angle = Vector3.Angle(-enemyTransform.right, pos - enemyTransform.position);
        }

        if (angle < 120 && Vector3.Distance(enemyTransform.position, target.position) < 20)//���Ƕ�С��120�㣬����С��20��˵��Ŀ���������Ӿ���Χ
        {
            //��ȡ��ҵ�ǰ�ֳ������ȼ����������ȼ��������������3������˽�������״̬
            Player playerScript = GameObject.Find("Player").GetComponent<Player>();//��ȡ���Player�ű����
            int gunLevel = playerScript.currentHandle.gameObject.GetComponent<Gun>().level;
            int enemyLevel = enemyTransform.gameObject.GetComponent<FsmEnemy>().level;

            //�����Ҵ��ڿ�״̬������˽�������״̬
            bool playerIsRage = playerScript.isRage;
            if (gunLevel - enemyLevel >= 3 || playerIsRage == true)
            {
                fsm.SetEnemyState(new EscapeState(fsm));//�ı�״̬Ϊ����״̬
            }
            else
            {
                fsm.SetEnemyState(new AttackState(fsm));//�ı�״̬Ϊ����״̬
            }
        }
    }
    #endregion

    #region Ѳ�߷���
    /// <summary>
    /// ����Ѳ�߷���
    /// </summary>
    private void Patrol()
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
            enemyTransform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontal > 0)
        {
            enemyTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        rig.velocity = new Vector2(horizontal, vertical) * Time.deltaTime * moveSpeed * 100;
    }
    #endregion

    #region ײǽ�ı��˶���������
    /// <summary>
    /// �������ײǽ���ı���˷���
    /// </summary>
    public static void EnemyHitWallChangeDir()
    {
        int num = Random.Range(0, 3);
        if (horizontal > 0)//������������ҹ���ײǽ0�����ת��
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
            enemyTransform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontal > 0)
        {
            enemyTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        rig.velocity = new Vector2(horizontal, vertical) * Time.deltaTime * moveSpeed * 100;
        timeValChangeDirection = 0;
    }
    #endregion
}
