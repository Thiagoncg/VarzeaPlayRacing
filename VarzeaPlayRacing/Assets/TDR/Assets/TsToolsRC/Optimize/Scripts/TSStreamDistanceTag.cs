// Description: TSStreamDistanceTag: Allows to activate or deactivate 
// the children of this object depending the player distance.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TS.Generics
{
    public class TSStreamDistanceTag : MonoBehaviour
    {
        [HideInInspector]
        public bool     isInitDone;

        public int      howManyObjectUpdatedByFrame = 10;

        public float    DistanceMin = 300;
        public float    refDistancePerSecond = 40;

        [HideInInspector]
        public bool     isAlreadyEnable = false;


        public List<Transform> transformToCheckList = new List<Transform>();
        public List<Vector3> positionToCheckList = new List<Vector3>();

        public UnityEvent ActionOnDisable;
        public UnityEvent ActionOnEnable;

        void Start()
        {
            #region
            if (TSOptiGrid.instance)
                StartCoroutine(InitRoutine(false, true));
            #endregion
        }

        void OnEnable()
        {
            #region
            if (TSOptiGrid.instance)
                StartCoroutine(InitRoutine(false,true));
            #endregion
        }

        void OnDisable()
        {
            #region
            StopAllCoroutines();
            #endregion
        }

        float CalculateDistanceToTarget()
        {
            #region
            float lastDist = -1;
            float tmpDist = -1;
            for (var i =0;i< TSOptiGrid.instance.targetsList.Count; i++)
            {
                
                if (transformToCheckList.Count > 0)
                {

                    for (var j = 0; j < transformToCheckList.Count; j++)
                    {
                        Vector3 targetPos = TSOptiGrid.instance.targetsList[i].target.position;
                        Transform posToCheck = transformToCheckList[j];
                        float dist = Vector3.Distance(posToCheck.position, targetPos);

                        // Debug.Log(dist);

                        if (tmpDist == -1 || tmpDist > dist)
                            tmpDist = dist;

                        // if (DistanceMin > dist)
                        //    tmpDist = -1;
                    }

                    if (tmpDist != -1)
                        lastDist = tmpDist;
                    else
                        lastDist = -1;

                }
                else if (positionToCheckList.Count > 0)
                {
                  
                    for (var j = 0; j < positionToCheckList.Count; j++)
                    {
                        Vector3 targetPos = TSOptiGrid.instance.targetsList[i].target.position;
                        Vector3 posToCheck = positionToCheckList[j] + transform.position;
                        float dist = Vector3.Distance(posToCheck, targetPos);

                       // Debug.Log(dist);

                        if (tmpDist == -1 || tmpDist > dist)
                            tmpDist = dist;

                       // if (DistanceMin > dist)
                        //    tmpDist = -1;
                    }

                    if(tmpDist != -1)
                        lastDist = tmpDist;
                    else
                        lastDist = -1;

                }
                else
                {
                    Vector3 targetPos = TSOptiGrid.instance.targetsList[i].target.position;
                    float dist = Vector3.Distance(transform.position, targetPos);

                    if (lastDist == -1 || lastDist > dist)
                        lastDist = dist;
                }
                
            }

            return lastDist;
            #endregion
        }

        public IEnumerator InitRoutine(bool newObjState,bool onEnableInit = false)
        {
            #region
            isInitDone = false;

            yield return new WaitUntil(() => TSOptiGrid.instance.isInitDone);

            float distanceToTarget = CalculateDistanceToTarget();

            if (distanceToTarget < DistanceMin)
                newObjState = true;

            //Debug.Log(distanceToTarget + " -> " + isAlreadyEnable +  " | " + newObjState);

            if (isAlreadyEnable != newObjState || onEnableInit)
            {
                if (newObjState)
                    ActionOnEnable.Invoke();
                else
                    ActionOnDisable.Invoke();


                // Add objects inside TSOptiGrid
                for (var i = 0; i < transform.childCount; i++)
                    TSOptiGrid.instance.AddObjToList(transform.GetChild(i).gameObject, newObjState);
            
            }

            isAlreadyEnable = newObjState;

            float waitDurationUntilNextCheck = distanceToTarget - DistanceMin;

            waitDurationUntilNextCheck /= refDistancePerSecond;


           

            // Target is inside the Zone
            if (waitDurationUntilNextCheck < 0) waitDurationUntilNextCheck = 5;

            float timer = 0;
            while(timer < waitDurationUntilNextCheck)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            isInitDone = true;

            StartCoroutine(InitRoutine(false));
            #endregion
        }
    }
}
