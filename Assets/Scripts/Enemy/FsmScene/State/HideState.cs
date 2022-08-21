using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : EnemyBaseState
{
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion
    #region �������ܹ�������Ҫ�õ��ı���������
    int currentHPValue;
    int currentMagicValue;
    float timer = 0f;
    GameObject bush;
    #endregion
    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fsmEnemy">���칥��״̬ʱ����Ҫ����һ�����˵Ľű�����FsmEnemy</param>
    public HideState(Fsm fsm)
    {
        Debug.Log("��������״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();


        currentHPValue = fsmEnemy.currentHPValue;
        currentMagicValue = fsmEnemy.currentMagicValue;
        bush = ItemManager.instance.bush;
    }
    #endregion
    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        HideInBush();
    }
    public void Update()
    {

    }
    #endregion
    private void HideInBush()
    {
        fsmEnemy.transform.position = bush.transform.position;
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            if(currentHPValue < 100)
            {
                currentHPValue += 1;
            }
            if(currentMagicValue < 100)
            {
                currentMagicValue += 1;
            }
            Debug.Log("���˵�Ѫ��" + currentHPValue.ToString());
            Debug.Log("���˵�ħ��ֵ" + currentMagicValue.ToString());
            fsmEnemy.currentHPValue = currentHPValue;
            fsmEnemy.currentMagicValue = currentMagicValue;
            timer = 0f;
        }
    }
}
