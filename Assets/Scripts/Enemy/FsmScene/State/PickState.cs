using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʰȡ״̬
/// </summary>
public class PickState : EnemyBaseState
{
    #region ����������
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion

    #region ��ʰȡ��Ʒ����ر���������
    GameObject pickItemObject;//��Ҫȥʰȡ����Ʒ
    Vector3 dir;//ǰ��ʰȡ�ķ���
    GameObject[] fsmEnemyPrefabs;
    #endregion

    #region Npc��ر���������
    Rigidbody2D rig;//���˸������
    List<GameObject> pickableItems;//���˿�ʰȡ��Ϸ��������
    int hpValue;
    int magicValue;
    int currentHPValue;
    int currentMagicValue;
    int damage;
    float moveSpeed;//���˵��ƶ��ٶ�
    int level;//���˵ȼ�
    bool isRage;//�����Ƿ��ڿ�״̬
    float shootSpeed;//��������ٶ�
    #endregion
    #endregion
    #region ���췽��
    public PickState(Fsm fsm, GameObject pickItemObject)
    {
        Debug.Log("����ʰȡ״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();
        this.pickItemObject = pickItemObject;
        this.pickableItems = fsmEnemy.pickableItems;
        this.rig = fsmEnemy.rig;
        this.fsmEnemyPrefabs = fsmEnemy.fsmEnemyPrefabs;
    }
    #endregion
    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        if(pickItemObject.tag == "EnvironmentWeapon")
        {
            Debug.Log("��ʼʰȡǹе");
            PickGun();
        }
        else if(pickItemObject.tag == "Potion")
        {
            Debug.Log("��ʼʰȡҩƿ");
            PickPotion();
        }
    }
    public void Update()
    {

    }
    #endregion


    private void PickGun()
    {
        this.moveSpeed = fsmEnemy.moveSpeed;
        this.fsmEnemyPrefabs = fsmEnemy.fsmEnemyPrefabs;
        this.level = fsmEnemy.level;
        Vector3 dir = pickItemObject.transform.position - fsmEnemy.transform.position;
        rig.velocity = dir * Time.deltaTime * moveSpeed * 100;
        if (pickableItems.Contains(pickItemObject))//�����ʰȡ��Ʒ���б���
        {
            GameObject.Instantiate(fsmEnemyPrefabs[level + 1], fsmEnemy.transform.position, fsmEnemy.transform.rotation);//�ȼ�+1
            fsmEnemy.DestroyItem(pickItemObject);//ɾ��ʰȡ���󣬱�ʾʰȡ�ɹ�
            fsmEnemy.Dead();//ԭ��������
        }
    }
    private void PickPotion()
    {
        this.hpValue = fsmEnemy.hpValue;
        this.magicValue = fsmEnemy.magicValue;
        this.currentHPValue = fsmEnemy.currentHPValue;
        this.currentMagicValue = fsmEnemy.currentMagicValue;
        this.damage = fsmEnemy.damage;
        this.shootSpeed = fsmEnemy.shootSpeed;
        Vector3 dir = pickItemObject.transform.position - fsmEnemy.transform.position;
        rig.velocity = dir * Time.deltaTime * moveSpeed * 100;
        if (pickableItems.Contains(pickItemObject))//�����ʰȡ��Ʒ�ڿ�ʰȡ������
        {
            switch (pickItemObject.GetComponent<Potion>().potionName)
            {
                case "HealthyPotion":
                    this.currentHPValue += 50;
                    if (this.currentHPValue > hpValue)
                    {
                        this.currentHPValue = hpValue;
                    }
                    fsmEnemy.DestroyItem(pickItemObject);
                    break;
                case "MagicPotion":
                    this.currentMagicValue += 50;
                    if (this.currentMagicValue > magicValue)
                    {
                        this.currentMagicValue = magicValue;
                    }
                    fsmEnemy.DestroyItem(pickItemObject);
                    break;
                case "RejuvenationPotion":
                    this.currentHPValue += 50;
                    if (this.currentHPValue > hpValue)
                    {
                        this.currentHPValue = hpValue;
                    }
                    this.currentMagicValue += 50;
                    if (this.currentMagicValue > magicValue)
                    {
                        this.currentMagicValue = magicValue;
                    }
                    fsmEnemy.DestroyItem(pickItemObject);
                    break;
                case "AgilityPotion":
                    this.moveSpeed += 1;
                    fsmEnemy.DestroyItem(pickItemObject);
                    break;
                case "RagePotion":
                    isRage = true;
                    this.moveSpeed += 5;
                    this.shootSpeed -= 0.1f;
                    this.damage += 10;
                    fsmEnemy.StartCoroutineInState("Calm"); 
                    fsmEnemy.DestroyItem(pickItemObject);

                    break;
                default:
                    break;
            }
        }
        fsm.SetEnemyState(new PatrolState(fsm));//����Ѳ��״̬
        EnemyPerspective.instance.isInPickState = false;
    }
}
