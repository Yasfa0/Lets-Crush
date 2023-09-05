using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    private List<GameObject> enemyTeamCharacter = new List<GameObject>();
    private List<GameObject> friendlyTeamCharacter = new List<GameObject>();
    [SerializeField] private GameObject bentengPlayer;
    [SerializeField] private GameObject bentengEnemy;
    [SerializeField] private List<Transform> neutralPoints = new List<Transform>();
    [SerializeField] private BaseChecker playerBaseChecker;
    [SerializeField] private BaseChecker enemyBaseChecker;
    [SerializeField] private List<GameObject> enemySpawnPosts = new List<GameObject>();
    [SerializeField] private List<GameObject> playerSpawnPosts = new List<GameObject>();
    [SerializeField] private Transform enemyChaseLimit;
    [SerializeField] private Transform friendlyChaseLimit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != null)
        {
            Destroy(this);
        }

        FillTeams();
    }

    public void FillTeams()
    {
        friendlyTeamCharacter.Clear();
        enemyTeamCharacter.Clear();

        foreach (GameObject oppChar in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyTeamCharacter.Add(oppChar);
        }

        friendlyTeamCharacter.Add(GameObject.FindGameObjectWithTag("Player"));
        foreach (GameObject oppChar in GameObject.FindGameObjectsWithTag("Friendly"))
        {
           friendlyTeamCharacter.Add(oppChar);
        }
    }

    public List<GameObject> GetEnemyTeam()
    {
        return enemyTeamCharacter;
    }

    public List<GameObject> GetFriendlyTeam()
    {
        return friendlyTeamCharacter;
    }

    public BaseChecker GetPlayerBaseChecker()
    {
        return playerBaseChecker;
    }

    public BaseChecker GetEnemyBaseChecker()
    {
        return enemyBaseChecker;
    }

    public List<GameObject> GetEnemyPosts()
    {
        return enemySpawnPosts;
    }

    public List<GameObject> GetPlayerPosts()
    {
        return playerSpawnPosts;
    }

    public Transform GetEnemyChaseLimit()
    {
        return enemyChaseLimit;
    }
    public Transform GetFriendlyChaseLimit()
    {
        return friendlyChaseLimit;
    }

    public GameObject GetBentengPlayer()
    {
        return bentengPlayer;
    }

    public GameObject GetBentengEnemy()
    {
        return bentengEnemy;
    }

    public List<Transform> GetNeutralPoints()
    {
        return neutralPoints;
    }
}
