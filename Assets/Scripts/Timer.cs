using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] private Text TimerText;

    private float m_TotalTime = 0;
    private void Update()
    {
        m_TotalTime += Time.deltaTime;
        // TimerText.text = "Time\n + Time.deltaTime;"
        TimerText.text = $"Time\n{m_TotalTime.ToString("F2")}"; // dynamic string, 보관된 문자열


    }

    public void StopTimer()
    {
        enabled = false;
        TimerText.text = $"win!\n{m_TotalTime}";
    }

}
