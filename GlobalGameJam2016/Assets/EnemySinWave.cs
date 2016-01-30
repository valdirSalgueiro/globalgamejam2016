using UnityEngine;
using System.Collections;

public class EnemySinWave : MonoBehaviour
{

    float speed = 3f;
    float m_degrees;
    float m_speed = 1.0f;
    float m_amplitude = 0.05f;
    float m_period = 1.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // Update degrees
        float degreesPerSecond = 360.0f / m_period;
        m_degrees = Mathf.Repeat(m_degrees + (Time.deltaTime * degreesPerSecond), 360.0f);
        float radians = m_degrees * Mathf.Deg2Rad;

        // Offset by sin wave
        Vector3 offset = new Vector3(m_amplitude * Mathf.Sin(radians), m_amplitude * Mathf.Sin(radians), 0.0f);
        transform.position += offset;
    }


}


