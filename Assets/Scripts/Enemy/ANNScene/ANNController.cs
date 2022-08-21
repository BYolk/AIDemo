using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���˿������ű�˼·�����ص�������
///     1���������У�ִ��Awake��������¼��������ʱ���˵�ǰλ��Ϊ��ʼλ�ã���¼���˵�ǰ�Ƕ�Ϊ��ʼ�Ƕȣ�����ʼ�����˵�������
///     
/// 
///     2��ÿ��0.02��ִ��һ��FixupUpdate������ InputSensors ��ȡ�������ϰ���֮��ľ��룬�����µ�ǰλ��Ϊ���˵����λ�ã������һ֡����ײǽ����ô��һ֡��¼��λ�þ����������˵����λ�ã������е��˵������磨����ֵ�ǵ������������������������ǽ�ľ��룩��������������ֵ�������ٶȺ�ת��ֵ�����ݼ��ٶȺ�ת��ֵ���õ����ƶ����� MoveCar �ƶ����ˣ�����¼�����ƶ�ʱ�� timeSinceStart ������ CalculateFitness �����������������Ӧ�ȡ�
///         2.1��InputSensors ������ȡ�������ϰ���֮��ľ��룺�������������������������߼��ֱ��ȡ������ǰ������ǽ�ľ��롢��ǰ����ǽ�ľ������ǰ����ǽ�ľ���
///         2.2��network.RunNetwork ��������NNet���ж���ķ�������������������
///         2.3��MoveCar ���������е��˵ķ�������������������ļ��ٶȺ�ת��ֵ���Ƶ�����μ������ת�򡣵��� Vector3.Lerp �������ݼ��ٶȼ��������ÿ֡�ƶ��ľ��루����������ͨ��transform.TransformDirection����������ת�����������ꡣ�ٸ���ת��ֵÿ֡��ת����
///         2.4��CalculateFitness ��������¼�����߹�����·�̺͵��˵ĵ�ǰ��ƽ���ٶȣ�������·�̡�ƽ���ٶȺ�������������ǽ�ľ���ֵ��������˵�������Ӧ�ȣ����������Ӧ��̫С�������ף����ߵ��˵�����ʱ��̫�ã�����Ϊ��������ʧЧ�������������ˡ�
///         2.5���� FixedUpdate ÿ�� 0.02s ���еĹ��̵��У�������˷�����ײ��Ҳ�����ٵ���
///     
/// 
///     3�����ٵ��˷�����Death()
///         1���� Death �����л��ҵ����˵Ļ�������� GeneticManager �ű�����������û���������� Death �������������������˵�������Ӧ�� overallFitness �� ������ network���ݹ�ȥ
///         2����������� GeneticManager �ᾭ��һϵ�в��������յ��û���������� ResetToCurrentGenome ���õ�ǰ�����鷽�����÷�������ñ������ ResetWithNetwork ��������һ���µ�������������õ�ǰ�ĵ���
///         
/// 
///     4���������������õ��˷�����ResetWithNetwork() �������������õ��������緽��
///         1���ڻ������������� ResetWithNetwork ������һ���µ�������������õ��˵�������
///         2���� ResetWithNetwork ���������õ���������֮��Ҫ��������µ������磬��Ҫ��������������ˣ������� ResetWithNetwork ��������Ҫ���� Reset����
///         
/// 
///     5�����õ��˷�����Reset() 
///         1�����õ��˵�����ʱ�䡢�ܾ��롢ƽ���ٶȡ����λ�á�������Ӧ�ȡ���ʼλ�úͿ�ʼ�Ƕ�
///         2�����õ��˷���ִ����Ϻ�õ�һ�������µ�δ���й����������NPC�������� FixedUpdate ��������֡����������
///         
///     6����Ҫ�����ǣ�
///         1���� FixedUpdate ���еĹ����У�ִ�е� network.RunNetwork ����������ʲô������Ҫ��� NNet ������ű�
///         2���������� Death() �����˻�������� GeneticManager �ű�����������������Ҫ��� GeneticManager ����������ű�
/// </summary>
public class ANNController : MonoBehaviour
{
    #region ��Ա
    public Vector3 startPosition;//���˿�ʼʱ��λ��
    private Vector3 lastPosition;//����λ�ã�����ײǽ��λ�ã�

