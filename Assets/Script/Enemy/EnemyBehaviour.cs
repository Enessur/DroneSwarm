using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public enum TaskCycleEnemy
    {
        Chase,
        Idle,
        Patrol
    }

    [SerializeField] protected EnemyScriptableObject m_EnemyData;
    [SerializeField] protected TaskCycleEnemy taskCycleEnemy;
    [SerializeField] private float xMin, yMin, xMax, yMax, zMin, zMax;
    [SerializeField] private float startWaitTime = 1f;
    [SerializeField]  private HealthBar healthBar;
    
   
    public Rigidbody Rb => _rb;
    public GameObject patrolBorders;
    public Transform moveSpot;
    protected Transform _target;
    private Rigidbody _rb;
    protected int currentHealth;
    private Vector3 _patrolPos;
    private float _patrolTimer;
    protected EnemyScriptableObject m_enemyDataInstance;

    protected virtual void Awake()
    {
        m_enemyDataInstance = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        m_enemyDataInstance.SetValue(m_EnemyData);
    }

    protected virtual void Start()
    {
        BoxCollider squareCollider = patrolBorders.GetComponent<BoxCollider>();

        xMin = patrolBorders.transform.position.x - squareCollider.size.x / 2;
        xMax = patrolBorders.transform.position.x + squareCollider.size.x / 2;
        yMin = patrolBorders.transform.position.y - squareCollider.size.y / 2;
        yMax = patrolBorders.transform.position.y + squareCollider.size.y / 2;
        zMin = patrolBorders.transform.position.z - squareCollider.size.z / 2;
        zMax = patrolBorders.transform.position.z + squareCollider.size.z / 2;
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _patrolPos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
        _rb = GetComponent<Rigidbody>();
        moveSpot.SetParent(null);
        patrolBorders.transform.parent = null;
        
        healthBar.UpdateHealthBar(m_enemyDataInstance.maxHealth, m_enemyDataInstance.health);
    }


    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, _target.position) < m_enemyDataInstance.chasingDistance)
        {
            taskCycleEnemy = TaskCycleEnemy.Chase;
        }
        else
        {
            taskCycleEnemy = TaskCycleEnemy.Patrol;
        }


        switch (taskCycleEnemy)
        {
            case TaskCycleEnemy.Chase:
                transform.position =
                    Vector3.MoveTowards(transform.position, _target.position,
                        m_enemyDataInstance.chaseSpeed * Time.deltaTime);
                break;
            case TaskCycleEnemy.Patrol:
                PatrolPosition();
                transform.position =
                    transform.position =
                        Vector3.MoveTowards(transform.position, _patrolPos,
                            m_enemyDataInstance.patrolSpeed * Time.deltaTime);
                moveSpot.position = _patrolPos;

                break;
        }
    }

    private void PatrolPosition()
    {
        _patrolTimer += Time.deltaTime;

        if (!(_patrolTimer >= startWaitTime)) return;
        _patrolTimer = 0;

        transform.position =
            Vector3.MoveTowards(transform.position, _patrolPos, m_enemyDataInstance.patrolSpeed * Time.deltaTime);

        if (transform.position == (Vector3)_patrolPos)
        {
            _patrolPos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Drone"))
        {
            TakeDamage();
        }

        if (col.CompareTag("Detection"))
        {
            TargetManager.Instance.AddEnemy(this);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Detection"))
        {
            TargetManager.Instance.RemoveEnemy(this);
        }
    }

    private void TakeDamage()
    {
        m_enemyDataInstance.health -= 5;
        healthBar.UpdateHealthBar(m_enemyDataInstance.maxHealth, m_enemyDataInstance.health);
        if (m_enemyDataInstance.health < 1)
        {
            TargetManager.Instance.RemoveEnemy(this);
            DestroyImmediate(m_enemyDataInstance);
            Destroy(this.gameObject);
        }
    }
    // protected virtual void SpeedUp(float value)
    // {
    //     item.patrolSpeed += value;
    // }
}