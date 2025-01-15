using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyList", menuName = "ScriptableObjects/EnemyList")]

public class EnemyList : ScriptableObject
{
    [System.Serializable]
    public struct Enemy
    {
        [SerializeField]
        private GameObject enemyObject;
        public GameObject EnemyObject
        {
            set => enemyObject = value;
            get => enemyObject;
        }

        [SerializeField]
        private EnemyParameter enemyParameter;
        public EnemyParameter Parameter
        {
            set => enemyParameter = value;
            get => enemyParameter;
        }

        public (GameObject, EnemyParameter) GetEnemy()
        {
            return (enemyObject, enemyParameter);
        }



    }

    [SerializeField]
    private List<Enemy> enemyList;

    public GameObject GetRandomEnemy()
    {
        if (!(enemyList?.Count > 0))
            return null;
        int count = enemyList.Count;
        var enemy = enemyList[Random.Range(0, count)];

        (var enemyObject, var parameter) = enemy.GetEnemy();
        if (enemyObject == null) return null;
        if (enemyObject.TryGetComponent(out EnemyController component) && parameter != null)
        {
            component.Parameter = parameter;
        }
        return enemyObject;
    }

}
