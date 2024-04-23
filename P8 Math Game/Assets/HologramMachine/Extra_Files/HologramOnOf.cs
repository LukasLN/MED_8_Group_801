using System.Collections;
using UnityEngine;

public class HologramOnOf : MonoBehaviour
{
    [SerializeField] bool t_pressed = false;
    [SerializeField] bool can_press_t_again = true;

    [SerializeField] Material HologramMat;
    [SerializeField] Transform StartupController;

  
    void Start()
    {
        StartupController.localPosition = new Vector3(0 ,12, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && can_press_t_again == true)
        { 
            can_press_t_again = false;
            StartCoroutine(T_Timer());
        }

        if (t_pressed == true)
        { 
            StartupController.position = StartupController.position + new Vector3(0 ,-2 * Time.deltaTime, 0);
        }

         HologramMat.SetVector("_Startup",StartupController.position);
     
    }

    IEnumerator T_Timer()
    {
        yield return new WaitForSeconds(3);
            t_pressed = true;
           
        yield return new WaitForSeconds(10);
            t_pressed = false;
            can_press_t_again = true;
    }

}
