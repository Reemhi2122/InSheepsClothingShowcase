using InSheepsClothing.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using VRTools.Interaction;

namespace InSheepsClothing.Interaction
{
	public class Camera : GrabbableObject
    {
        private Animator animator = null;

		private AudioSource audioSource = null;

        private bool canFlash = true;

		[SerializeField]
		private AudioClip beep = null, shutter = null;

		private bool pressed = false, focused = false;

        public void EjectPicture()
        {
            for (int i = 0; i < pictureSpawnpoint.transform.childCount; i++)
				pictureSpawnpoint.transform.GetChild(i).GetComponent<TakenPicture>().EnablePhysics();
        }

        private UnityEngine.Camera camera = null;

		[SerializeField]
		private Transform pictureSpawnpoint = null;

		[SerializeField]
		private GameObject polaroidPrefab = null;

		[SerializeField]
		private Texture2D sheepTexture = null;

        [SerializeField]
        private Transform cameraTransform = null;

        private IEnumerator ResetFlash()
        {
            canFlash = false;
            yield return new WaitForSeconds(GameplaySettings.CameraPictureDelay);
            canFlash = true;
        }

        new private void Awake()
        {
            base.Awake();
			camera = GetComponentInChildren<UnityEngine.Camera>();

            animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
        }

		private void OnEnable()
		{
			if (VRControls.Instance != null)
			{
				VRControls.Instance.TriggerPressed += TriggerPressed;
				VRControls.Instance.TriggerReleased += TriggerReleased;
			}
		}

        private void OnDisable()
		{
			if (VRControls.Instance != null)
			{
				VRControls.Instance.TriggerPressed -= TriggerPressed;
				VRControls.Instance.TriggerReleased -= TriggerReleased;
			}
        }

		public void TakePicture(TriggerPressedEventArgs e)
		{
			if (!canFlash || pictureSpawnpoint.childCount > 0)
				return;

			pressed = true;
			if (e != null)
				e.device.SendHapticImpulse(0, 0.9f, 0.1f);

			audioSource.clip = shutter;
			audioSource.Play();
			StartCoroutine(ResetFlash());
			animator.SetTrigger("Flash");
			EventManager.Instance.OnPictureTaken(this);
			focused = false;

			{
				Plane[] planes = GeometryUtility.CalculateFrustumPlanes(GetComponentInChildren<UnityEngine.Camera>());
                TakenPicture picture = Instantiate(polaroidPrefab, transform.position, transform.rotation, pictureSpawnpoint.transform).GetComponent<TakenPicture>();

                Bounds cameraBounds = new Bounds();
                cameraBounds.center = cameraTransform.transform.position;
                cameraBounds.size = new Vector3(0.05f, 0.05f, 0.05f);

                if (GeometryUtility.TestPlanesAABB(planes, cameraBounds))
				{
					picture.SetPoints(false);
					picture.SetTexture(sheepTexture);
                    return;
				}

				bool targetInPicture = false;
				if (GameManager.Instance.TargetSystem.CurrentTarget)
				{
					if (GeometryUtility.TestPlanesAABB(planes, GameManager.Instance.TargetSystem.CurrentTarget.GetComponentInChildren<Collider>().bounds))
					{
						targetInPicture = true;
						EventManager.Instance.OnPictureTakenTarget(this);
					}
					else
					{
						targetInPicture = false;
						EventManager.Instance.OnPictureTakenTargetless(this);
					}
				}

				picture.SetPoints(targetInPicture);
				picture.SetTexture(GetCameraTexture());
			}
		}

		private Texture2D GetCameraTexture()
		{
			Rect rect = new Rect(0, 0, camera.pixelWidth, camera.pixelHeight);
			Texture2D screenShot = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGBA32, false, true);

			RenderTexture old = RenderTexture.active;

			camera.Render();

			RenderTexture.active = camera.targetTexture;

			screenShot.ReadPixels(rect, 0, 0);
			screenShot.Apply();

			RenderTexture.active = old;
			return screenShot;
		}

		private void TriggerPressed(object sender, TriggerPressedEventArgs e)
		{
			if (pressed)
				return;

			if (!_grabbedBy)
				return;

			if (e.grabber != _grabbedBy)
				return;

			if (e.floatValue > 0.24465f && !focused)
			{
				focused = true;
				audioSource.clip = beep;
				audioSource.Play();
				e.device.SendHapticImpulse(0, e.floatValue, 0.1f);
				return;
			}

			if (e.floatValue >= 0.9f)
				TakePicture(e);
		}

		private void TriggerReleased(object sender, TriggerReleasedEventArgs e)
		{
			if (!_grabbedBy)
				return;

			if (e.grabber != _grabbedBy)
				return;

			pressed = false;
			if (e.value == 0.0f)
				focused = false;
		}

		public override bool Release(Grabber grabber, Vector3 linearVelocity, Vector3 angularVelocity)
		{
			bool b = base.Release(grabber, linearVelocity, angularVelocity);
			if (b)
			{
				pressed = false;
				focused = false;
			}
			return b;
		}
	}
}