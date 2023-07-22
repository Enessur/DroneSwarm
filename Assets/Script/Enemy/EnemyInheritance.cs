using UnityEngine;

namespace Enemy
{
    public class EnemyInheritance : EnemyBehaviour
    {
        private enum Inheritance
        {
            Teleport,
            None
        }

        [SerializeField] private NewEnemyTypeScriptable newEnemyType;
        [SerializeField] private Inheritance inheritence = Inheritance.None;
        private float _teleportDistance = 2f;

        protected override void Start()
        {
            base.Start();

            m_enemyDataInstance.SetValue(newEnemyType);
        }

        protected override void Update()
        {
            base.Update();

            if (Vector3.Distance(transform.position, _target.position) < _teleportDistance)
            {
                inheritence = Inheritance.Teleport;
            }
            else
            {
                inheritence = Inheritance.None;
            }

            switch (inheritence)
            {
                case Inheritance.Teleport:
                    break;
                case Inheritance.None:
                    break;
            }
        }
    }
}