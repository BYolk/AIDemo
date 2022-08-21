using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ӧ��FSM����״̬���ĵ��˽ű�
/// 
/// 1��״̬ģʽ��
///     Ϊ����״̬����һ���ӿ� StateStructure���ýӿڲ���������Ӧ�������״̬����������չ���κ���Ϸʵ��״̬��
///     Ϊÿ��״̬����һ���ࣨΪEnemy��ÿһ��״̬������һ���ࣩ
///     �������ί��
///     
/// 2��״̬ģʽ�ṹ��
///     1��State�ӿڣ�Ϊ����״̬����Ľӿ�
///     2������״̬�ࣺΪÿ��״̬����һ����
///         ConcreteStateA������״̬A
///         ConcreteStateB������״̬B
///         ����
///     
///     3��Context�ӿڣ�
///         1�������Ľӿڣ����ڹ���״̬����״̬������
///         2����Context�ж�����ؽӿڷ���
///         3�����嵱ǰ״̬����ǰ״̬��Ϊ����״̬���һ��ʵ����
/// </summary>
public class Fsm
{
    #region ��������
    EnemyBaseState state;//���˻���״̬��������ʾ��״̬����ǰ�����״̬
    FsmEnemy fsmEnemy;//���˶��󣬱�ʾ������״̬����������һ��FsmEnemy��FsmEnemy��ʾ���˶���
    #endregion

    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="startFsm">����FsmEnemyʱ����Ҫ����һ��StartFsm�ű��������</param>
    public Fsm(FsmEnemy fsmEnemy)
    {
        this.fsmEnemy = fsmEnemy;
    }
    #endregion

    #region ���õ���״̬����
    /// <summary>
    /// ���õ���״̬����
    /// </summary>
    /// <param name="newState">��״̬</param>
    public void SetEnemyState(EnemyBaseState newState)
    {
        state = newState;
    }
    #endregion

    #region ��ȡ���˻���״̬�����״̬����
    /// <summary>
    /// ��ȡ����״̬����
    /// </summary>
    /// <returns></returns>
    public EnemyBaseState GetEnemyState()
    {
        return state;
    }

    /// <summary>
    /// ��ȡ����״̬�������������
    /// </summary>
    /// <returns></returns>
    public string GetEnemyStateName()
    {
        return state.GetType().Name;
    }
    #endregion


    #region ��ȡ��������״̬���Ľű�����
    /// <summary>
    /// ��ȡ��������״̬���Ľű�����
    /// </summary>
    /// <returns>����StartFsm����</returns>
    public FsmEnemy GetFsmEnemy()
    {
        return fsmEnemy;
    }
    #endregion

    #region Handle
    /// <summary>
    /// ��ǰ״̬��Ϊ����
    /// </summary>
    public void Handle()
    {

    }
    #endregion

    #region ����:��Update������FsmEnemy��Update�����е��ã����˴���Update�����൱��Unity��Update����
    /// <summary>
    /// ���õ�ǰ״̬����Ϊ����
    /// ��Update������FsmEnemy��Update�����е��ã����˴���Update�����൱��Unity��Update����
    /// </summary>
    public void Update()
    {
        state.Handle();
        state.Update();
    }
    #endregion
}