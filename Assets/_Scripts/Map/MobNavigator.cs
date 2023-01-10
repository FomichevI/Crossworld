using UnityEngine;
using UnityEngine.AI;

public class MobNavigator : MonoBehaviour
{
    protected Animator _animator;
    private NavMeshAgent _agent;
    private bool _isRunMode = false;
    private bool _isMoving = false;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runMultiplier;
    [SerializeField] private float _idleTime = 3f; //Время, которое моб будет находиться на месте прежде, чем продолжить движение
    private float _currentIdleTime; //Счетчик нахождения на одном месте
    [SerializeField] private float _moveDistance = 4; //Дальность, на которую моб передвинится за раз
    private Transform _playerTrans; //Transform игрока
    [SerializeField] private float _followDistance; //Дистанция, на которой моб перестает преследовать игрока
    [Header("Настройки аудио")]
    [SerializeField] private AudioClip _startRun;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        OnStart();
    }
    public virtual void OnStart() //Необходим для дочерних классов
    {    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            //Анимация движения
            if (_agent.velocity.magnitude < 0.5f && _currentIdleTime > 0 && !_isRunMode)
            {
                _isMoving = false;
                _animator.SetInteger("Speed", 0);
                _currentIdleTime = _idleTime;
            }
            else if (_isRunMode)
            {
                _agent.speed = _moveSpeed*_runMultiplier;
                _animator.speed = _runMultiplier;
                _animator.SetInteger("Speed", 1);
            }
            else
            {
                _agent.speed = _moveSpeed;
                _animator.speed = 1f;
                _animator.SetInteger("Speed", 1);
            }
            //Следование за игроком
            if(_playerTrans!= null && _isRunMode)
            {
                if ((transform.position - _playerTrans.position).magnitude > _followDistance)
                {
                    _agent.destination = transform.position;
                    _isRunMode = false;
                }
                else
                {
                    _agent.destination = _playerTrans.position;
                }
            }
            //Счетчик во время движения (необходим для того, чтобы персонаж успел набрать скорость после начала движения. Иначе - слетает анимация)
            if (_currentIdleTime <= 0) _currentIdleTime += 0.02f;
        }
        else
        {
            if (_currentIdleTime >= 0)
            {
                _currentIdleTime -= 0.02f;
            }
            else if (_currentIdleTime < 0) //После окончания ожидания
            {
                _currentIdleTime -= 1f;
                float x = Random.Range(-_moveDistance, +_moveDistance);
                float z = Mathf.Sqrt(Mathf.Pow(_moveDistance, 2) - Mathf.Pow(x, 2));
                if (Random.Range(0, 2) == 0) z = -z;
                Vector3 v = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
                _agent.destination = v;
                _isMoving = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Если задевает триггер игрока, то начинает за ним следовать
        if (other.CompareTag("Player")) StartRun(other.transform);              
    }
    private void StartRun(Transform target)
    {
        _playerTrans = target;
        _isRunMode = true;
        _isMoving = true;
        _agent.destination = _playerTrans.position;
        _audioSource.PlayOneShot(_startRun);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Если сталкивается с персонажем, то начинается файт
        if (collision.gameObject.CompareTag("Player")) StartFight();              
    }
    private void StartFight()
    {
        string[] name = this.name.Split(' ');
        LocationController.S.StartFight(name[0]);
        Destroy(gameObject);
    }
}
