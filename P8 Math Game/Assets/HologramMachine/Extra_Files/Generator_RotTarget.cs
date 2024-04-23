using System.Collections;
using UnityEngine;

public class Generator_RotTarget : MonoBehaviour
{
    [SerializeField] bool t_pressed = false;
    [SerializeField] bool can_press_t_again = true;

    [SerializeField] GameObject[] particlesystems;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && can_press_t_again == true)
        { 
            t_pressed = true;
            can_press_t_again = false;
            StartCoroutine(T_Timer());
        }


        if(transform.eulerAngles.z < 128f && t_pressed)
        {
            transform.Rotate(Vector3.forward * 15 * Time.deltaTime);
        }
    }

    IEnumerator T_Timer()
    {
        yield return new WaitForSeconds(3);
            t_pressed = false;
            can_press_t_again = true;
            particlesystems[0].SetActive(true);
            particlesystems[1].SetActive(true);
            particlesystems[2].SetActive(true);
            particlesystems[3].SetActive(true);
    }

}