    [Range(-1f, 1f)]
    public float x, y;//x ��ʾ��һ��λ�õ� x ���꣬y ��ʾ��һ��λ�õ� y ����
    public float timeSinceStart = 0f;//��������״̬��������ʱ�䣬�����������Ƿ�����̫�ã��������̫��˵����������ʧЧ


    [Header("Fitness")]
    public float overallFitness;//Fitness �������û�������û���ָ��
    public float totalDistanceMultipler = 6.4f;//�ܾ��뱶�������������Ӿ��룬�ߵ���Զ�ĵ���������Ҫ�ġ�
    public float disFormDoorMultipler = 0.001f;//�����ŵľ��뱶����
    public float sensorMultiplier = 0.2f;//���������������������Դ�������ǽ�ľ���

    

    //����Ӧ����صı������ܹ��ߵľ���Խ������Ҫ����� Door �ŵľ���ԽС��������ǽ�ľ���Խ����˵����Ӧ�����
    private float totalDistanceTravelled;//�ܹ��ߵľ���
    private float aSensor, bSensor, cSensor;//���˵�������������ǽ�ľ��룺�Ӵ������������ߣ�������ǽ��ײ�󣬻�ȡ���߾���
    private float disFromDoor;//���ŵľ���


    private ANN network;//�����������
    [Header("Network Options")]
    public int LAYERS = 1;//���������
    public int NEURONS = 10;//���������Ԫ�ڵ���


    Transform doorTrans;//Ҫ��������
    Rigidbody2D enemyRig;//�������
    private Vector3 newPosition;//������һ֡���ڵ�λ��
    int enemyLayerMask;//����������ײ�Ĳ㼶
    #endregion






    #region ��ʼ��
    /// <summary>
    /// ��ʼ������ʼ����ʼλ�ú�����ű�
    /// </summary>
    private void Awake()
    {
        Debug.Log("��ʼ�������������");
        startPosition = transform.position;
        network = new ANN();//��ʼ��������ű�����
        enemyRig = transform.GetComponent<Rigidbody2D>();
        doorTrans = GameObject.Find("Door2").transform;
        enemyLayerMask = LayerMask.NameToLayer("Enemy");//Ҫ���˵����߼��㼶
    }
    #endregion







    #region ����
    /// <summary>
    /// ������ͬ���Դ����Ĳ�ͬӲ���������ڹ̶���֡������ִ��
    /// </summary>
    private void FixedUpdate()
    {
        InputSensors();//��ȡ��������ǽ�ľ���
        //Debug.Log("aSensor:" + aSensor.ToString() + "--------------bSensor:" + bSensor.ToString() + "------------cSensor:" + cSensor.ToString());
        lastPosition = transform.position;//�õ���ǰλ�ã���Ϊִ����һ֮֡ǰ�����һ��λ��
        disFromDoor = Vector3.Distance(transform.position, doorTrans.position);
        (x, y) = network.RunNetwork(aSensor, bSensor, cSensor);//ͨ�����������磬�õ����ֵ
        Escape(x, y);//��������� x �� y �õ��µ�����
        timeSinceStart += Time.deltaTime;//�ۼ�ʱ��
        CalculateFitness();//������Ӧ��
    }
    #endregion







