using InSheepsClothing.Events;
using InSheepsClothing.Interaction;
using UnityEngine;

namespace InSheepsClothing
{
    public class Suitcase : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<TakenPicture>())
            {
                TakenPicture p = collision.gameObject.GetComponent<TakenPicture>();
                p.CanBeGrabbed = false;
                Destroy(p.GetComponent<Rigidbody>());
                Destroy(p.GetComponent<Collider>());

                PictureInSuitcaseEventArgs e = new PictureInSuitcaseEventArgs
                {
                    picture = p
                };
                EventManager.Instance.OnPictureInSuitcase(this, e);
            }
        }
    }
}