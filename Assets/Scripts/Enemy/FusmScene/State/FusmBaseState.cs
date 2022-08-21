using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ģ��״̬������״̬��
/// </summary>
public interface FusmBaseState
{
    float Evaluate();//�����Ƿ����Լ���ģ��״̬
    void Update();//����
    //���߽磬ȷ��Evaluate���صļ������ӵ�ֵ�ĺϷ���
    void CheckLowerBound(float lowerBound = 0.0f);//�����ͱ߽�
    void CheckUpperBound(float upperBound = 1.0f);//�����߽߱�
    void CheckBounds(float lowerBound = 0.0f, float upperBound = 1.0f);//���߽�








    void Exit();
    void Init();//��ʼ��



    /*public float activation;//�ж��л�ģ��״̬���ı���
    protected FusmEnemy fusmEnemy;//���˶���

    public virtual void Enter() { }//����״ִ̬�з���
    public virtual void Exit() { }//�˳�״ִ̬�з���
    public abstract void UpdateFusmBaseState();//����
    public virtual void Init() { activation = 0.0f; }//��ʼ��
    public abstract float CalculateActivation();//�����������أ��Ƿ������л�ģ��״̬��

    public virtual void CheckLowerBound(float lbound = 0.0f) { if (activation < lbound) activation = lbound; }//�����ͱ߽�
    public virtual void CheckUpperBound(float ubound = 1.0f) { if (activation > ubound) activation = ubound; }//�����߽߱�
    public virtual void CheckBounds(float lb = 0.0f, float ub = 1.0f) { CheckLowerBound(lb); CheckUpperBound(ub); }//���߽�*/

}