    #region InputSensors �������������߼���ȡ������������ǽ�ľ���
    /// <summary>
    /// ��ȡ��������ǽ�ľ���
    /// </summary>
    private void InputSensors()
    {

        Vector3 a = (transform.up + transform.right);//��ǰ������
        Vector3 b = (transform.up);//��ǰ������
        Vector3 c = (transform.up - transform.right);//��ǰ������


        RaycastHit2D hitA = Physics2D.Raycast(transform.position, a, 200 , ~(1 << enemyLayerMask));
        if (hitA.collider)//�������r������ײ������ײ��Ϣ�����hit��
        {
            aSensor = hitA.distance / 20;//a����������ǽ�ľ������20,ע�������뵽�������ʱ����Ҫ��ֵ���б�׼������Ҫȷ�������������ֵ��ʱ��-1��1��0��1��
        }

        //�õ� bSensor �� cSensor ��ֵ
        RaycastHit2D hitB = Physics2D.Raycast(transform.position, a, 200, ~(1 << enemyLayerMask));
        if (hitB.collider)
        {
            bSensor = hitB.distance / 20;
        }

        RaycastHit2D hitC = Physics2D.Raycast(transform.position, a, 200, ~(1 << enemyLayerMask));
        if (hitC.collider)
        {
            cSensor = hitC.distance / 20;
        }
    }
    #endregion









    #region �����ƶ�����
    

    /// <summary>
    /// �����ƶ�����
    /// </summary>
    /// <param name="v">�������ܷ��������� x ����</param>
    /// <param name="h">�������ܷ��������� y ����</param>
    public void Escape(float x, float y)
    {
        newPosition = Vector3.Lerp(Vector3.zero, new Vector3(x * 21.4f, y * 11.4f, 0), 0.02f);//���������������� x �� y �����ÿ֡���ӵľ���
        newPosition = transform.TransformDirection(newPosition);//����ǰ���巽������ת������������ϵ��
        newPosition.x = newPosition.x + transform.position.x;
        newPosition.y = newPosition.y + transform.position.y;
        transform.position = newPosition;
        
    }
    #endregion








    #region CalculateFitness �������������Ӧ�ȵķ���
    /// <summary>
    /// ���� overallFitness
    /// </summary>
    private void CalculateFitness()
    {

        totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);//ÿһ֡�������һ��λ������Ϊ��ǰλ�ã�����ÿһ֡���ӵľ�����ǵ�ǰλ�ú����һ��λ��֮��ľ���
        //overallFitness = (totalDistanceTravelled * totalDistanceMultipler) + 1.0f / (disFromDoor * disFormDoorMultipler) + (3 / ((aSensor + bSensor + cSensor) * sensorMultiplier));
        overallFitness = 1 / (disFromDoor * disFormDoorMultipler) + totalDistanceTravelled * totalDistanceMultipler;
        if ((transform.position.x > 37 && transform.position.y > 13) || (transform.position.x < 48 && transform.position.y > 13))
        {
            //Debug.Log("�ɹ�Խ���ϰ��ﲢ�������ѿ�--------------------------------------------------------");
            //Debug.Log("��ǰ��Ӧ��Ϊ��" + overallFitness.ToString());
        }
        if (timeSinceStart > 20 && overallFitness < 40)
        {//�����������̫�û�������Ӧ��С��40����˵��������������Ч�����ٵ���
            //Debug.Log("��Ϊ��������̫�û�������Ӧ��С��40������");
            Death();
            
        }
        if (overallFitness >= 1000)
        {//���������Ӧ�ȹ������ٵ���
            //Debug.Log("��Ϊ������Ӧ�ȹ��������:" + overallFitness.ToString());
            Death();
            
        }
    }
    #endregion







    #region OnCollisionEnter ������ײ��ⷽ��
    /// <summary>
    /// ��ײ����������ײʱ��ִ����������
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        Death();
    }
    #endregion







    #region ��������
    /// <summary>
    /// �������������������˵���Ӧ�Ⱥ������綼���ݸ���������������ű����������浽������
    /// </summary>
    public void Death()
    {
        GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness, network);
    }
    #endregion






    #region ���õ��˷���
    /// <summary>
    /// �������纯����������ײǽ����������͵���
    /// </summary>
    /// <param name="net"></param>
    public void ResetWithNetwork(ANN net)
    {
        network = net;
        Reset();
    }








    /// <summary>
    /// ���õ��˺�����������ײǽ�����õ���
    /// </summary>
    public void Reset()
    {
        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        transform.position = startPosition;
    }
    #endregion



}
