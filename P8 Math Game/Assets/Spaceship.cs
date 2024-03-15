using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public int[] Cargotype = { 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(GeneratePosCoordinate(50,80), GeneratePosCoordinate(50, 80), GeneratePosCoordinate(50, 80));

        if (Random.Range(0, 2) < 0)
        {
            Cargotype[0] = Random.Range(0, 4);
        }
        if (Random.Range(0, 2) < 0)
        {
            Cargotype[1] = Random.Range(0, 4);
        }
        if (Random.Range(0, 2) < 0)
        {
            Cargotype[2] = Random.Range(0, 4);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// used for generating ship positions range, usually 50-80
    /// </summary>
    /// <param name="min">
    /// lower boundary of range for random int
    /// </param>
    /// <param name="max">
    /// higher boundary of range for random int
    /// </param>
    /// <returns>
    /// an int within range
    /// </returns>
    public int GeneratePosCoordinate(int min, int max)
    {
        int result;
        int roll = Random.Range(min, max);
        if (Random.Range(0, 2) > 0)
        {
            result = roll * -1;
        }
        else 
        {
            result = roll;
        }

        return result;
    }
}
