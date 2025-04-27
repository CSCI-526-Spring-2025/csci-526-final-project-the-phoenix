using UnityEngine;

public class VanishingBlock : MonoBehaviour
{
    public GameObject objectToHide1;  
    public GameObject objectToHide2;

    public void Start()
    {
        objectToHide1.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"));
        {
            if (objectToHide1 != null)
                objectToHide1.SetActive(false);

            if (objectToHide2 != null)
                objectToHide2.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  

        if (collision.gameObject.CompareTag("Player"));
        {
            if (objectToHide1 != null)
                objectToHide1.SetActive(false);

            if (objectToHide2 != null)
                objectToHide2.SetActive(false);
        }
    }
}
