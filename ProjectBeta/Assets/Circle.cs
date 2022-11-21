using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRend;
    private int _segments;

    private void Update()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (_segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 5f;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 5f;

            _lineRend.SetPosition(0, new Vector3(x, y, 0));

            angle += (360f / _segments);
        }
    }
}
