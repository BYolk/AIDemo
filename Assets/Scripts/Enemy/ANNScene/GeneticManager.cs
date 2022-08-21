using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;

/// <summary>
/// ����������ű�˼·��ʹ���Ŵ��㷨���ڳ����д��������岢���ش˽ű������ǹ��ص����˶����ϣ�
///     1����ظ��
///         1.1���Ŵ��㷨��������Ȼѡ����ÿһ��֮��ѡȡ������õĸ��壬������ͨ���ӽ���ֳ�õ��Ӵ��������Ӵ����и��ʷ���ͻ�䣬�����Ӵ��Ķ�����
///         
/// 
/// 
///     2��������
///         2.1��initialPopulation������ÿһ�����˿���������Ⱥ������
///         2.2��mutationRate��ͻ����ʣ������ȡһ��ֵ��С�����ͻ���������Ϊ����ͻ��
///         2.3��bestAgentSelection�����ôӵ����л�ȡ���ܱ�����õĸ�����
///         2.4��worstAgentSelection�����ôӵ����л�ȡ���ܱ������ĸ�����
///         2.5��numberToCrossover������ѡȡ�����ĸ���Ľ��深ֳ�Ĵ���
///         2.6��controller����ǰ�������еĸ���Ŀ������ű�����Ϊʲô���Ǵ�����������ʾ��Ⱥ�����������ǹ�ע���������磬����ע���� NNet �ű��������� controller �ű���ֻҪΪ controller ������ͬ�� NNet �ű����Ϳ��Ա�ʾ��ͬ����Ⱥ����
///         2.7��genePool������⣬�˱�����Ŀ�Ĳ��ý��ͣ���ϴ���������
///         2.8��naturallySelected����Ȼѡ������������������Ŀ�Ĳ���˵������ϴ���������
///         2.9��population����Ⱥ�����˿������������� NNet ���飬�ò�ͬ�� NNet �ű�����ͬ���������ʾ��ͬ�ĸ��壨ɸѡ���ֺõĸ�����ʵ����ɸѡ���ֺõ������磩
///         2.10��currentGeneration����ǰΪ�ڼ���
///         2.10��currentGenome����ǰ�Ļ����飬����ǰΪ�ڼ�����Ⱥ���壬����˵��ǰΪ�ڼ���������
///         
/// 
///     3���������ع����д˽ű�����Ϸ����ִ�д˽ű���Start�������г�ʼ������Start�����е��� CreatePopulation ����������Ⱥ
///     
/// 
/// 
///     4��CreatePopulation() ������Ⱥ������
///         1������һ�� ANN ���飬�����С�Ƕ���õġ���ʼ����Ⱥ���������˴���ʼ����Ⱥ����Ϊ 85������ʼ�� 85 �� NNet �ű�����
///         2������ FillPopulationWithRandomValues ������Ϊ���������鸳ֵ��������������󱣴浽�����������У�
///         3������ ResetToCurrentGenome ���������õ�ǰ�����飨�������������±�Ϊ currentGenome �������紫�ݸ� CarController �ű���������ȡ�����˵������磨�������˵������磩
///         
/// 
/// 
///     5��void FillPopulationWithRandomValues(NNet[] newPopulation, int startingIndex) ʹ������������Ⱥ������
///         1���÷�����Ҫ����������һ������Ҫ������Ⱥ���飬�����������飻һ�����������±꣬��ʾ���������������һ����ʼ���
///         2��ʹ�� while ������ѭ������ startingIndex �±�λ�ô��ڵ�����Ⱥ����ʱ��ֹͣѭ��
///         3����ѭ�������У�ʵ������Ⱥ���壬��ʵ���������磬new һ�� NNet �ű����󣬸��� startingIndex λ�ô�������Ԫ�أ�����ʼ��ÿһ�������磬����ʼ��ÿһ������������ز㡢Ȩ�غ�ƫ��ֵ��Ȩ�غ�ƫ��ֵ������ģ�����Ҫ�����Ѿ��趨�õ����ز����������ز���Ԫ������controller ���趨�����ز����������ز���Ԫ������
///         
/// 
/// 
///     6��ResetToCurrentGenome() ���õ�ǰ�����鷽�����÷�������� CarController �� ResetWithNetwork ����������һ����Ⱥ���壨�����磩�� CarController �ű���������ȡ�� CarController �ű�����ǰ�������硣
///     
/// 
/// 
///     7��ִ�� Start �����󣬹����д˽ű��Ŀն����������һ������Ϊ 85(initialPopulation) �����������飬��ͨ�� FillPopulationWithRandomValues �������� 85 �������������䵽�����������У���Ϊ currentGenome ��ʼֵΪ0��Start ��������� ResetToCurrentGenome ������ ResetToCurrentGenome ������ִ�� controller.ResetWithNetwork(population[currentGenome]) �� controller.ResetWithNetwork(population[0]) ������������ĵ�һ�������紫�ݵ� controller �ű������У���������Ϊ controller �ű�����������硪��������Ҫע����ǣ�controller �ű������ڽ��� GeneticManager �ű����ݹ�ȥ������������ĵ�һ�����������֮ǰ�������Ѿ�������һ������������������������� controller �ű��� Awake ������ʵ�������� GeneticManager �ű�ʵ��������������������� Start ������ִ�еġ�
///     
/// 
/// 
///     8�����ű����˳������ؾͻ����е� Start �����Լ��� Start �������õķ���֮�⣬ʣ�µ�����ִ�еķ������ǵ����˶�������ʱ��������ʱ������ײ���٣�����ʱ��Ӧ��̫С����ֵĴ�����٣�Ҳ����ʱ����ʱ����ö����٣�CarController �ű��������� Death ���������÷����ֻ���ñ��ű��� Death() ����
///     
/// 
/// 
///     9��Death()������
///         9.1���ڳ���ʼ��ʱ��GeneticManager �ű��Ὣʵ����������������ĵ�һ����������󴫵ݸ� Contorller �ű��������������ѵ����������ǰ��ѵ������������±��¼�� currentGenome �У����� currentGenome ��ʼ��Ϊ0���� Controller �ű����� Death ����ʱ��������һ��������ѵ����ϣ�Controller �� Death ��������ñ��ű��� Death �����������ݸ��������ѵ����� fitness ����
///         9.2�����ű��� Death �������жϵ�ǰ����ѵ����������ʱ����������ĵڼ��������磬��Ϊ currentGenome Ϊ0����ʾ��ǰ����ѵ����������������������ĵ�1��Ҳ�����±�Ϊ0�������磬˵�������������黹�� 84 ��������û��ѵ���������Ƚ���ǰ����ѵ�����������ѵ���������������� fitness ���浽������������ fitness �������С�
///         9.3������õ�ǰ����ѵ�����������ѵ�������currentGenome ����1�����������������±��1���������Ƶ�����һ��δѵ�����������ϣ������� ResetToCurrentGenome() ������δѵ���������紫�ݵ� CarController �ű���������Ѿ�����ѵ���������磬Ȼ�� FixedUpdate ���������ѵ����������
///         9.4���Դ�����
///         9.5���� CarController ѵ������������ GeneticManager �����������е����һ��Ԫ��ʱ���� currentGenome = population.Length - 1������ GeneticManager �����������Ѿ�ȫ��ѵ����ϣ�����������Ⱥ�Ѿ�ȫ��ѵ����ϣ�������Ҫ������һ����Ⱥ��ѵ�������� RePopulate() ����������Ⱥ�����һ����Ⱥ�����������飩���Ŵ��㷨�ľ���������һ����Ⱥ������һ����Ⱥ�Ŵ�����������
/// 
/// 
/// 
///     10��Repopulate()��
///         10.1����������Ⱥ����������������ʱ����Ҫ���ü��ϵ� .Clear ������ genePool ����أ�����⣩������գ�ȷ�� genePool ����Ϊ�գ�������Ҫ������Ӻܶ�ܶ�����������
///         10.2��naturallySelected ��ֵΪ 0�����������Ŀ�Ĳ���˵������ϴ���������
///         10.3��currentGeneration ����1����ʾ������һ����Ⱥѵ��
///         10.4������ SortPopulation() ��������һ����Ⱥ�������򣬽����ܱ�����õĸ��壨�����磩������ǰ�棬�������������
///         10.5������ PickBestPopulation() ���������ܱ�����ú�����ǰ�������壨�����磩���浽 newPopulation ������
///         10.6������ Crossover(newPopulation)�����Ա���������������ͻ����ǰ����������н��深ֳ����õڶ������壨�����磩
///         10.7������ Mutate(newPopulation) ��������ֳ�ĸ�����ʽ��л���ͻ�䣨����ı�Ȩ��ֵ��ƫ��ֵ��
///         10.8������ FillPopulationWithRandomValue() ��������0��ʼʵ���� newPopulation ����
///         10.9���� newPopulation �����ǰ�� population���������ǰ����Ⱥ�����������飩
///         10.10���� currentGenome ��ǰѵ���������������±�����Ϊ0
///         10.11������ ResetToCurrentGenome() ����������ǰ��һ��������������ĵ�һ����������󴫵ݸ� CarController �������ѵ��
///         10.12����ʼ�ڶ���������ѵ������
///         
/// 
/// 
///     11��SortPopulation()��ʹ��ð�������㷨���Ѿ�ѵ����ϵĵ�����Ⱥ�����������飩 population ��������ָ�� fitness �Ӵ�С����
///     
/// 
///     12��NNet[] PickBestPopulation()����ѡ���ܱ�����õĺ����ļ������壨�����磩����������ʽ����
///         12.1������һ���µ���Ⱥ��������������������� newPopulation
///         12.2��ѭ�� bestAgentSelection �Σ�bestAgentSelection��ʼ��Ϊ8����ѭ��8�Σ�����Ⱥ�����������飩�б�����õ� 8 ��������ѡ�������浽 newPopulation �У���Ϊ��Ⱥ�Ѿ��������ܱ�������ã�������ѡ�ĸ��壨�����磩������Ⱥ�����������飩���±�Ϊ 0 �� 7 ��Ԫ�أ���ѡ���ĸ����±��� naturallySelected ����ʾ������ѡ���ĸ��������Ȼѡ��Ľ��������Ҳ��Ϊʲô��������Ⱥ�㷨����Ҫ�� naturallySelected ��Ϊ0��
///         12.3����ѭ�������е������������� InitialiseCopy ����������ǰ�������磬�õ���ǰ������ĸ����������������縱������Ӧ�� fitness ����Ϊ0��Ȼ�󱣴浽 newPopulation ����Ӧλ�ã� naturallySelected ������ 1
///         12.4��������Ҫ��������������ǣ���ѡ�����Ķ���Ҫ�������深ֳ�����深ֳ�Ķ��󶼷��� genePool ���棬��Ϊ���ܱ��ֺõĶ���ѡ�з�ֳ�ĸ���Ҫ������ genePool �����ܱ��ֺõĸ���������ҪԽ�࣬����ѭ�����±�ֵ i �������������󡣾�����������ǽ���������� fitness * 10��Ȼ��תΪInt�������֣���Ϊ f��Ȼ���� f Ϊѭ������ѭ�� f �ˣ�ÿѭ��һ�ν� i ��ӵ� genePool �У����ܱ��ֺõ� fitness ֵ�͸ߣ�����10֮�������Խ��ѭ���Ĵ�����Խ�࣬������������ i ֵ����ӵ� genePool �Ĵ�����Խ�ࡣ
///         12.5��ͬ��ѭ�� worstAgentSelection(3) �Σ���ѡ�����ܱ������� 3 �����壨�����磩����������Щ���屣�浽 newPopulation �У����ǽ�����������������±�ֵ last ��ӵ� genePool ���棬��ӷ���ͬ��
///         12.6����ѡ��ϣ����� newPopulation����ʱ newPopulation �����а˸�����������ǵ���ѵ����Ϻ������õİ˸������磩
///         12.7����Ҫע����ǣ���ѡ�� newPopulation �����е�ֻ�����ܱ�����õ���������󣬶�û�����ܱ���������������󡣰����ܱ������������������±�ֵ���浽 genePool ��ԭ���ǣ��ڽ��深ֳ���� Crossover �У�����ʴ� 8 ����õĺ��Ӻ� 3 �����ĺ����ܹ� 11 �������������ѡ���������������ܰ����������Խ������з�ֳ��
///         
/// 
/// 
///     13��void Crossover(NNet[] newPopulation)�����深ֳ����
///         13.1��newPopulation ���������������Ҫ���н��深ֳ������������
///         13.2��ѭ�� numberToCrossover �Σ������深ֳ������������
///         13.3����ѭ�������ж��������±� Aindex �� Bindex��Ȼ��Ƕ��ѭ��100�Σ��� 0 �� genePool ���ϳ����������ȡһ����������Ϊ genePool ���ϵ������±꣬������� genePool ��ȡ����������ֵ�� Aindex �� Bindex��ֻҪ������������ȣ���������ѭ��������ѭ�� 100 �δ� genePool �����ȡ����ֵ������ȵġ�֮�������������±��ڵ�ǰ��Ⱥ�����������飩�л�ȡ��Ӧ�ĸ���������������
///         13.4������������������� Child1 �� Child2����ʾ����1�ͺ���2������������������ Initialise ������ʼ����������������󣬲���ʼ���������������Ӧ�� fitness Ϊ 0
///         13.5�������������Ӷ����Ȩ�ؾ��󼯺ϣ���ÿһ��Ȩ�ؾ����У�����1��һ��ĸ��ʽ������� Aindex ���������������Ȩ�ؾ���һ��ĸ������� Bindex ���������������Ȩ�ؾ��󣬺���2�뺢��1��Ӧ�෴������ AIndex �� BIndexֵһ����
///         13.6��ͬ������������Ӷ����ƫ��ֵ���ϲ����ʸ���
///         13.7��������1�ͺ���2���浽 newPopulation �����У�ÿ����һ���� naturallySelected ����1��naturallySelected ����һ����Ⱥ����һ�����������飩�ĸ��������������������������鳤�ȣ�
///         
/// 
/// 
/// 
///     14��void Mutate(NNet[] newPopulation)������ͻ�䷽��
///         14.1��ѭ�� naturallySelected ������naturallySelected ��ʾ�Ѿ���ӵ� newPopulation �����е������������������ naturallySelected �����е�ÿһ�����������
///         14.2��Ƕ��ѭ������ÿһ������������Ȩ�ؾ��󼯺ϣ�������ÿһ��Ȩ�ؾ���
///         14.3�������ȡһ�� 0.0f �� 1.0f ���������������������С�� mutationRate ͻ���ʣ�����Ϊ����Ԫ����ͻ�䣬��ȷ��˵��Ȩ�ؾ�����ͻ�䣬���� MutateMatrix ͻ����󷽷�����ͻ�䡱�þ��󣬽�ͻ���ľ��������ǰ����
///         
/// 
///         
///     15��Matrix<float> MutateMatrix(Matrix<float> A)��ͻ����󷽷�
///         15.1������һ��������������þ�����С�ͻ�䡱����ͻ���ľ��󷵻�
///         15.2���������ͻ��� randomPoints = Random.Range(1,(A.RowCount * A.ColumnCount) / 7);�����ֵ�Ǹ���ʵ�����ȷ����
///         15.3������һ���¾�������������轫Ҫͻ��ľ����ֵ
///         15.4���� randomPoints ��Ϊѭ������ѭ����ÿһ��ѭ����������巢��ͻ�������������������������������������ͻ���Ԫ�أ�Ϊ��ͻ���Ԫ��������Ͻ��� -1 �� 1 ��һ�������������� Mathf.Clamp ��������Ԫ�ص�ֵ������ [-1,1] ���䣬��������ϸ����������С�� -1 �򷵻� -1������ 1 �򷵻� 1��
///         15.5������ͻ���ľ���
///         
/// 
/// 
///     
///     
/// </summary>
public class GeneticManager : MonoBehaviour
{
    #region ��Ա
    [Header("References")]
    public ANNController controller;//��ȡ���˿�����

