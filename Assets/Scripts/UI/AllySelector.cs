using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllySelector : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private AudioClip audioKlik;
    [SerializeField] private Image iconImg; 
    [SerializeField] private List<SummonData> summonableList = new List<SummonData>();

    private void Awake()
    {
        iconImg.sprite = summonableList[currentIndex].icon;    
    }

    public void ChangeSummonable(int val)
    {
        currentIndex = currentIndex + val;

        Debug.Log("Current Index " + currentIndex);
        if(currentIndex >= summonableList.Count)
        {
            currentIndex = 0;
        }else if(currentIndex < 0)
        {
            currentIndex = summonableList.Count-1;
        }

        AudioManagerY.Instance.PlayAudio(audioKlik,1);
        iconImg.sprite = summonableList[currentIndex].icon;
    }

    public List<SummonData> GetSummonableList()
    {
        return summonableList;
    }

    public SummonData GetSelectedSummon()
    {
        return summonableList[currentIndex];
    }

}
