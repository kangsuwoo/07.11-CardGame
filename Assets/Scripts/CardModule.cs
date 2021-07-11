using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModule : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_FrontCardRenderer;    //  ī�� �ո� ������
    [SerializeField] private SpriteRenderer m_BackCardRenderer;     //  ī�� �޸� ������
    [SerializeField] private AudioSource m_AudioSource;             //  ī�� Ŭ�� ����

    //  get set?
    //  getter, setter -> ������, ������
    //  C, C++ ���� �������� (C++17 �̻󿡼��� ����)
    //  C# ������ ���� ������ �� �ֵ��� ����
    //  public bool Active { get; private set; }
    //  -> ������ ���� ���� �� ������, ���� �����ϴ� �� �� Ŭ���� �ȿ����� �� �� �ִ�
    private bool Active { get; set; } = true;   //  ī�尡 ��� �ִ�?

    //  get set �� ������ �Լ� �̴�!
    public Color CardColor
    {
        get => m_BackCardRenderer.color;
        set
        {
            m_BackCardRenderer.color = value;
        }
    }
    //  ���� get, set �� ���ٸ� ..
    //  public Color GetCardColor()
    //  { return m_BackCardRenderer.color}
    //  public Color SetCardColor(Color color)
    //  { m_BackCardRenderer.color = color; }

    /// <summary>
    /// ī�� ȸ�� �ڷ�ƾ
    /// </summary>
    /// <param name="isUp">ī���� ȸ�� ����</param>
    /// <param name="time">ī���� ȸ�� �ð�</param>
    /// <returns></returns>
    private IEnumerator UpdateRotate(bool isUp, float time)
    {
        float currentTime = Time.time;  //  ���� �ð��� ����
        //  ? -> ���� ������
        //  ���� ? true : false;
        Quaternion prev = isUp ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        Quaternion next = isUp ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        while (Time.time - currentTime <= time)
        {
            //  ���� ������
            transform.rotation = Quaternion.Lerp(
                prev,
                next,
                (Time.time - currentTime) / time);  //  �ð��� ���� ��з� ���ϱ�

            //  ���Ϲ� �ȿ� �ȳ����� ����Ƽ ����!!!!!!!!
            yield return null;  //  �� ������ ��ٷ���!
        }

        //  ������ �Ѵ� �ؼ� 100% �Ϻ��� next ��(�� ���ư� ��)�� Ȯ���� �� �����Ƿ�
        //  ������ �ð� (time) �� �����ٸ� ī���� ȸ�� ���� �������ش�
        transform.rotation = next;
        yield break;
    }
    public void StartRotate(bool isUp, float time)
    {
        if (!Active) return;    //  �̹� ������Ʈ �� �̶�� ������!
        else if (!gameObject.activeSelf) return;    //  ��Ȱ��ȭ �Ǿ����� ������!

        StartCoroutine(UpdateRotate(isUp, time));
        m_AudioSource.Play();
    }

    //   ī�带 ������� �ϴ� ������ �Լ�
    private IEnumerator UpdateOpacity(float time)
    {
        //  �ǽ� ���� 1
        //  ī�� ���� ���� 0���� ���� �ٲ�� ������ �����

        float currentTime = Time.time;  //  ���� �ð��� �����Ѵ�

        //  Mathf.Lerp(���۰�, ����, �ð��� ���� ��з�)
        //  ���İ��� �̿��Ͽ� ī�带 ���� ������� �Ѵ�
        while (Time.time - currentTime <= time)
        {
            CardColor = new Color(1, 1, 1,
                Mathf.Lerp(1, 0, (Time.time - currentTime) / time));

            yield return null;
        }

        CardColor = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);    //  ������Ʈ ��Ȱ��ȭ

        yield break;
    }
    public void StartDisableCard(float time)
    {
        if (!Active) return;

        StartCoroutine(UpdateOpacity(time));
    }

    //  ���� ������ ?
    //  private, protected, public
    //  �η�          |   public  |   protected   |   private
    //  ���� ����     |      O     |   �ڽ�(O) X   |      X

}
