using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace AstroMath
{
    public class HologramController : MonoBehaviour
    {
        #region Status
        [Header("Status")]
        [SerializeField] bool isOn; //whether or not the hologram is turned ON or OFF
        [SerializeField] bool isMoving; //whether or not the hologram is "moving", i.e. changing from activation state to another
        #endregion

        #region Shader Controller
        [Header("Shader Controller")]
        [SerializeField] MeshRenderer hologramMeshRenderer;
        Material hologramMaterial; //the material of the hologram sphere
        [SerializeField] Transform startUpController; //used for controlling the shader behind the material
        //> the start and end point of the 'startUpController'
        [SerializeField] Transform startPointTF; //TF = Transform
        [SerializeField] Transform endPointTF;
        #endregion

        #region Generators
        [Header("Generators")]
        [Tooltip("The point that the generators will point towards when the hologram is on.")]
        [SerializeField] Transform onLookAtPointTF;
        [Tooltip("The point that the generators will point towards when the hologram is off.")]
        [SerializeField] Transform offLookAtPointTF;
        Vector3[] onRotations; //
        Vector3[] offRotations; //
        Vector3[] rotationDistances; //
        Vector3[] rotationSpeeds; //
        [SerializeField] Transform[] generatorsTF;
        [SerializeField] ParticleSystem[] particleSystems;
        #endregion

        #region Time, Distance and Speed
        [Header("Time, Distance and Speed")]
        [Tooltip("The amount of time in seconds that the opening/closing of the hologram should last.")]
        [SerializeField] float timeToChange;
        float distanceToTravel; //the distance between 'startPointTF' and 'endPointTF'
        float travelSpeed; //the speed at which the 'startUpController' will travel
        #endregion

        void Start()
        {
            hologramMaterial = hologramMeshRenderer.material; //the material of the hologram
            hologramMaterial.SetVector("_Startup", startUpController.position); //the initial value of the "_Startup" parameter is set

            #region Setting Generator Rotation Parameters
            onRotations       = new Vector3[generatorsTF.Length];
            offRotations      = new Vector3[generatorsTF.Length];
            rotationDistances = new Vector3[generatorsTF.Length];
            rotationSpeeds    = new Vector3[generatorsTF.Length];

            for (int i = 0; i < generatorsTF.Length; i++)
            {
                var direction = onLookAtPointTF.position - generatorsTF[i].position;
                onRotations[i] = Quaternion.LookRotation(direction).eulerAngles - new Vector3(360f, 0f, 0f); //for some reason, the 'LookRotation()-method' goes the long way around the circle, and so I have to add 360 degrees -_-

                direction = offLookAtPointTF.position - generatorsTF[i].position;
                offRotations[i] = Quaternion.LookRotation(direction).eulerAngles; //I don't have to do it on this on for some reason?? O.o
                //generatorsTF[i].rotation = Quaternion.Euler(offRotations[i]);

                rotationDistances[i] = onRotations[i] - offRotations[i];
                rotationDistances[i] = new Vector3(Mathf.Abs(rotationDistances[i].x),
                                                   Mathf.Abs(rotationDistances[i].x),
                                                   Mathf.Abs(rotationDistances[i].x));

                rotationSpeeds[i].x = rotationDistances[i].x / timeToChange;
                rotationSpeeds[i].y = 0f;
                rotationSpeeds[i].z = 0f;
            }

            //Quaternion rotationNeeded = Quaternion.LookRotation(onRotations[0]);
            //Quaternion currentRotation = generatorsTF[0].rotation;
            //Quaternion rotationDelta = Quaternion.Inverse(rotationNeeded) * currentRotation;
            //Debug.Log($"Rotation Delta (Euler Angles): {rotationDelta.eulerAngles}");
            #endregion

            //> calculate distance and travel speed of the 'controller point' of the hologram
            distanceToTravel = Vector3.Distance(startPointTF.position, endPointTF.position); 
            travelSpeed = distanceToTravel / timeToChange;
            Toggle();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) //if the <T> key is pressed...
            {
                Toggle(); //we toggle the activation of the hologram
            }

            if (isMoving == true) //if the hologram should be moving...
            {
                //we create this 'sign' controlling whether the hologram should move down or up
                //isOn = true  -> sign = -1, the hologram will move DOWN
                //isOn = false -> sign =  1, the hologram will move UP
                var sign = -1; if (isOn == false) sign = 1;

                //we change the position of the controller point that controls the '_Startup' parameter of the shader of the material
                startUpController.position += new Vector3(0, travelSpeed * sign * Time.deltaTime, 0);
                hologramMaterial.SetVector("_Startup", startUpController.position);

                //we change the rotation of the generators
                for (int i = 0; i < generatorsTF.Length; i++)
                {
                    generatorsTF[i].Rotate(rotationSpeeds[i] * sign * Time.deltaTime);
                }
            }
        }

        public void Toggle()
        {
            SetActivation(!isOn); //we flip the activation to the opposite state (true->false, false->true)
        }

        public void TurnOn()
        {
            SetActivation(true); //we set the activation to true
        }

        public void TurnOff()
        {
            SetActivation(false); //we set the activation to false
        }

        void SetActivation(bool activationState)
        {
            if(isMoving == true) //if the hologram is currently "moving", i.e. changing graphically...
            {
                //we log a warning, telling the user
                Debug.LogWarning("Can't change activation now! Hologram is still moving!");
            }
            else //else, if the hologram is NOT moving...
            {
                isOn = activationState; //we set the activation accordingly
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (isOn == true) particleSystems[i].Play();
                    else              particleSystems[i].Stop();
                }
                StartCoroutine(WaitWhileChangingHologram()); //we start a coroutine, which will run simultaneously with the Update()-method
            }
        }

        IEnumerator WaitWhileChangingHologram()
        {
            isMoving = true; //we tell the hologram to start changing
            yield return new WaitForSeconds(timeToChange); //we wait for the specified amount of time
            isMoving = false; //then we tell the hologram to stop
        }
    }
}