    [Header("Controls")]
    public int initialPopulation = 200;//��ʼ���˿�����
    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.055f;//ͻ���ʣ�������������Ļ���

    [Header("Crossover Controls")]
    public int bestAgentSelection = 10;//������ѵĵ���
    public int worstAgentSelection = 0;//�������ĵ���
    public int numberToCrossover;//�ӽ�����
    private List<int> genePool = new List<int>();//����أ��⣩������������ѡ������磨������11�����˻��Բ�ͬ������ӵ�����⣩
    private int naturallySelected;//��Ȼѡ��

    private ANN[] population;//�˿ڱ�����ÿһ���˶���һ�������磬��������������·����

    [Header("Public View")]//���ڵ���
    public int currentGeneration;//��ǰ�ڼ���
    public int currentGenome = 0;//��ǰ������
    public int exceptSuccessed = 0;//�ɹ����Ѵ���
    #endregion







    #region ��ʼ��
    /// <summary>
    /// ��ʼ����������Ⱥ
    /// </summary>
    private void Start()
    {
        controller = GameObject.Find("Ann_Enemy_0_AngryPig").GetComponent<ANNController>();
        CreatePopulation();//�����˿�
    }




    /// <summary>
    /// �����˿���Ⱥ
    /// </summary>
    private void CreatePopulation()
    {
        population = new ANN[initialPopulation];//ÿһ���˿ڶ���һ��������
        FillPopulationWithRandomValues(population, 0);//ʹ�����ֵ����˿ڣ�Ȼ��Ὣ��һ��������������ǰ���ˣ�����Ҫ���õ�ǰ������
        ResetToCurrentGenome();//���õ�ǰ������
    }
    #endregion




