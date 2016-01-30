using UnityEngine;
using System.Collections;

public class EnemySinWave : MonoBehaviour {

    Vector3 v3Axis = new Vector3(1.0f, 1.0f, 0.0f);

    // Use this for initialization
    void Start () {
        v3Axis.Normalize();
    }

    // Update is called once per frame
    void Update () {
         transform.localPosition = v3Axis * Mathf.Sin(Time.time) * 2f;

    }
}
