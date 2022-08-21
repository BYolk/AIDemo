using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ķ�����״̬ģʽ�е�״̬������
///     1������State�ӿ����ԣ���ʾ��ǰ�����ģ��������еĵ�ǰ״̬
///     2�����幹�췽��
/// </summary>
public class Context
{
    private EnemyBaseState enemyBaseState;

    //Context���췽��
    public Context(EnemyBaseState enemyBaseState)
    {
        this.EnemyState = enemyBaseState;
    }

    //State����
    public EnemyBaseState EnemyState
    {
        get { return enemyBaseState; }
        set
        {
            enemyBaseState = value;
            Debug.Log("State: " + enemyBaseState.GetType().Name);
        }
    }

    /// <summary>
    /// ���õ�ǰ״̬��Ӧ�ķ���
    /// </summary>
    public void Request()
    {
        enemyBaseState.Handle();
    }
}
