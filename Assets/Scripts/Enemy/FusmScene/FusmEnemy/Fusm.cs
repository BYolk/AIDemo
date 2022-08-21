using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ģ��״̬��
/// </summary>
public class Fusm 
{
    FsmEnemy fsmEnemy;//���˶���
    public List<FusmBaseState> states;//ģ��״̬��������״̬����
    public List<FusmBaseState> activated;//ģ��״̬�����Ѿ������״̬����
    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    public Fusm(FsmEnemy fsmEnemy)
    {
        this.fsmEnemy = fsmEnemy;
        states = new List<FusmBaseState>();//����ģ��״̬������״̬���ϱ���
        activated = new List<FusmBaseState>();//����ģ��״̬���Ѽ���״̬���ϱ���
    }
    #endregion
    /// <summary>
    /// ����ģ��״̬������ȡ�����Ѽ���״̬���÷�������FsmEnemy NPC��Ϸ�����Update������ÿ֡����
    /// ����Ѿ������״̬��������δ����״̬���ϡ����㼤�����ӣ����������ӴﵽҪ���򽫸�״̬��ӵ�����״̬����������֮��״̬��ӵ�δ����״̬����
    /// �������δ����״̬
    /// </summary>
    public void UpdateFusm()
    {
        if (states.Count == 0)
            return;
        activated.Clear();
        List<FusmBaseState> nonActiveStates = new List<FusmBaseState>();
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].Evaluate() > 0)
                activated.Add(states[i]);
            else
                nonActiveStates.Add(states[i]);   
        }
        if (nonActiveStates.Count != 0)
        {
            for (int i = 0; i < nonActiveStates.Count; i++)
                nonActiveStates[i].Exit();
        }
    }
    public void Update()// ����״̬��Ϊ��������Update������FsmEnemy��Update�����е��ã����˴���Update�����൱��Unity��Update����
    {
        foreach(FusmBaseState activatedState in activated)//����ÿһ���Ѿ������״̬�������ø�״̬��Update����
        {
            activatedState.Update();
        }
    }



    #region ���״̬
    /// <summary>
    /// ���״̬
    /// </summary>
    /// <param name="newState"></param>
    public void AddState(FusmBaseState newState)
    {
        states.Add(newState);
    }
    #endregion

    #region �ж�״̬�Ƿ񱻼���
    /// <summary>
    /// �ж�ĳһ��״̬�Ƿ񼤻�����Ѽ����״̬���ϣ����ж��������state�Ƿ����
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool IsActive(FusmBaseState state)
    {
        if (activated.Count != 0)
        {
            for (int i = 0; i < activated.Count; i++)
            {
                if (activated[i] == state)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region ����ģ��״̬��
    /// <summary>
    /// ����ģ��״̬������������״̬�����������ʼ��
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < states.Count; i++)
        {
            states[i].Exit();
            states[i].Init();
        }
    }
    #endregion

    #region ��ȡNPC��Ϸ����
    public FsmEnemy GetFusmEnemy()
    {
        return fsmEnemy;
    }
    #endregion

    #region ��ȡ���˻���״̬�����״̬����
    /// <summary>
    /// ��ȡ����״̬����
    /// </summary>
    /// <returns></returns>
    public List<FusmBaseState> GetEnemyStates()
    {
        return states;
    }


    #endregion

}
