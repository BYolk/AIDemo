using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyBaseState
{
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion
    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fsmEnemy">��������״̬ʱ����Ҫ����һ�����˵Ľű�����FsmEnemy</param>
    public DeadState(Fsm fsm)
    {
        Debug.Log("��������״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();
    }
    #endregion
    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        Dead();
    }
    public void Update()
    {
    }
    #endregion
    #region ��������
    private void Dead()
    {
        fsmEnemy.Dead();
    }
    #endregion
}
