using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private EnemyList m_enemyList;
    [SerializeField]
    private GameObject m_parent;

    [SerializeField]
    private BattleManager m_battleManager;

    private void Awake()
    {
        if (m_parent != null && m_enemyList != null)
        {
            var enemy = m_enemyList.GetRandomEnemy();
            Vector3 vector3 = new Vector3(0.0f, 0.0f, 4.5f);
            Quaternion quaternion = Quaternion.Euler(0.0f, 149.6280f, 0.0f);
            enemy = Instantiate(enemy, vector3, quaternion, m_parent.transform);
            if (enemy.TryGetComponent<EnemyController>(out EnemyController component) && m_battleManager != null)
            {
                m_battleManager.EnemyController = component;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
