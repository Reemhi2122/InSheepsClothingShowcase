using InSheepsClothing.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InSheepsClothing
{
    public class Ipad : MonoBehaviour
    {
        public Slider TotalSuspicousness;
        public Slider[] IndividualSuspicousness = new Slider[11];
        public GameObject TextBox;
        public TMP_Text TextPrefab;

        private SheepSystem sheepSystem;
        private int amountPic = 0;

        private void OnEnable()
        {
            EventManager.Instance.RoundStart += RoundStart;
        }

        private void RoundStart(object obj)
        {
            EventManager.Instance.RoundStart -= RoundStart;
            EventManager.Instance.PictureInSuitcase += PictureInSuitcase;
        }

        private void OnDisable()
        {
            EventManager.Instance.PictureInSuitcase -= PictureInSuitcase;
        }

        private void PictureInSuitcase(object sender, PictureInSuitcaseEventArgs e)
        {
            TMP_Text newText = Instantiate(TextPrefab, TextBox.transform);
            if (e.picture.ReceivePoints)
            {
                amountPic++;
                newText.text = "Picture amount: " + amountPic;
            }
            else
            {
                newText.text = "Don't send me useless pictures please!";
            }
        }

        public void Start()
        {
            sheepSystem = GameManager.Instance.SheepSystem;
        }

        public void UpdateIpadBars()
        {
            for (int i = 0; i < sheepSystem.AllSheep.Count; i++)
                IndividualSuspicousness[i].gameObject.SetActive(true);
        }

        public void Update()
        {
            float TotalSheepSuspicousness = 0;

            for (int i = 0; i < sheepSystem.AllSheep.Count; i++)
            {
                float correctedValue = ((float)sheepSystem.AllSheep[i].GetSuspiciousness()) / 100.0f;
                IndividualSuspicousness[i].value = correctedValue;
                TotalSheepSuspicousness += correctedValue;
            }

            if(TotalSheepSuspicousness > 0)
                TotalSuspicousness.value = TotalSheepSuspicousness / GameManager.Instance.SheepSystem.AllSheep.Count;
        }
    }
}
