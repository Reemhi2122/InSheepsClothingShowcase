using InSheepsClothing.Events;
using UnityEngine;

namespace InSheepsClothing
{
    public class WindowWashingSheep : Sheep
    {
        private Animator animator = null;

        [SerializeField]
        private Timer 
            windowWashingSheepTimer = new Timer(),
            windowWashingSheepPeekTimer = new Timer();

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();

            windowWashingSheepTimer.Enabled = false;
            windowWashingSheepPeekTimer.Enabled = false;

            windowWashingSheepTimer.SetTime(GameplaySettings.WindowSheepDelayRoundStart);
            windowWashingSheepPeekTimer.SetTime(GameplaySettings.WindowSheepPeekTimeMin);
            windowWashingSheepPeekTimer.TickAction = StopPeeking;
        }

        private void OnEnable()
        {
            EventManager.Instance.RoundStart += RoundStart;
        }

        private void Update()
        {
            windowWashingSheepTimer.Update();
            windowWashingSheepPeekTimer.Update();
        }

        private void RoundStart(object sender)
        {
            EventManager.Instance.RoundStart -= RoundStart;

            windowWashingSheepTimer.Enabled = true;
            windowWashingSheepTimer.TickAction = PeekSheepGracePeriodEnded;
        }

        private void PeekSheepGracePeriodEnded()
        {
            float time = GameplaySettings.PeekSheepTimeMax;
            DisableTimer(ref windowWashingSheepTimer, time);

            windowWashingSheepTimer.TickAction = PeekSheepTimerFinished;
            Peek();
        }

        private void PeekSheepTimerFinished()
        {
            float time = windowWashingSheepTimer.GetTime() * GameplaySettings.PeekSheepTimeMultiplier;
            time = Mathf.Clamp(time, GameplaySettings.PeekSheepTimeMin, GameplaySettings.PeekSheepTimeMax);
            DisableTimer(ref windowWashingSheepTimer, time);

            Peek();
        }

        private void Peek()
        {
            float time = windowWashingSheepPeekTimer.GetTime() * GameplaySettings.PeekSheepPeekMultiplier;
            time = Mathf.Clamp(time, GameplaySettings.PeekSheepPeekMin, GameplaySettings.PeekSheepPeekMax);
            DisableTimer(ref windowWashingSheepPeekTimer, time);

            windowWashingSheepPeekTimer.Enabled = true;
            animator.SetBool("IsPeeking", true);
        }

        private void StopPeeking()
        {
            animator.SetBool("IsPeeking", false);
            windowWashingSheepPeekTimer.Enabled = false;
            windowWashingSheepTimer.Enabled = true;
        }

        private void DisableTimer(ref Timer timer, float time)
        {
            timer.Enabled = false;
            timer.Reset();
            timer.SetTime(time);
        }
    }
}