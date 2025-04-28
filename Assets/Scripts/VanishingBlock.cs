using UnityEngine;

public class VanishingBlock : MonoBehaviour
{
    public GameObject objectToHide1;  
    public GameObject objectToHide2;

    public bool isHold = false;

    public void Start()
    {
        objectToHide1.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Clone"))
        {
            if (objectToHide1 != null)
                objectToHide1.SetActive(false);

            if (objectToHide2 != null)
                objectToHide2.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Clone"))
        {
            if (objectToHide1 != null)
                objectToHide1.SetActive(false);

            if (objectToHide2 != null)
                objectToHide2.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isHold){
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Clone"))
            {
                if (objectToHide1 != null)
                    objectToHide1.SetActive(true);

                if (objectToHide2 != null)
                    objectToHide2.SetActive(true);
            }
            else
            {
                if (objectToHide1 != null)
                    objectToHide1.SetActive(true);

                if (objectToHide2 != null)
                    objectToHide2.SetActive(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isHold){
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Clone"))
            {
                if (objectToHide1 != null)
                    objectToHide1.SetActive(true);

                if (objectToHide2 != null)
                    objectToHide2.SetActive(true);
            }
            else
            {
                if (objectToHide1 != null)
                    objectToHide1.SetActive(true);

                if (objectToHide2 != null)
                    objectToHide2.SetActive(true);
            }
        }
    }

}
