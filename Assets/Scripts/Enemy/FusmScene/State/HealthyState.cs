using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����״̬��:ֻ����ͬʱ���ڽ���״̬������״̬
/// </summary>
public class HealthyState : FusmBaseState
{
    #region ģ��״̬�������뿪��ģ������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private Fusm fusm;//��ȡ����ģ��״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion
    #region ����״̬��ر���
    private float activation;//�����ڱ�״̬�ĳ̶ȣ�0.0f��ʾ��ȫ�����ڴ�״̬��1.0f��ʾ��ȫ���ڴ�״̬
    int hpValue;//Ѫ������
    int currentHPValue;//��ǰѪ��
    float timer;//��ʱ��
    #endregion
    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fusm"></param>
    public HealthyState(Fusm fusm)
    {
        //Debug.Log("ע�ὡ��״̬");
        this.fusm = fusm;
        this.fsmEnemy = fusm.GetFusmEnemy();
    }
    #endregion
    #region ���㽡��״̬��������
    /// <summary> 
    /// �� Fusm ģ��״̬���� UpdateFusm ����ģ��״̬�������е��� Evaluate ���жϸ�״̬�� activation �Ƿ����0
    /// ���Ѫ���ٷֱ�Ϊ�ٷ�֮��ʮ��˵������ȫȫ��������״̬
    /// ������ڰٷ�֮��ʮС�ڰٷ�֮��ʮ��˵��ͬʱ��������״̬�봹��״̬
    /// ���ڰٷ�֮��ʮС�ڰٷ�֮��ʮ��˵��ͬʱ��������״̬�뽡��״̬
    /// </summary>
    public float Evaluate()
    {
        hpValue = fsmEnemy.hpValue;
        currentHPValue = fsmEnemy.currentHPValue;
        float hpPercentage = (float)Math.Round((decimal)currentHPValue / hpValue, 2);//�õ���ǰѪ���ٷֱ�
        activation = 0.025f * (hpPercentage * 100) - 1.5f;
        CheckBounds();//��� activation �ĺϷ���
        if (activation > 0)
        {
            Debug.Log("��ǰ���ڽ���״̬�������ڽ���״̬�ĳ̶�Ϊ��" + activation.ToString());
        }
        if (activation > 0.25)
        {
            Debug.Log("��ǰѪ�������ڽ���״̬�ĳ̶ȸ��ߣ��������κβ���");
        }
        else if (activation > 0 && activation <= 0.25)
        {
            Debug.Log("��ǰѪ������������״̬�ĳ̶ȸ��ߣ����������������ҩƿ������ʰȡ״̬");
            /*GameObject[] potions = ItemManager.instance.potions;
            foreach(GameObject potion in potions)//��ȡ����������ҩƿ������ҩƿ�����ҩƿ�ָܻ�HP�������ʰȡ״̬
            {
                if (potion && (potion.GetType().Name == "HealthyPotion" || potion.GetType().Name == "RejuvenationPotion"))
                {
                    fsm.SetEnemyState(new PickState(fsm, potion));
                    fsmEnemy.GetComponent<EnemyPerspective>().isInRunAwayState = true;
                    break;
                }
            }*/
        }
        return activation;//���� activation
    }
    #endregion




    public void Update()
    {


    }

    #region ��⼤������ֵ�ĺϷ���
    /// <summary>
    /// ��⼤������ֵ�ĺϷ���
    /// </summary>
    /// <param name="lowerBound"></param>
    /// <param name="upperBound"></param>
    public void CheckBounds(float lowerBound = 0, float upperBound = 1)
    {
        CheckLowerBound(lowerBound);
        CheckUpperBound(upperBound);
    }
    /// <summary>
    /// �����������޺Ϸ���
    /// </summary>
    /// <param name="lowerBound"></param>
    public void CheckLowerBound(float lowerBound = 0)
    {
        if (activation < lowerBound)
        {
            activation = lowerBound;
        }
    }
    /// <summary>
    /// �����������޺Ϸ���
    /// </summary>
    /// <param name="upperBound"></param>
    public void CheckUpperBound(float upperBound = 1)
    {
        if (activation > upperBound)
        {
            activation = upperBound;
        }

    }
    #endregion

























    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Init()
    {
        
    }
}