    #region ʹ������������Ⱥ��ʵ������Ⱥ�������磩��ӵ���Ⱥ�������磩������
    /// <summary>
    /// ʹ�����������˿�
    /// </summary>
    /// <param name="newPopulation">Ҫ�����˿�</param>
    /// <param name="startingIndex">��ʼ�����±�λ��</param>
    private void FillPopulationWithRandomValues(ANN[] newPopulation, int startingIndex)
    {
        while (startingIndex < initialPopulation)
        {
            newPopulation[startingIndex] = new ANN();//Ϊÿ�������������
            newPopulation[startingIndex].Initialise(controller.LAYERS, controller.NEURONS);//��ʼ��ÿ�������磨��ʼ����ӵ�ֵ��������ģ�
            startingIndex++;
        }
    }
    #endregion





    #region ���õ�ǰ������
    /// <summary>
    /// ���õ��������磨�����飩
    /// </summary>
    private void ResetToCurrentGenome()
    {
        controller.ResetWithNetwork(population[currentGenome]);//���õ���������͵��˱���
    }
    #endregion






    #region ��������
    /// <summary>
    /// ����������һ���������������������Ӧ�ȼ������磨����ÿһ����Ӧ�ȣ�����������һ����Ⱥ���壨�����磩
    /// </summary>
    /// <param name="fitness"></param>
    /// <param name="network"></param>
    public void Death(float fitness, ANN network)
    {
        if (currentGenome < population.Length - 1)//��������������±�С�����������鳤��-1��˵������������û�о���ѵ�������������һ�������紫�ݵ� CarController �ű������н���ѵ��
        {
            population[currentGenome].fitness = fitness;//����������ѵ���Ľ������Ӧ�ȱ��浽���񾭽ű��� fitness ������
            currentGenome++;//����������1���������������±�����1
            ResetToCurrentGenome();//����һ�������紫�ݸ� CarController�����õ��˵������磬���� FixedUpdate �����м�������������
        }
        else//����������˵����ǰ����������ȫ��ѵ����ϣ�����һ��ȫ��ѵ����ϣ�������Ⱥ������һ��
        {
            RePopulate();//�����˿�
        }
    }
    #endregion






