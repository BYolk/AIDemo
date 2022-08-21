using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeState : EnemyBaseState
{
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion
    #region ����
    float nearestDistance = 9999;//�������������ź͵���֮��ľ���
    float moveSpeed;
    Vector3 escapeDir;
    #endregion
    #region ����
    GameObject[] doors;
    Transform nearestDoor;
    Transform enemyTransform;
    Rigidbody2D enemyRig;
    #endregion
    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fsmEnemy">��������״̬ʱ����Ҫ����һ�����˵Ľű�����FsmEnemy</param>
    public EscapeState(Fsm fsm)
    {
        Debug.Log("��������״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();

        
        fsmEnemy.gameObject.AddComponent<ANNController>();
        moveSpeed = fsmEnemy.moveSpeed;
        enemyRig = fsmEnemy.rig;
        enemyTransform = fsmEnemy.transform;
        doors = GameObject.FindGameObjectsWithTag("Door");
    }
    #endregion
    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        Escape();
    }
    public void Update()
    {

    }
    #endregion
    private void Escape()
    {
        foreach(GameObject door in doors)
        {
            float tempDistance = Mathf.Abs(Vector3.Distance(door.transform.position, enemyTransform.position));
            if(tempDistance < nearestDistance)
            {
                nearestDoor = door.transform;
            }
        }
        escapeDir = (nearestDoor.position - enemyTransform.position).normalized;
        enemyRig.velocity = escapeDir * Time.deltaTime * moveSpeed * 100 * 5;
    }
}
