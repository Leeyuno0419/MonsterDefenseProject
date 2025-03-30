using UnityEngine;
using System.Collections.Generic;

public enum MonsterLane { Top, Middle, Bottom }

public class MonsterManager : MonoBehaviour
{
    [System.Serializable]
    public class LaneInfo
    {
        public MonsterLane lane;
        public Transform spawnPoint;
        public string monsterLayerName;     // "Top", "Middle", "Bottom" (Physics 용)
        public string sortingLayerName;     // "Top", "Middle", "Bottom" (렌더링 용)
    }

    [Header("몬스터 프리팹")]
    public GameObject monsterPrefab;

    [Header("라인별 설정")]
    public List<LaneInfo> lanes;

    [Header("스폰 주기")]
    public float spawnInterval = 1f;

    [Header("풀링 수")]
    public int poolSize = 30;

    private List<GameObject> monsterPool = new List<GameObject>();
    private Dictionary<MonsterLane, Transform> laneParents = new Dictionary<MonsterLane, Transform>();
    private Transform monsterRoot;

    private float nextSpawnTime = 0f;

    void Start()
    {
        CreateMonsterHierarchy();
        InitPool();
        SetNextSpawnTime(); // 최초 스폰 시간 설정
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            int randomIndex = Random.Range(0, lanes.Count);
            LaneInfo lane = lanes[randomIndex];
            SpawnMonster(lane);

            SetNextSpawnTime(); // 다음 스폰 시간 설정
        }
    }

    void SetNextSpawnTime()
    {
        float randomOffset = Random.Range(-0.1f, 0.1f);
        nextSpawnTime = Time.time + spawnInterval + randomOffset;
    }

    void CreateMonsterHierarchy()
    {
        monsterRoot = new GameObject("MonsterRoot").transform;
        monsterRoot.SetParent(this.transform);

        foreach (LaneInfo lane in lanes)
        {
            GameObject laneObj = new GameObject(lane.lane.ToString());
            laneObj.transform.SetParent(monsterRoot);
            laneParents[lane.lane] = laneObj.transform;
        }
    }

    void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(false);
            monster.transform.SetParent(monsterRoot); // 초기 풀링은 루트에
            monsterPool.Add(monster);
        }
    }

    void SpawnMonster(LaneInfo lane)
    {
        GameObject monster = GetPooledMonster();
        if (monster != null)
        {
            // 위치 설정
            monster.transform.position = lane.spawnPoint.position;
            monster.SetActive(true);

            // 부모 설정
            if (laneParents.TryGetValue(lane.lane, out Transform parent))
            {
                monster.transform.SetParent(parent);
            }

            // 물리 레이어 설정
            int monsterLayer = LayerMask.NameToLayer(lane.monsterLayerName);
            monster.layer = monsterLayer;
            foreach (Transform child in monster.transform)
            {
                child.gameObject.layer = monsterLayer;
            }

            // MonsterMove 설정
            MonsterMove move = monster.GetComponent<MonsterMove>();
            if (move != null)
            {
                move.monsterLayer = LayerMask.GetMask(lane.monsterLayerName);
            }

            // 렌더링 레이어 설정
            SpriteRenderer[] parts = monster.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in parts)
            {
                sr.sortingLayerName = lane.sortingLayerName;
            }
        }
    }

    GameObject GetPooledMonster()
    {
        foreach (GameObject monster in monsterPool)
        {
            if (!monster.activeInHierarchy)
            {
                return monster;
            }
        }
        return null;
    }
}