    #region �����˿ڷ���
    /// <summary>
    /// �����˿�
    /// </summary>
    private void RePopulate()
    {
        genePool.Clear();//��������
        currentGeneration++;//������һ��
        naturallySelected = 0;//������Ȼѡ�� 
        SortPopulation();//���˿ڽ������򣨽�������õĵ�������ǰ�棬����ѡ����õ��˿ڣ�

        ANN[] newPopulation = PickBestPopulation();//��ѡ��õ��˿�
        Crossover(newPopulation);//���深ֳ
        Mutate(newPopulation);//����ͻ��

        FillPopulationWithRandomValues(newPopulation, naturallySelected);//���ֵ����˿�
        population = newPopulation;//�����õ��˿ڸ��赱ǰ�˿�

        currentGenome = 0;//��������Ϊ0
        ResetToCurrentGenome();//���õ�ǰ������
    }
    #endregion






    #region �����㷨�����Ѿ�ѵ����ϵĵ�����Ⱥ�����������飩��������ָ�� fitness �Ӵ�С����
    /// <summary>
    /// ʹ��ð���㷨�����˿�
    /// </summary>
    private void SortPopulation()
    {
        for (int i = 0; i < population.Length; i++)
        {
            for (int j = i; j < population.Length; j++)
            {
                if (population[i].fitness < population[j].fitness)//����i���͵�i��֮���ÿһ���˽��жԱȣ�����Ӧ�ȸߵ���ǰ��
                {
                    ANN temp = population[i];
                    population[i] = population[j];
                    population[j] = temp;
                }
            }
        }
    }
    #endregion






