using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModule : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_FrontCardRenderer;    //  카드 앞면 렌더러
    [SerializeField] private SpriteRenderer m_BackCardRenderer;     //  카드 뒷면 렌더러
    [SerializeField] private AudioSource m_AudioSource;             //  카드 클릭 사운드

    //  get set?
    //  getter, setter -> 접근자, 설정자
    //  C, C++ 에는 없었으나 (C++17 이상에서는 있음)
    //  C# 에서는 쉽게 구현할 수 있도록 만듬
    //  public bool Active { get; private set; }
    //  -> 누구나 값을 읽을 수 있지만, 값을 변경하는 건 이 클래스 안에서만 할 수 있다
    private bool Active { get; set; } = true;   //  카드가 살아 있니?

    //  get set 도 실제론 함수 이다!
    public Color CardColor
    {
        get => m_BackCardRenderer.color;
        set
        {
            m_BackCardRenderer.color = value;
        }
    }
    //  만약 get, set 이 없다면 ..
    //  public Color GetCardColor()
    //  { return m_BackCardRenderer.color}
    //  public Color SetCardColor(Color color)
    //  { m_BackCardRenderer.color = color; }

    /// <summary>
    /// 카드 회전 코루틴
    /// </summary>
    /// <param name="isUp">카드의 회전 방향</param>
    /// <param name="time">카드의 회전 시간</param>
    /// <returns></returns>
    private IEnumerator UpdateRotate(bool isUp, float time)
    {
        float currentTime = Time.time;  //  현재 시간을 저장
        //  ? -> 삼항 연산자
        //  조건 ? true : false;
        Quaternion prev = isUp ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        Quaternion next = isUp ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        while (Time.time - currentTime <= time)
        {
            //  선형 보간법
            transform.rotation = Quaternion.Lerp(
                prev,
                next,
                (Time.time - currentTime) / time);  //  시간에 대한 백분률 구하기

            //  와일문 안에 안넣으면 유니티 멈춤!!!!!!!!
            yield return null;  //  한 프레임 기다려라!
        }

        //  보간을 한다 해서 100% 완벽히 next 값(다 돌아간 값)을 확정할 수 없으므로
        //  지정한 시간 (time) 이 끝났다면 카드의 회전 값을 고정해준다
        transform.rotation = next;
        yield break;
    }
    public void StartRotate(bool isUp, float time)
    {
        if (!Active) return;    //  이미 업데이트 중 이라면 하지마!
        else if (!gameObject.activeSelf) return;    //  비활성화 되었으면 하지마!

        StartCoroutine(UpdateRotate(isUp, time));
        m_AudioSource.Play();
    }

    //   카드를 사라지게 하는 마법의 함수
    private IEnumerator UpdateOpacity(float time)
    {
        //  실습 과제 1
        //  카드 알파 값을 0으로 점점 바뀌는 보간법 만들기

        float currentTime = Time.time;  //  현재 시간을 저장한다

        //  Mathf.Lerp(시작값, 끝값, 시간에 대한 백분률)
        //  알파값을 이용하여 카드를 점점 사라지게 한다
        while (Time.time - currentTime <= time)
        {
            CardColor = new Color(1, 1, 1,
                Mathf.Lerp(1, 0, (Time.time - currentTime) / time));

            yield return null;
        }

        CardColor = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);    //  오브젝트 비활성화

        yield break;
    }
    public void StartDisableCard(float time)
    {
        if (!Active) return;

        StartCoroutine(UpdateOpacity(time));
    }

    //  접근 제한자 ?
    //  private, protected, public
    //  부류          |   public  |   protected   |   private
    //  접근 가능     |      O     |   자식(O) X   |      X

}
