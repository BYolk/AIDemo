using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : EnemyBaseState
{
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion

    #region �������ܹ�������Ҫ�õ��ı���������
    Rigidbody2D rig;//���˵ĸ���
    GameObject bush;//�ݴ�
    Vector3 dir;//���˵��ݴӷ���
    float moveSpeed;
   
    #endregion

    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fsmEnemy">���칥��״̬ʱ����Ҫ����һ�����˵Ľű�����FsmEnemy</param>
    public RunAwayState(Fsm fsm)
    {
        Debug.Log("��������״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();

        moveSpeed = fsmEnemy.moveSpeed;
        moveSpeed += 5;//����״̬�ٶȼ�5
        this.rig = fsmEnemy.rig;
        this.bush = ItemManager.instance.bush;
        
    }
    #endregion

    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        RunAway();
    }
    public void Update()
    {

    }
    #endregion

    private void RunAway()
    {
        dir = bush.transform.position - fsmEnemy.transform.position;
        rig.velocity = dir * Time.deltaTime * moveSpeed * 100;//���ݴԷ�����
    }
}
