using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region

    #endregion






    #region ����

    /// <summary>
    /// ����
    /// </summary>
    public static Player instance;
    #endregion






    #region �����ر�������
    //������Ѫ����ħ��ֵ���ƶ��ٶȣ�����ƶ���������ꣻ����ƶ�������ҵ�ǰ��������
    public int hpValue = 100;            
    public int magicValue = 100;
    int currentHPValue = 100;
    int currentMagicValue = 100;
    public float moveSpeed = 50f;
    Vector2 newPosition;
    Vector2 moveDir;
    Vector2 lookAt;
    public bool isRage = false;


    //���ã���Ҷ���;��Ҹ���;���ģ�Ͷ���
    GameObject player;
    Rigidbody2D rig;
    GameObject model;
    public GameObject deadPlayer;
    #endregion





    #region װ����ر�������
    //��������������ٶȣ������ʱ������ǰ��ҿ�ʰȡ����������ǰ����ֳ������Ƿ�Ϊ���������ӵ����㼶�����߼��㼶;����λ��
    public List<GameObject> pickableItems = new List<GameObject>();
    LayerMask layerMask;
    string bulletLayer;
    float weaponSpeed;
    float shootTimer = 0.02f;//�����ʱ������shootTimer����weaponSpeedʱ���������һ��     
    bool isMainWeapon = true;
    Vector3 weaponPos;

    //���ã�������������������ʼ����Ԥ���壻��ǰ�ֳ�װ�����ӵ����λ��;�ӵ�Ԥ����
    public GameObject mainWeapon;
    public GameObject secondaryWeapon;
    public GameObject bornWeaponPrefab;
    public GameObject currentHandle;
    public GameObject bulletPrefab;
    Transform shootPos;
    #endregion






    #region UI��ر�������
    //���ã�Ѫ�����ͼƬ��ħ��ֵ���ͼƬ
    Image hpFillImage;
    Image magicFillImage;
    Text hpValueText;
    Text magicValueText;
    #endregion





    #region ��ʼ��

    /// <summary>
    /// ʵ������������
    /// </summary>
    private void Awake()
    {
        instance = this;                                                                            //��ʼ������
    }



    /// <summary>
    /// ��ʼ�����������ã���Ҷ������ģ�Ͷ�����Ҷ�����������������û�и���������ֶ���Ӳ�������ֵΪ0
    /// ��ʼ��װ��������ã����������󣻵�ǰ�������󣻵�ǰ������������󣻵�ǰ����λ�ã������������ʣ���������λ��;����λ��
    /// ��ʼ��UI������ã�Ѫ��ͼƬ��ħ��ֵͼƬ
    /// </summary>
    void Start()
    {
        //������
        player = GameObject.Find("Player").gameObject;
        model = this.transform.Find("Model").gameObject;
        rig = GetComponent<Rigidbody2D>();
        if (rig == null)
        {
            rig = this.gameObject.AddComponent<Rigidbody2D>();
        }
        rig.gravityScale = 0;


        //װ�����
        mainWeapon = Instantiate(bornWeaponPrefab);
        currentHandle = mainWeapon;

        ChangeCurrentHandle();

        //UI���:
        hpFillImage = GameObject.Find("HPFillImage").GetComponent<Image>();
        hpFillImage.fillAmount = currentHPValue / hpValue;
        hpValueText = GameObject.Find("HPValueText").GetComponent<Text>();
        hpValueText.text = Convert.ToString(currentHPValue) + "/" + Convert.ToString(hpValue);

        magicFillImage = GameObject.Find("MagicFillImage").GetComponent<Image>();
        magicFillImage.fillAmount = currentMagicValue / magicValue;
        magicValueText = GameObject.Find("MagicValueText").GetComponent<Text>();
        magicValueText.text = Convert.ToString(currentMagicValue) + "/" + Convert.ToString(magicValue);
    }
    #endregion






    #region ����

    /// <summary>
    /// �����������ȡ�����һ�ƶ��������귽��������ƶ������������ת�������л�����������ʰȡ��������
    /// </summary>
    void Update()
    {
        if (shootTimer >= weaponSpeed)
        {
            Shoot();
        }
        else
        {
            shootTimer += Time.deltaTime;
        }
        BeforeMove();
        Move();
        Rotate();
        SwitchWeapon();
        PickWeapon();
        PickPotion();
    }
    #endregion






    #region ����ƶ�����ת



    /// <summary>
    /// �ڽ�ɫ�˶�ǰ��ȡ��ɫ��һ���˶�λ��������
    /// </summary>
    void BeforeMove()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        newPosition = moveDir * moveSpeed * Time.deltaTime;
    }
    private void Move()
    {
        rig.velocity = newPosition * 100;
    }
  /// <summary>
    /// ������A��������ת������D��������ת
    /// ��ȡ��ǰ��Ҷ���λ�ã��������ǰλ��ֻ���������λ�õ�������x�����0��˵��������ұߣ����ҿ�����֮����
    /// </summary>
    private void Rotate()
    {
        Vector3 dir = Camera.main.WorldToScreenPoint(transform.position);
        lookAt = Input.mousePosition - dir;
        Vector2 shootDir = lookAt.normalized;
        //��ת����������
        if (lookAt.x > 0)
        {
            model.transform.eulerAngles = new Vector3(0, 180, 0);
            if (shootDir.y > 0)
            {
                currentHandle.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(shootDir, new Vector2(1, 0)));
            }
            else
            {
                currentHandle.transform.eulerAngles = new Vector3(0, 0, 360 - Vector3.Angle(shootDir, new Vector2(1, 0)));
            }
        }
        else
        {
            model.transform.eulerAngles = new Vector3(0, 0, 0);
            if (shootDir.y > 0)
            {
                currentHandle.transform.eulerAngles = new Vector3(0, 180, Vector3.Angle(shootDir, new Vector2(-1, 0)));
            }
            else
            {
                currentHandle.transform.eulerAngles = new Vector3(0, 180, 360 - Vector3.Angle(shootDir, new Vector2(-1, 0)));
            }
        }
    }
    #endregion






    #region ���
    /// <summary>
    /// 1������ҵ������������ʼ����
    /// 2���Ӷ���أ��ű����л�ȡ�ӵ�����
    /// 3�������ӵ����λ�ã����ӵ����λ��shootPos.position��ֵ���ӵ�����
    /// 4�������ӵ��������
    ///     1���ӵ����������ǵ�ǰ��Ϸ����������ķ���
    ///     2�����÷�����������Ҷ�������λ�õ��������λ�õ���������һ�������жϸ�������������ڼ�����
    ///     3�������������ڵ�һ�����ޣ�����һ������y����ֵ����0������ӵ���Z����ת��X�����������������ļнǡ�����
    ///     4�������������ڵ��������ޣ�����ͬ��3��������Ҫ��360���ȥ������3�����ö���
    /// 5����ȡ�ӵ������������Ϊ���һ���������ķ���Ϊ����������Ĵ�СΪ�ӵ��ٶ�
    /// 6�������ӵ��㼶���ӵ�ֻ���ض���������ײ�������ӣ����˵ȣ�
    /// </summary>
    private void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            int consumeMagic = currentHandle.GetComponent<Gun>().consumeMagic;
            if (consumeMagic <= currentMagicValue)
            {
                shootTimer = 0;
                GameObject bullet = PlayerBulletPoolManager.instance.getBullet();
                bullet.transform.position = currentHandle.transform.Find("ShootPos").position;
                Vector2 shootDir = lookAt.normalized;
                if (shootDir.y > 0)
                {
                    bullet.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(shootDir, new Vector2(1, 0)));
                }
                else
                {
                    bullet.transform.eulerAngles = new Vector3(0, 0, 360 - Vector3.Angle(shootDir, new Vector2(1, 0)));
                }
                bullet.GetComponent<Rigidbody2D>().AddForce(shootDir * Bullet.instance.speed);
                Bullet.instance.damage = currentHandle.GetComponent<Gun>().damage;  //��ȡ�������˺�ֵ��ֵ���ӵ�
                currentMagicValue -= consumeMagic;

                //����UI���������ħ��ֵ
                updateMagic();
            }
        }
    }
    #endregion






    #region ������ز���



    /// <summary>
    /// 1�������ʰȡ������������0���Ұ���E����ʰȡ������������������pickableWeapons[0]
    /// 2�����������Ϊ�գ�ʰȡ������������������
    ///     1����ȡ������
    ///     2������ʰȡ�����������Ƴ���ʰȡ����������
    ///     3����������״̬����ǰ����ʱ������������Ҫ���ø�����
    ///     4�������������������
    ///     5����������TagΪPlayerWeapon
    /// 3�������������Ϊ�գ����жϵ�ǰ����ʱ���������Ǹ�����������ǰ��������ϵ��������и�����
    ///     1�������ǰ��������������
    ///         1������ʰȡ�����Ķ����λ�ñ���������tempWeapon
    ///         2������������������Ϊ��
    ///         3��������������λ�ø���Ϊ����ʰȡ�������󡱵�λ��
    ///         3.1����������Tag��ΪEnvironmentWeapon
    ///         4��������������ֵ������ʰȡ�������ϡ����±�Ϊ0��λ�ã���ʾʰȡ��������������Ϊ�����������Ŀ�ʰȡ��������
    ///         5������ʱ����Ŀ�ʰȡ��������tempWeapon��ֵ������������
    ///         6���������õ�ǰ���ֶ���currentHandle
    ///         7����Ϊ��ǰ���ֶ������仯������ChangeCurrentHandle()����
    ///     2�������ǰ�����Ǹ�������ͬ��
    /// </summary>
    private void PickWeapon()
    {
        if (pickableItems.Count > 0 && pickableItems[0].tag == "EnvironmentWeapon" && Input.GetKeyDown(KeyCode.E))
        {
            if (secondaryWeapon == null)
            {
                secondaryWeapon = pickableItems[0];
                pickableItems.RemoveAt(0);
                secondaryWeapon.SetActive(false);
                secondaryWeapon.transform.parent = this.transform;
                secondaryWeapon.gameObject.tag = "PlayerWeapon";
            }
            else if(currentHandle == mainWeapon)
            {
                
                GameObject tempWeapon = pickableItems[0];

                mainWeapon.transform.parent = null;
                mainWeapon.transform.position = pickableItems[0].transform.position;
                mainWeapon.gameObject.tag = "EnvironmentWeapon";
                pickableItems.RemoveAt(0);

                mainWeapon = tempWeapon;
                currentHandle = mainWeapon;
                ChangeCurrentHandle();
                //Debug.Log(pickableItems.Count);
            }
            else if(currentHandle == secondaryWeapon)
            {
                GameObject tempWeapon = pickableItems[0];//����ǰ��ʰȡ�������浽һ��������

                secondaryWeapon.transform.parent = null;
                secondaryWeapon.transform.position = pickableItems[0].transform.position;
                secondaryWeapon.gameObject.tag = "EnvironmentWeapon";
                pickableItems.RemoveAt(0);

                secondaryWeapon = tempWeapon;
                currentHandle = secondaryWeapon;
                ChangeCurrentHandle();
            }
        }
    }




    /// <summary>
    /// �л���������
    /// ���������Ϊ�գ�����Q�޲���
    /// �����������λ�գ�����Q�������������isMainWeaponΪTrue������ǰΪ�������������Ϊ����������֮����Ϊ������
    /// �л�����֮�󣬻�Ҫ���ã�
    ///     1��������״̬��SetActive��
    ///     2��������λ��
    ///     3�������Ĺ����ٶ�
    ///     4����������λ��
    /// </summary>
    private void SwitchWeapon()
    {
        if (secondaryWeapon != null && Input.GetKeyDown(KeyCode.Q))
        {
            if (isMainWeapon)
            {
                isMainWeapon = false;
                mainWeapon.SetActive(false);
                secondaryWeapon.SetActive(true);
                currentHandle = secondaryWeapon;
                ChangeCurrentHandle();
                
            }
            else
            {
                isMainWeapon = true;
                mainWeapon.SetActive(true);
                secondaryWeapon.SetActive(false);
                currentHandle = mainWeapon;
                ChangeCurrentHandle();
            }
        }
    }




    /// <summary>
    /// ������ǰ��������������ʱ�����õķ���
    /// �жϵ�ǰ���常�����Ƿ�ΪPlayer��������ǣ����ĵ�ǰ�����ĸ�����
    /// �жϵ�ǰ����Tag�ǲ���PlayerWeapon��������ǣ�����ΪPlayerWeapon
    /// ���ĵ�ǰ����λ��
    /// ���ĵ�ǰ�����ٶ�
    /// ���ĵ�ǰ��������λ��
    /// </summary>
    private void ChangeCurrentHandle()
    {
        if(currentHandle.transform.parent == null)
        {
            currentHandle.transform.parent = this.transform;
        }
        if(!currentHandle.CompareTag("PlayerWeapon"))
        {
            currentHandle.gameObject.tag = "PlayerWeapon";
        }
        currentHandle.transform.position = player.transform.position + currentHandle.GetComponent<Gun>().weaponPos;//��������λ��
        weaponSpeed = currentHandle.GetComponent<Gun>().cooling;
        shootPos = currentHandle.transform.Find("ShootPos");
    }
    #endregion




    #region ʰȡҩƿ����
    /// <summary>
    /// �����ʰȡ��Ʒ��������0�ҵ�һ����ʰȡ��Ʒ�����tagΪ��ʾҩƿ��Potion��������Ұ���E��ʱʰȡҩƿ
    /// ����Ѫƿ���ָ�50hp
    /// ����ħ��ƿ���ָ�50ħ��ֵ
    /// ���ûָ�ҩƿ���ָ�50hp��50ħ��ֵ
    /// ��������ҩƿ���ƶ��ٶ����1
    /// ���ÿ�ҩƿ������ƶ��ٶȣ�����������ȴ�ʣ���߹�������������Э�̣���10s��ȡ����ҩƿ������
    /// </summary>
    private void PickPotion()
    {
        if (pickableItems.Count > 0 && pickableItems[0].tag == "Potion" && Input.GetKeyDown(KeyCode.E))
        {
            switch (pickableItems[0].GetComponent<Potion>().potionName)
            {
                case "HealthyPotion":
                    this.currentHPValue += 50;
                    if(this.currentHPValue > hpValue)
                    {
                        this.currentHPValue = hpValue;
                        updateHP();
                    }
                    else
                    {
                        updateHP();
                    }
                    Destroy(pickableItems[0].gameObject);
                    
                    break;
                case "MagicPotion":
                    this.currentMagicValue += 50;
                    if (this.currentMagicValue > magicValue)
                    {
                        this.currentMagicValue = magicValue;
                        updateMagic();
                    }
                    else
                    {
                        updateMagic();
                    }
                    Destroy(pickableItems[0].gameObject);
                    
                    break;
                case "RejuvenationPotion":
                    this.currentHPValue += 50;
                    if (this.currentHPValue > hpValue)
                    {
                        this.currentHPValue = hpValue;
                        updateHP();
                    }
                    else
                    {
                        updateHP();
                    }
                    this.currentMagicValue += 50;
                    if (this.currentMagicValue > magicValue)
                    {
                        this.currentMagicValue = magicValue;
                        updateMagic();
                    }
                    else
                    {
                        updateMagic();
                    }
                    Destroy(pickableItems[0].gameObject);
                    
                    break;
                case "AgilityPotion":
                    this.moveSpeed += 1;
                    Destroy(pickableItems[0].gameObject);
                    
                    break;
                case "RagePotion":
                    isRage = true;
                    this.moveSpeed += 5;
                    this.currentHandle.GetComponent<Gun>().cooling -= 0.1f;
                    this.currentHandle.GetComponent<Gun>().damage += 999;
                    StartCoroutine("Calm");
                    Destroy(pickableItems[0].gameObject);
                    
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// ��ҩƿʧЧЭ�̷���
    /// </summary>
    /// <returns></returns>
    IEnumerator Calm()
    {
        //��10��
        yield return new WaitForSeconds(10f);
        isRage = false;
        this.moveSpeed -= 5;
        this.currentHandle.GetComponent<Gun>().cooling += 0.1f;
        this.currentHandle.GetComponent<Gun>().damage -= 999;
    }
    #endregion






    #region ��Ѫ����������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            
            currentHPValue -= collision.gameObject.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
            //����UI���������Ѫ��
            if (currentHPValue <= 0)//�������
            {
                List<GameObject> enemies = new List<GameObject>();
                GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().enemyState = Enemy.EnemyState.PATROL;
                }
                currentHPValue = 0;
                hpFillImage.fillAmount = 0;
                hpValueText.text = "0/100";
                Instantiate(deadPlayer, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
                Invoke("ReturnToMenu", 5);//��Ϸʧ��5s����ת���˵�����
            }
            else
            {
                updateHP();
            }
        }
    }
    /// <summary>
    /// �����������ת���˵�
    /// </summary>
    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region ����Ѫ��ħ��ֵUI����
    private void updateHP()
    {
        hpFillImage.fillAmount = (float)Math.Round((decimal)currentHPValue / hpValue, 2);//���������λС��
        hpValueText.text = Convert.ToString(currentHPValue) + "/" + Convert.ToString(hpValue);
    }

    private void updateMagic()
    {
        magicFillImage.fillAmount = (float)Math.Round((decimal)currentMagicValue / magicValue, 2);//���������λС��
        magicValueText.text = Convert.ToString(currentMagicValue) + "/" + Convert.ToString(magicValue);
    }
    #endregion
}
