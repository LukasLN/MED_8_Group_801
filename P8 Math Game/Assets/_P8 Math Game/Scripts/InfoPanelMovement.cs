using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AstroMath
{
    public class InfoPanelMovement : MonoBehaviour
    {
        public MathProblem mathProblem;
        public int problemID;
        public Vector3 FollowOffset;
        [HideInInspector] public Transform player;
        public bool isActive;


        private void Update()
        {
            if(isActive == true)
            {
                LookAtPlayer();
            }

            if (mathProblem.type == MathProblem.Type.Scale)
            {
                FollowPlayer();
            }


        }

        void FollowPlayer()
        {
            transform.position = player.transform.position + FollowOffset;
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