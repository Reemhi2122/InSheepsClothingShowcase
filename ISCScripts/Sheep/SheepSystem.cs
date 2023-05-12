using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InSheepsClothing.Events;

namespace InSheepsClothing
{
    public class SheepSystem : MonoBehaviour
    {
        public List<Sheep> AllSheep;
        public List<VotingSheep> availableSheep; 
        public List<VotingSheep> VotingSheepList;
        public PeekSheep peekSheep;
        public PresentationSheep presentationSheep;
        public WindowWashingSheep windowWashingSheep;

        private Timer sheepSpawnTimer = new Timer();
		
        private void RoundStart(object sender)
        {
            StartTimer(this);
            EventManager.Instance.PictureTaken += CheckIfSheepLooking;
			for (int i = 0; i < GameplaySettings.AmountOfStartingSheep; i++)
			{
				int randomSheep = Random.Range(0, availableSheep.Count);
				AllSheep.Add(availableSheep[randomSheep]);
				VotingSheepList.Add(availableSheep[randomSheep]);
				availableSheep[randomSheep].transform.parent.gameObject.SetActive(true);
				availableSheep.RemoveAt(randomSheep);
			}

			AllSheep.Add(presentationSheep);
			AllSheep.Add(peekSheep);
			AllSheep.Add(windowWashingSheep);

            GameManager.Instance.Ipad.UpdateIpadBars();

			sheepSpawnTimer.SetTime(GameplaySettings.ExtraSheepSpawnTimer);
			sheepSpawnTimer.Enabled = true;
			sheepSpawnTimer.TickAction = OnSpawnNewSheep;
		}

        private void OnEnable()
        {
            EventManager.Instance.RoundStart += RoundStart;
        }

        private void OnDisable()
        {
            EventManager.Instance.PictureTaken -= CheckIfSheepLooking;
            EventManager.Instance.RoundStart -= StartTimer;
        }

        private void Update()
        {
            sheepSpawnTimer.Update();
        }

        public void CheckIfSheepLooking(object sender)
        {
            for(int i = 0; i < AllSheep.Count; i++)
            {
                if (AllSheep[i].IsSheepLooking())
                {
                    AllSheep[i].AddSuspiciousness();
                    GameManager.Instance.Player.Detected();
                }
            }
        }

        public int GetSheepSuspiciousness()
        {
            int TotalSuspiciousness = 0;
            for (int i = 0; i < AllSheep.Count; i++)
                TotalSuspiciousness += AllSheep[i].GetSuspiciousness();
            return TotalSuspiciousness;
        }

        public void StartTimer(object sender)
        {
            sheepSpawnTimer.Enabled = true;
        }

        public void OnSpawnNewSheep()
        {
            if (availableSheep.Count > 0) {
                int randomSheep = Random.Range(0, availableSheep.Count);
                AllSheep.Add(availableSheep[randomSheep]);
                VotingSheepList.Add(availableSheep[randomSheep]);
                availableSheep[randomSheep].transform.parent.gameObject.SetActive(true);
                availableSheep.RemoveAt(randomSheep);

                sheepSpawnTimer.SetTime(GameplaySettings.ExtraSheepSpawnTimer);
                GameManager.Instance.Ipad.UpdateIpadBars();

                return;
            }

            sheepSpawnTimer.Enabled = false;
        }
    }
}