using UnityEngine;

namespace InSheepsClothing
{
    public class OutlinedObject : MonoBehaviour
    {
        private Outline[] outlines;

        private void Awake()
        {
            outlines = GetComponentsInChildren<Outline>();
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        public void Select()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }

        public void Deselect()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }
    }
}