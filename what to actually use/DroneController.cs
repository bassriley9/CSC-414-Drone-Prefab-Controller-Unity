using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets_1_1_2.CrossPlatformInput;
using UnityStandardAssets_1_1_2.Characters.ThirdPerson;

namespace SIGVerse.Drone
{
	public class DroneController : MonoBehaviour
	{

		public Rigidbody Drone;



		public int DirectionalSpeed = 25;
		public int Tilt = 25;
		public int rotationSpeed = 5;
		public int LiftSpeed = 14;
		private bool moving;
		public bool landing;


		private Vector3 DroneRotation;

        private void Start()
        {

        }

        void Update()
		{

		}
		//rather than using update which updates every frame, fixed update runs at a fixed interval
		//this means taht it can update multiple times per frame
		private void FixedUpdate()
		{
			DroneRotation = Drone.transform.localEulerAngles;//provides angle relative to parent transforms rotation

			TiltCorrection();

			DroneControls();
	

		}
		void TiltCorrection()
        {
			#region tilt correction
			//if tilt too big(stabilizes drone on z-axis)
			if (DroneRotation.z > 10 && DroneRotation.z <= 180)
			{
				Drone.AddRelativeTorque(0, 0, -10);
			}
			if (DroneRotation.z > 180 && DroneRotation.z <= 350)
			{
				Drone.AddRelativeTorque(0, 0, 10);
			}

			//if tilt too small(stabilizes drone on z-axis)
			if (DroneRotation.z > 1 && DroneRotation.z <= 10)
			{
				Drone.AddRelativeTorque(0, 0, -3);
			}
			if (DroneRotation.z > 350 && DroneRotation.z <= 359)
			{
				Drone.AddRelativeTorque(0, 0, 3);
			}

			if (!moving)
			{
				//tilt drone left
				if (Input.GetKey(KeyCode.A))
				{
					Drone.AddRelativeTorque(0, Tilt / -1, 0);
				}
				//tilt drone right
				if (Input.GetKey(KeyCode.D))
				{
					Drone.AddRelativeTorque(0, Tilt, 0);
				}
			}

			//if tilt too big(stabilizes drone on x-axis)
			if (DroneRotation.x > 10 && DroneRotation.x <= 180)
			{
				Drone.AddRelativeTorque(-10, 0, 0);
			}
			if (DroneRotation.x > 180 && DroneRotation.x <= 350)
			{
				Drone.AddRelativeTorque(10, 0, 0);
			}

			//if tilt too small(stabilizes drone on x-axis)
			if (DroneRotation.x > 1 && DroneRotation.x <= 10)
			{
				Drone.AddRelativeTorque(-3, 0, 0);
			}
			if (DroneRotation.x > 350 && DroneRotation.x <= 359)
			{
				Drone.AddRelativeTorque(3, 0, 0);
			}


			#endregion
		}

		void DroneControls()
        {
			landing = false;
            #region drone controls
            if (!landing)
            {
				Drone.AddForce(0, 7.5f, 0);//keeps the drone from losing height quickly, since the drone is set to a rigidbody itll fall at 9.8 until it hits a collider
										   //9.8f(f makes it a float rather than a double
				if (moving == false)
				{

					//forward
					if (Input.GetKey(KeyCode.W))
					{
						Drone.AddRelativeForce(0, 0, DirectionalSpeed);
						Drone.AddRelativeTorque(10, 0, 0);
					}

					//backward
					if (Input.GetKey(KeyCode.S))
					{
						Drone.AddRelativeForce(0, 0, -DirectionalSpeed);
						Drone.AddRelativeTorque(-10, 0, 0);

					}
					//left
					if (Input.GetKey(KeyCode.A))
					{
						Drone.AddRelativeForce(-DirectionalSpeed, 0, 0);
						Drone.AddRelativeTorque(0, 0, 10);

					}
					//right
					if (Input.GetKey(KeyCode.D))
					{
						Drone.AddRelativeForce(DirectionalSpeed, 0, 0);
						Drone.AddRelativeTorque(0, 0, -10);

					}

					//lift
					if (Input.GetKey(KeyCode.UpArrow))
					{
						Drone.AddRelativeForce(0, LiftSpeed, 0);

					}
					//drop
					if (Input.GetKey(KeyCode.DownArrow))
					{
						Drone.AddRelativeForce(0, -LiftSpeed, 0);

					}
					//rotate left
					if (Input.GetKey(KeyCode.LeftArrow))
					{
						transform.Rotate(0f, -rotationSpeed, 0f, Space.Self);

					}
					//rotate right
					if (Input.GetKey(KeyCode.RightArrow))
					{
						transform.Rotate(0f, rotationSpeed, 0f, Space.Self);

					}

					//call land funciton
					if (Input.GetKey(KeyCode.Space))
					{
						//call land and set land bool to true so that you just descend from the selected location
						land();
					}
				}
			
			}

			#endregion
		}
        #region landing
		void land()
        {
			landing = true;

			//drop the drone at a slowish speed
			Drone.AddForce(0, -15, 0);

			//stop landing
			if (Input.GetKey(KeyCode.Space))
			{
				landing = false;
			}
			
		}

        #endregion
    }
}
