using UnityEngine;

namespace AstroMath
{
    public class InfoPanelMovement : MonoBehaviour
    {
        public MathProblem mathProblem;
        public int problemID;
        public Vector3 FollowOffset;
        [HideInInspector] public Transform player;
        public bool isActive;

        #region Scaling Problem
        [Header("Scaling Problem")]
        [SerializeField] Transform endPointTF; //TF = Transform
        public bool isForScaling;
        #endregion

        private void Start()
        {
            player = FindObjectOfType<OVRCameraRig>().centerEyeAnchor;
        }

        private void Update()
        {
            if(isActive == true)
            {
                LookAtPlayer();
            }

            //if (mathProblem.m_type == MathProblem.Type.Scale)
            //{
            //    FollowPlayer();
            //}
        }

        public void SetEndPointAsParent()
        {
            transform.SetParent(endPointTF);
        }

        void FollowPlayer()
        {
            transform.position = player.transform.position + FollowOffset;
        }

        void LookAtPlayer()
        {
            transform.LookAt(player, Vector3.up);

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