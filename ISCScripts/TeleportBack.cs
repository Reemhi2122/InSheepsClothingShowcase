using UnityEngine;

namespace InSheepsClothing
{
    public class TeleportBack : MonoBehaviour
    {
        protected Vector3 initialPosition;
        protected Quaternion initialRotation;

        public float MaxDistance = 0.8f;

        protected void Awake()
        {
            initialPosition = this.transform.position;
            initialRotation = this.transform.rotation;
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("TeleportSurface"))
            {
                ReturnToInitalPosition();
                return;
            }

            if(Vector3.Distance(this.transform.position, initialPosition) > MaxDistance)
                ReturnToInitalPosition();
        }

        protected virtual void ReturnToInitalPosition()
        {
            this.transform.position = initialPosition;
            this.transform.rotation = initialRotation;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
