using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CardModule[] m_Cards = null;
    private bool m_IsStart = false;

    private void Awake()
    {
        //  Array? 배열
        //  여러 객체를 담을 수 있다! 단,
        //  1. 처음 담을 양을 지정해주어야 한다(바구니 사이즈를 미리 말해야한다)
        //  ex) CardModule[] m_Cards = new CardModule[10]; <- 이러면 칸이 10개인 배열
        //  2. 중간 원소 (임의의 원소) 를 선택할 수 있다
        //  ex) CardModule[0] -> 하면 배열의 0번째 객체를 불러올 수 있다!
        //  3. 배열의 맨 마지막 요소(element) 는 절대 건들지 마라!
        //  ex) 내가 처음에 10 개(new CardModule[10]) 이라고 했을 경우
        //  ->  CardModule[10] 이렇게 사용하는건 불가능하다! (에러난다!) 0 ~ 9 까지 사용할 수 있다
        //  ->  CardModule[10].Length 를 호출하면 크기를 10으로 주니 주의할 것!

        //  주석 ?            주석 푸는 방법 ?
        //  Ctrl + K, C       Ctrl + K, U
        //int length = transform.childCount;  //  이 객체의 자식 개수를 구한다
        //m_Cards = new CardModule[length];   //  자식 갯수만큼 배열 크기를 만들어준다
        //for (int i = 0; i < m_Cards.Length; ++i)
        //{
        //    //  자식의 객체를 가져와서 CardModule 스크립트를 배열 안에 넣어준다
        //    m_Cards[i] = transform.GetChild(i).GetComponent<CardModule>();
        //}

        //  내 객체 기준으로 자식들을 검사하여 해당 컴포넌트를 가져와 배열로 만들어준다
        m_Cards = transform.GetComponentsInChildren<CardModule>();

        OnGameStart();
    }

    public void OnGameStart()
    {
        //  List ? (C++ Vector 랑 비슷)
        //  배열 (바구니) 이긴 한데, 크기의 제한이 없음. 그리고 중간에 늘릴 수 있음. 자유로움
        List<Color> colors = new List<Color>();
        int length = m_Cards.Length / 2;

        SetRandomColor(colors, length); //  랜덤 컬러 생성. 단 카드 총 개수의 절반만 생성
        colors.AddRange(colors);        //  절반 생성한 컬러 값들을 똑같이 복붙해서 리스트에 넣어준다

        Shuffle(colors, colors.Count);  //  컬러 섞섞
        for (int i = 0; i < colors.Count; ++i)  //  섞은 컬러값 들을 카드한테 차례대로 넣어준다
        {
            m_Cards[i].CardColor = colors[i];
            m_Cards[i].gameObject.SetActive(true);
        }

        m_IsStart = true;
    }

    //  컬러 셋팅!
    private void SetRandomColor(List<Color> list, int count)
    {
        if (count == 0) return;
        Color color = new Color
        (
            Random.Range(0f, 1f),   //  r
            Random.Range(0f, 1f),   //  g
            Random.Range(0f, 1f),   //  b
            1                       //  a
        );

        //  재귀 함수
        //  lambda 검사식
        //  list.Exists(x <- list 의 원소를 순차적으로 하나 씩 넣어준다
        //  for (int i = 0; i < list.Count; ++i) { var x = list[i]; }
        if (list.Exists(x => x.Equals(color))) SetRandomColor(list, count);
        else
        {
            list.Add(color);
            SetRandomColor(list, count - 1);
        }
    }

    //  카드를 섞어주는 함수
    //  Fisher-Yates shuffle
    //  https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    private void Shuffle(List<Color> list, int count)
    {
        while (count-- > 1)
        {
            int i = Random.Range(0, count);

            //  Swap
            //  Temp 한테 A 값을 임시 저장
            //  A = B 값을 넣어준다
            //  B = 임시저장한 Temp (A) 값을 넣어준다
            Color temp = list[i];
            list[i] = list[count];
            list[count] = temp;
        }
    }

    //public bool CheckAllDisableCard()
    //{
    //    for (int i = 0; i < m_Cards.Length; ++i)
    //    {
    //        if (m_Cards[i].gameObject.activeSelf)
    //            return false;
    //    }
    //    return true;
    //}

}
