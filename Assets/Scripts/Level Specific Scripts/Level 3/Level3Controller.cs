using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Controller : MonoBehaviour
{
    [SerializeField] private SquareRotator room2;
    [SerializeField] private SquareRotator room3;
    [SerializeField] private SquareRotator room4;

    [SerializeField] private GameObject room4Portal;

    [SerializeField] private GameObject wedge1_2;
    [SerializeField] private GameObject wedge2_3;
    [SerializeField] private GameObject wedge3_4;
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
        if (room2.currentAngle == 90f)
        {
            wedge1_2.SetActive(false);
            square2.SetActive(true);

        }
        else
        {
            wedge1_2.SetActive(true);
            square2.SetActive(false);
        }

        // Wedge 2-3
        if (room2.currentAngle == 0f && room4.currentAngle == 270f)
        {
            wedge2_3.SetActive(false);
            square3.SetActive(true);
        }
        else
        {
            wedge2_3.SetActive(true);
            square3.SetActive(false);
        }

        // Wedge 3-4
        if (room3.currentAngle == 0f && room4.currentAngle == 180f)
        {
            wedge3_4.SetActive(false);
            square4.SetActive(true);
        }
        else
        {
            wedge3_4.SetActive(true);
            square4.SetActive(false);
        }
    }
}
