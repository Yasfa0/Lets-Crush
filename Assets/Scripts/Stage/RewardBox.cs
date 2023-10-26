using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardBox : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject ammoIconPrefab;
    private List<AmmoScriptable> receivedReward = new List<AmmoScriptable>();

    public void GenerateReward(int amount,List<AmmoScriptable> rewardList)
    {
        receivedReward.Clear();
        for (int i = 0; i < amount; i++)
        {
            receivedReward.Add(rewardList[Random.Range(0,rewardList.Count)]);
        }

        for (int i = 0; i < receivedReward.Count; i++)
        {
            GameObject tempIcon = Instantiate(ammoIconPrefab,rewardPanel.transform);
            tempIcon.GetComponent<Image>().sprite = receivedReward[i].ammoIcon;
        }
    }

    public void ToMenu()
    {
        Weapon weap = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
        foreach (AmmoScriptable reward in receivedReward)
        {
            weap.AddNewAmmo(reward, 1);
        }
        weap.SaveAmmoData();
        FindObjectOfType<ObjectiveManager>().TogglePause(false);
        SceneManager.LoadScene("MainMenuY2");
    }

}