    #region ��ѡ������������õ���Ⱥ(����������)����ѡ���ܱ�����õĸ����ŵ�һ���µ���Ⱥ����(�������������)���в�����
    /// <summary>
    /// ��ѡ������õ��˿�
    /// </summary>
    /// <returns></returns>
    private ANN[] PickBestPopulation()
    {

        ANN[] newPopulation = new ANN[initialPopulation];//����һ���µ���Ⱥ

        //�����ܱ�����õ��˿���Ⱥ��������ӱ�����õ���
        for (int i = 0; i < bestAgentSelection; i++)
        {
            newPopulation[naturallySelected] = population[i].InitialiseCopy(controller.LAYERS, controller.NEURONS);//�������������ز���ӵ����˿ڶ���
            newPopulation[naturallySelected].fitness = 0;//�����˿ڶ������Ӧ������Ϊ0
            naturallySelected++;//��Ȼѡ������

            int f = Mathf.RoundToInt(population[i].fitness * 10);//��Ӧ��Խ�ߵ��ˣ�����10֮��ֵ��Խ��ֵԽ��ѭ��������Խ�࣬����ӵ�������еĴ�����Խ��
            for (int c = 0; c < f; c++)
            {
                genePool.Add(i);
            }

        }

        //�������ܱ��������˿���Ⱥ
        for (int i = 0; i < worstAgentSelection; i++)
        {
            int last = population.Length - 1;//���˿���Ⱥ��β����ʼѡ�����ܱ���������
            last -= i;

            //���ܱ���������Ӧ����ͣ���10��ֵ�����������ֺõ�ҪС�ܶ࣬ѭ������Ҳ��ӦС�ܶ࣬����ӵ�����صĴ���Ҳ��С�ܶ�
            int f = Mathf.RoundToInt(population[last].fitness * 10);
            for (int c = 0; c < f; c++)
            {
                genePool.Add(last);
            }

        }
        return newPopulation;//�����µ��˿���Ⱥ���������������һ����Ⱥ�б�����ú����Ļ����飩
    }
    #endregion







