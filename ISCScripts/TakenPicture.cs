using System;
using UnityEngine;
using VRTools.Interaction;

namespace InSheepsClothing.Interaction
{
    public class TakenPicture : GrabbableObject
    {
        private bool receivePoints = false;
        public bool ReceivePoints => receivePoints;

        private bool pickedUp = false;

        private Rigidbody rbody = null;

        [SerializeField]
        private MeshRenderer meshRenderer = null;

        public override bool Grab(Grabber grabber, bool parentGrabbedObject = false)
        {
            _initialTransform = null;

            bool b = base.Grab(grabber, parentGrabbedObject);
            if (b && !pickedUp)
            {
                EnablePhysics();
                pickedUp = b;
            }

            return b;
        }

        public void EnablePhysics()
        {
            transform.SetParent(null);
            rbody.isKinematic = false;
            pickedUp = false;
            GetComponent<Collider>().isTrigger = false;
        }

        new private void Awake()
        {
            base.Awake();
            rbody = GetComponentInChildren<Rigidbody>();
            rbody.isKinematic = true;
        }

        public void SetPoints(bool b)
        {
            receivePoints = b;
        }

        public void SetTexture(Texture2D texture2D)
        {
            meshRenderer.material.SetTexture("_MainTex", texture2D);
            meshRenderer.material.SetTexture("_BaseMap", texture2D);
            meshRenderer.material.SetTexture("_Albedo", texture2D);
        }
    }
}