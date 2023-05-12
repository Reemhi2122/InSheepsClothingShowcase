using InSheepsClothing.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InSheepsClothing
{
    public class TargetSystem : MonoBehaviour
    {
        private Target currentTarget = null;
        public Target CurrentTarget => currentTarget;

        private Target[] targets;

        private void Awake() => targets = FindObjectsOfType<Target>();

        private void OnEnable()
        {
            EventManager.Instance.PictureTakenTarget += PictureTakenTarget;
            EventManager.Instance.RoundStart += RoundStart;
        }

        private void OnDisable()
        {
            EventManager.Instance.PictureTakenTarget -= PictureTakenTarget;
            EventManager.Instance.RoundStart -= RoundStart;
        }

        private void RoundStart(object sender) => SetTarget();

        private void PictureTakenTarget(object sender) => SetTarget();

        public void SetTarget()
        {
            ClearTarget();
            Target newTarget = null;
            while (!newTarget || newTarget == currentTarget || !newTarget.gameObject.activeInHierarchy)
                newTarget = targets[Random.Range(0, targets.Length)];
            currentTarget = newTarget;
            currentTarget.Select();
        }

        public void ClearTarget()
        {
            if (currentTarget)
                currentTarget.Deselect();
            currentTarget = null;
        }
    }
}