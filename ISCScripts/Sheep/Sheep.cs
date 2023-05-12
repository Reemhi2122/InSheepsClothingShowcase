using InSheepsClothing.Events;
using UnityEngine;

namespace InSheepsClothing
{
    public class Sheep : MonoBehaviour
    {
        protected bool IsLooking = false;
        private int Suspiciousness = 0;

        private int MaxSuspiciousness = GameplaySettings.SheepMaxSuspiciousness;

        private Timer DecreaseSuspicousnessTimer = new Timer();

        protected void Update()
        {
            DecreaseSuspicousnessTimer.Update();
        }

        protected void OnEnable()
        {
            EventManager.Instance.RoundStart += StartDecreaseTimer;
        }

        protected void OnDisable()
        {
            EventManager.Instance.RoundStart -= StartDecreaseTimer;
        }

        public void StartDecreaseTimer(object sender)
        {
            DecreaseSuspicousnessTimer.SetTime(5);
            DecreaseSuspicousnessTimer.TickAction = DecreaseSuspicousness;
            DecreaseSuspicousnessTimer.Enabled = true;
        }

        private void DecreaseSuspicousness()
        {
            Suspiciousness = Mathf.Clamp(Suspiciousness - 1, 0, 100);
            DecreaseSuspicousnessTimer.Reset();
        }
        public void EnableLooking() => IsLooking = true;
        public void DisableLooking() => IsLooking = false;

        public virtual bool IsSheepLooking() => IsLooking;

        public void AddSuspiciousness()
        {
            if(Suspiciousness < MaxSuspiciousness)
                Suspiciousness += GameplaySettings.SheepIncrementSuspiciousness;
        }

        public int GetSuspiciousness() => Suspiciousness;
    }
}
