﻿// Description: PauseManager: Manage pause in gameplay scene.
// OnPause and OnUnpause are delegate that can be used for pause something.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace TS.Generics
{
    public class PauseManager : MonoBehaviour
    {
        public bool                 SeeInspector;
        public static PauseManager  instance = null;
        public bool                 helpBox = true;
        public bool                 moreOptions;

        public bool                 Bool_IsGamePaused;

        public Action<int>          OnPause;
        public Action<int>          OnUnPause;

        public bool                 isPauseModeEnable;

        [System.Serializable]
        public class C_Pause
        {
            public string       m_Name;
            public UnityEvent   m_Pause;
            public UnityEvent   m_Unpause;
        }

        public List<C_Pause>        listOfPause = new List<C_Pause>();

        void Awake()
        {
            #region Create ony one instance of the gameObject in the Hierarchy
            if (instance == null)
                instance = this;
            #endregion
        }

        public void PauseChangeAnimatorSpeed(float _speed)
        {
            #region 
            Animator[] foundObjects = FindObjectsByType<Animator>(FindObjectsSortMode.None);

            foreach (Animator child in foundObjects)
                child.speed = _speed; 
            #endregion
        }

        public void PauseAudio(bool b_PauseAudio)
        {
            #region 
            AudioSource[] foundObjects = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (AudioSource child in foundObjects)
            {
                if (!child.GetComponent<IgnorePause>())
                {
                    if (b_PauseAudio) child.Pause();
                    else child.UnPause();
                }

            } 
            #endregion
        }

        public void PauseParticle(bool b_PauseParticle)
        {
            #region 
            ParticleSystem[] foundObjects = FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);

            foreach (ParticleSystem child in foundObjects)
            {
                if (!child.GetComponent<IgnorePause>())
                {
                    if (b_PauseParticle) child.Pause();
                    else if (child.isPaused) child.Play();
                }
            } 
            #endregion
        }

        public void bPauseGame_Bool_IsGamePaused()
        {
            #region 
            Bool_IsGamePaused = true; 
            #endregion
        }

        public void UnpauseGame_Bool_IsGamePaused()
        {
            #region 
            Bool_IsGamePaused = false; 
            #endregion
        }

        public void PauseGame(int selectedPause = 0)
        {
            #region 
            OnPause.Invoke(selectedPause);
            listOfPause[selectedPause].m_Pause.Invoke(); 
            #endregion
        }

        public void UnpauseGame(int SelectedUnpause = 0)
        {
            #region MyRegion
            OnUnPause.Invoke(SelectedUnpause);
            listOfPause[SelectedUnpause].m_Unpause.Invoke(); 
            #endregion
        }
    }
}