    #region ���深ֳ����,ÿ���深ֳһ�λ��������Ӷ���
    /// <summary>
    /// ����õ��µĺ��ӣ��ӽ���ֳ��
    /// </summary>
    /// <param name="newPopulation"></param>
    private void Crossover(ANN[] newPopulation)
    {
        for (int i = 0; i < numberToCrossover; i += 2)//ѭ���ӽ�����
        {
            //��һ���͵ڶ������з�ֳ
            int AIndex = i;
            int BIndex = i + 1;

            if (genePool.Count >= 1)//ֻ�л�����������ڵ���1����������������ſ��Խ��з�ֳ
            {
                for (int l = 0; l < 100; l++)//ѭ��100�Σ����������ͬ���±�
                {
                    AIndex = genePool[Random.Range(0, genePool.Count)];//�ӻ����������õ�һ���±�
                    BIndex = genePool[Random.Range(0, genePool.Count)];//�ӻ����������õ�һ���±�

                    if (AIndex != BIndex)//����õ��������±겻ͬ
                        break;//����ѭ��
                }
            }

            //���������µĺ��Ӷ���
            ANN Child1 = new ANN();
            ANN Child2 = new ANN();

            //��ʼ���������Ӷ���
            Child1.Initialise(controller.LAYERS, controller.NEURONS);
            Child2.Initialise(controller.LAYERS, controller.NEURONS);

            //��ʼ�����Ӷ������Ӧ��
            Child1.fitness = 0;
            Child2.fitness = 0;

            //�������Ӷ����ÿһ��Ȩ�ز������ֵ
            for (int w = 0; w < Child1.weights.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)//������0��1���������С��0.5
                {
                    Child1.weights[w] = population[AIndex].weights[w];//������Ⱥ�������õ�AIndex�˿ڵ�Ȩ�ظ�����һ������
                    Child2.weights[w] = population[BIndex].weights[w];//������Ⱥ�������õ�BIndex�˿ڵ�Ȩ�ظ����ڶ�������
                }
                else//������������0.5���򽻻�AIndex��BIndex��ֵȨ�ظ���������
                {
                    Child2.weights[w] = population[AIndex].weights[w];
                    Child1.weights[w] = population[BIndex].weights[w];
                }

            }

            //�������Ӷ����ÿһ��ƫ��������ֵ
            for (int w = 0; w < Child1.biases.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    Child1.biases[w] = population[AIndex].biases[w];
                    Child2.biases[w] = population[BIndex].biases[w];
                }
                else
                {
                    Child2.biases[w] = population[AIndex].biases[w];
                    Child1.biases[w] = population[BIndex].biases[w];
                }

            }
            //�����Ӷ��󱣴浽�µ��˿���Ⱥ��
            newPopulation[naturallySelected] = Child1;
            naturallySelected++;

