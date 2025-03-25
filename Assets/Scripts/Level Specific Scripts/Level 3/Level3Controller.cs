using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Controller : MonoBehaviour
{
    [SerializeField] private SquareRotator room2;
    [SerializeField] private SquareRotator room3;
    [SerializeField] private SquareRotator room4;

    [SerializeField] private GameObject room4Portal;

    [SerializeField] private GameObject square2;
    [SerializeField] private GameObject square3;
    [SerializeField] private GameObject square4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Wedge 1-2
        if (room2.currentAngle == -180f)
        {
            square2.SetActive(true);

        }
        else
        {
            square2.SetActive(false);
        }

        // Wedge 2-3
        if (room4.currentAngle == -90 && room2.currentAngle == -90)
        {
            square3.SetActive(true);
        }
        else
        {
            square3.SetActive(false);
        }

        // Wedge 3-4
        if (room3.currentAngle == 0 && room4.currentAngle == 0)
        {
            square4.SetActive(true);
        }
        else
        {
            square4.SetActive(false);
        }
    }
}
