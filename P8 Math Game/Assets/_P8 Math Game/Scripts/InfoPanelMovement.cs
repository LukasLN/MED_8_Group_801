using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AstroMath
{
    public class InfoPanelMovement : MonoBehaviour
    {
        [HideInInspector] public Transform player;
        public bool isActive;

        private void Update()
        {
            if(isActive == true)
            {
                LookAtPlayer();
            }
        }

        void LookAtPlayer()
        {
            //transform.LookAt(player, Vector3.up);

            Vector3 direction = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            transform.rotation = lookRotation;
        }

        public void SetActivation(bool newBool)
        {
            isActive = newBool;
        }
    }
}