            newPopulation[naturallySelected] = Child2;
            naturallySelected++;

        }
    }
    #endregion







    #region ����ͻ�䷽��
    /// <summary>
    /// ����ͻ�䷽��������ͻ�䣬���Ӳ�ȷ����
    /// </summary>
    /// <param name="newPopulation"></param>
    private void Mutate(ANN[] newPopulation)
    {

        for (int i = 0; i < naturallySelected; i++)//ѭ����Ȼѡ����
        {
            for (int c = 0; c < newPopulation[i].weights.Count; c++)//ѭ��ÿһ���˿ڵ�ÿһ��Ȩ��
            {

                if (Random.Range(0.0f, 1.0f) < mutationRate)//��0��1��������һ��������������ø�����С��ͻ����
                {
                    newPopulation[i].weights[c] = MutateMatrix(newPopulation[i].weights[c]);//����ͻ����󷽷����ͻ��Ȩ�أ������丳����˿ڣ���ʾ���˿ڶ�����ͻ��
                }

            }
        }
    }


    /// <summary>
    /// ͻ����󷽷�����һ�������ֵ���иı�
    /// </summary>
    /// <param name="A">����ͻ��ľ���</param>
    /// <returns>����һ������ͻ��ľ���</returns>
    Matrix<float> MutateMatrix(Matrix<float> A)
    {
        //���ͻ���
        int randomPoints = Random.Range(1, (A.RowCount * A.ColumnCount) / 7);//����ʵ���������ȷ����

        Matrix<float> C = A;//����һ���µľ��󣬲���Ϊͻ��ľ���ֵ����

        //ѭ�����ͻ���
        for (int i = 0; i < randomPoints; i++)
        {
            int randomColumn = Random.Range(0, C.ColumnCount);//���巢��ͻ����������
            int randomRow = Random.Range(0, C.RowCount);//���巢��ͻ����������

            C[randomRow, randomColumn] = Mathf.Clamp(C[randomRow, randomColumn] + Random.Range(-1f, 1f), -1f, 1f);//ͻ��
        }

        return C;

    }
    #endregion


}
