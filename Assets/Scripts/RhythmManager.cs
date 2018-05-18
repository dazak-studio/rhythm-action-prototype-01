using System.Collections;
using UnityEngine;

public class RhythmManager : Singleton<RhythmManager>
{
    enum BAR_AUTOMATA
    {
        NONE,
        REMOVE_FIRST_BAR,
        REMOVE_LAST_BAR
    }


    private BAR_AUTOMATA   bar_automata = BAR_AUTOMATA.NONE;

    public void Initialize()
    {
        _triggerTime = .0f;
        LiftTime = 60.0f / SoundManager.GetInstance.BgmInfo.Bpm;
        //LiftTime = 60.0f / 5;     //for debug.
        
        Debug.Log("SoundManager.GetInstance.BgmInfo.Bpm >> " + SoundManager.GetInstance.BgmInfo.Bpm + "   LiftTime >> " + LiftTime);
          
        CleanUp();            
        _rhythmControlCoroutine = StartCoroutine(RhythmControlCoroutine());
    }

    // 노트들 제어하는 코루틴
    private IEnumerator RhythmControlCoroutine()
    {
        while (true)
        {
            // 스핀 락 형태로 조건부 확인
            while (SoundManager.GetInstance.BgmAudioSource.time < _triggerTime)
            {
                SyncRhythm();
                yield return new WaitForEndOfFrame();
            }

            _triggerTime += LiftTime;    
            _checkCycle = true; 

            //
            if(bar_automata == BAR_AUTOMATA.REMOVE_FIRST_BAR)
            {
                firstRhythmBar.GetComponent<RhythmBar>().ShowBar(true);
                lastRhythmBar.GetComponent<RhythmBar>().ShowBar(false);
                bar_automata = BAR_AUTOMATA.REMOVE_LAST_BAR;
            }
            else if(bar_automata == BAR_AUTOMATA.REMOVE_LAST_BAR)
            {
                lastRhythmBar.GetComponent<RhythmBar>().ShowBar(true);
                bar_automata = BAR_AUTOMATA.NONE;
            }
         
            //Debug.Log(" _checkCycle >> true");
        
         }                        
        // ReSharper disable once IteratorNeverReturns
    }

    private void SyncRhythm()
    {
        _syncRate = (_triggerTime - SoundManager.GetInstance.BgmAudioSource.time) / LiftTime;
        
        if(_checkCycle == true && _syncRate < (1.0f - Accuracy))
        {
            _checkCycle = false;

            // START RHYTHM CYCLE..
            _isTouchRhythmBar = false;
        }

    }

    public void CleanUp()
    {
        _checkCycle = true;
        _isTouchRhythmBar = true;
        
    }

    public float GetRhythmScore()
    {
        var tmpScore = Mathf.Abs((_triggerTime - SoundManager.GetInstance.BgmAudioSource.time) / LiftTime);

        if (tmpScore < 0.3f)
            return (1.0f - tmpScore);
        else
            return tmpScore;
    }

    
//    public float JudgeRhythm()
//    {
//        if(_syncRate < Accuracy ||  _syncRate > (1.0f - Accuracy)) 
//        {
//            if(_isTouchRhythmBar == false)
//            {
//                Debug.Log("SYNC RATE >>" + _syncRate);
//                _isTouchRhythmBar = true;
//
//                /* 
//                if(_syncRate > (1.0f - Accuracy))
//                {
//                    // remove last rhythmbar
//                    lastRhythmBar.GetComponent<RhythmBar>().ShowBar(false);
//                    bar_automata = BAR_AUTOMATA.REMOVE_LAST_BAR;
//                }
//                else
//                {
//                     firstRhythmBar.GetComponent<RhythmBar>().ShowBar(false);
//                     bar_automata = BAR_AUTOMATA.REMOVE_FIRST_BAR;
//                }*/
//                return true;
//            }
//
//            _isTouchRhythmBar = true;
//            
//            // 
//            return false;
//        }
//        else
//        {
//        
//        }
//
//        _isTouchRhythmBar = true;
//        Debug.Log("MISSED !");
//
//        // remove 0 rhythmbar
//        firstRhythmBar.GetComponent<RhythmBar>().ShowBar(false);
//        bar_automata = BAR_AUTOMATA.REMOVE_FIRST_BAR;
//        return false;
//    }                

    public const float Accuracy = 0.30f;
    
    [HideInInspector] public float LiftTime;

    private float _triggerTime;        
    public  float _syncRate;        

    public  GameObject  lastRhythmBar, firstRhythmBar;

    private bool  _isTouchRhythmBar = true;
    private bool  _checkCycle = true;
    
    private Coroutine _rhythmControlCoroutine;
}