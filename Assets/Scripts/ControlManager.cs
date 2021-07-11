using System.Collections;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    private CardModule Select { get; set; } = null; //  선택한 카드

    //  게임이 시작되자마자 한 번만 호출된다
    private void Awake()
    {
        StartCoroutine(UpdateControll());
    }

    private CardModule OnClickCard()
    {
        //  Ray? 레알 빔(레이저 슝-)
        //  특정한 방향으로 레이를 쏴서 다른 객체가 맞았는지 확인하는 용도
        //  ScreenPointToRay (스크린 좌표계 기준의 Vector2 Ray 를 반환한다)
        //  마우스 좌표 기준으로 레이를 만들어 준다
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider == null) return null;
        GameObject go = hit.collider.gameObject;

        return go.GetComponent<CardModule>();
    }

    private IEnumerator UpdateControll()
    {
        while (true)
        {
            yield return null;

            //  상수!
            //  절대 변하지 않는 수
            const float RotateSpeed = .2f;
            if (Input.GetMouseButtonDown(0))
            {
                var newCard = OnClickCard();
                if (newCard == null) continue;    //  잘못된 클릭!
                //  선택한 카드가 있고, 선택한 카드를 다시 선택하였을 경우
                else if (Select != null && newCard.gameObject == Select.gameObject)
                {
                    //  선택 취소 시켜준다 (카드 다시 원래 방향으로 돌려주고, 선택한 객체 null)
                    Select.StartRotate(false, RotateSpeed);
                    Select = null;
                }
                else
                {
                    //  카드 돌릴동안 기다려준다
                    newCard.StartRotate(true, RotateSpeed);
                    yield return new WaitForSeconds(RotateSpeed);

                    //  선택한 카드가 없다면 새로 선택한 카드를 Select 로 변경
                    if (Select == null) Select = newCard;
                    //  만약 새로 선택한 카드가 기존 선택 카드의 컬러와 같다면 매칭!
                    else if (newCard.CardColor == Select.CardColor)
                    {
                        Select.StartDisableCard(RotateSpeed);
                        newCard.StartDisableCard(RotateSpeed);
                        Select = null;

                        // 하이어라키 안에 있는 모든 객체에서 먼저 발견 된다
                        // ScoreManager component 를 찾는다!
                        var manager = FindObjectOfType<ScoreManager>();
                        manager.AddScore(100);

                        //var gameManager = FindObjectOfType<GameManager>();
                        //if(gameManager.CheckAllDisableCard())
                        //{
                        //    var timer = FindObjectOfType<Timer>();
                        //    timer.StopTimer();
                        //}
                    }
                    //  다르면 매칭 실패!
                    else
                    {
                        Select.StartRotate(false, RotateSpeed);
                        newCard.StartRotate(false, RotateSpeed);
                        Select = null;
                    }
                }
            }

        }
    }

}
