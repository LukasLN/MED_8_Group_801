using UnityEngine;

namespace AstroMath
{
    public class VolumetricPointer : MonoBehaviour
    {
        [SerializeField] InteractableSpaceship spaceship;
        AudioPlayer audioPlayer;

        void Start()
        {
            audioPlayer = GetComponent<AudioPlayer>();

        }

            private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Entered " + other.gameObject.tag);
            if (other.gameObject.tag == "Parking" ||
                other.gameObject.tag == "Asteroid")
            {
                if (spaceship.targetGO == null)
                {
                    spaceship.AddNewTarget(other.gameObject);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //Debug.Log("Stayed " + other.gameObject.tag);
            if (other.gameObject.tag == "Parking" ||
                other.gameObject.tag == "Asteroid")
            {
                audioPlayer.PlaySoundEffect("targetLocked");
                if (spaceship.targetGO == null)
                {
                    spaceship.AddNewTarget(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("Exited " + other.gameObject.tag);
            if (other.gameObject.tag == "Parking" ||
                other.gameObject.tag == "Asteroid")
            {
                spaceship.RemoveOldTarget();
            }
        }
    }
}