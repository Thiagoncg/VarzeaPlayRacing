﻿// Description: Countdown: Allows to create UI Countdown. Call from the SceneStepManager bStep3_TT_Countdown.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace TS.Generics
{
    public class Countdown : MonoBehaviour
    {
        [HideInInspector]
        public bool                 SeeInspector;
        [HideInInspector]
        public bool                 moreOptions;
        [HideInInspector]
        public bool                 helpBox = true;

        public int                  editorSelectedList;
        public string               editorNewCountdownName;

        public static Countdown     instance = null;
        public bool                 b_InitDone;
        public bool                 b_IsCountdownEnded;


        [System.Serializable]
        public class EventParams
        {
            public string           _Name;
            public List<UnityEvent> unityEventsList = new List<UnityEvent>();
            public float            StepDuration = 1;
            public UnityEvent       unityEventsAfterCountdown = new UnityEvent();
            public Action<int>      ActionAfterCountdown;
        }

        public List<EventParams>    multiListUnityEvents = new List<EventParams>();
        bool                        bSplitscreenInit = false;

        public Action countdownStartAction;

        void Awake()
        {
            #region 
            //-> Check if instance already exists
            if (instance == null)
                instance = this; 
            #endregion
        }

        //-> 
        public bool BCountdown(int countdownID = 0)
        {
            #region
            if (b_IsCountdownEnded)
                StartCoroutine(CountdownRoutine(countdownID));

            return true;
            #endregion
        }

        IEnumerator CountdownRoutine(int countdownID)
        {
            #region 
            b_IsCountdownEnded = false;

            countdownStartAction?.Invoke();

            SplitsreenInit();

            while (!bSplitscreenInit) yield return null;

            for (var i = 0; i < multiListUnityEvents[countdownID].unityEventsList.Count; i++)
            {
                float t = multiListUnityEvents[countdownID].StepDuration;

                multiListUnityEvents[countdownID].unityEventsList[i].Invoke();


                while (t > 0)
                {
                    t -= Time.deltaTime;

                    yield return null;
                }
                yield return null;
            }

            multiListUnityEvents[countdownID].unityEventsAfterCountdown.Invoke();
            multiListUnityEvents[countdownID].ActionAfterCountdown?.Invoke(countdownID);

            b_IsCountdownEnded = true;
            yield return null; 
            #endregion
        }

        public void CallNextGameModeStep()
        {
            #region 
            SceneStepsManager.instance.NextStep(); 
            #endregion
        }

        public void SplitsreenInit()
        {
            #region 
            int howManyPlayers = InfoRememberMainMenuSelection.instance.playerMainMenuSelection.HowManyPlayer;

            if (howManyPlayers > 1)
            {

            }

            bSplitscreenInit = true; 
            #endregion
        }
    }
}
