using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InSheepsClothing.Events;

namespace InSheepsClothing
{
    public class PeekSheep : Sheep
    {
        private Animator DoorAnimator;

        private Timer 
            peekSheepTimer = new Timer(),
            peekSheepPeekTimer = new Timer();

        private void Awake()
        {
            DoorAnimator = gameObject.GetComponent<Animator>();

            peekSheepTimer.Enabled = false;
            peekSheepPeekTimer.Enabled = false;

            peekSheepTimer.SetTime(GameplaySettings.PeekSheepTimeDelayRoundStart);
            peekSheepPeekTimer.SetTime(GameplaySettings.PeekSheepTimeMax);
            peekSheepPeekTimer.TickAction = StopPeeking;
        }

        private new void OnEnable()
        {
            base.OnEnable();
            EventManager.Instance.RoundStart += RoundStart;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            EventManager.Instance.RoundStart -= RoundStart;
        }


        private new void Update()
        {
            base.Update();
            peekSheepTimer.Update();
            peekSheepPeekTimer.Update();
        }

        private void RoundStart(object sender)
        {
            EventManager.Instance.RoundStart -= RoundStart;

            peekSheepTimer.Enabled = true;
            peekSheepTimer.TickAction = PeekSheepGracePeriodEnded;
        }

        private void PeekSheepGracePeriodEnded()
        {
            float time = GameplaySettings.PeekSheepTimeMax;
            DisableTimer(ref peekSheepTimer, time);

            peekSheepTimer.TickAction = PeekSheepTimerFinished;
            Peek();
        }

        private void PeekSheepTimerFinished()
        {
            float time = peekSheepTimer.GetTime() * GameplaySettings.PeekSheepTimeMultiplier;
            time = Mathf.Clamp(time, GameplaySettings.PeekSheepTimeMin, GameplaySettings.PeekSheepTimeMax);
            DisableTimer(ref peekSheepTimer, time);

            Peek();
        }

        private void Peek()
        {
            float time = peekSheepPeekTimer.GetTime() * GameplaySettings.PeekSheepPeekMultiplier;
            time = Mathf.Clamp(time, GameplaySettings.PeekSheepPeekMin, GameplaySettings.PeekSheepPeekMax);
            DisableTimer(ref peekSheepPeekTimer, time);

            peekSheepPeekTimer.Enabled = true;
            DoorAnimator.SetBool("IsPeeking", true);
        }

        private void StopPeeking()
        {
            DoorAnimator.SetBool("IsPeeking", false);
            peekSheepPeekTimer.Enabled = false;
            peekSheepTimer.Enabled = true;
        }

        private void DisableTimer(ref Timer timer, float time)
        {
            timer.Enabled = false;
            timer.Reset();
            timer.SetTime(time);
        }
    }
}