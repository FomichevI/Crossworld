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
    [SerializeField] private float _idleTime = 3f; //�����, ������� ��� ����� ���������� �� ����� ������, ��� ���������� ��������
    private float _currentIdleTime; //������� ���������� �� ����� �����
    [SerializeField] private float _moveDistance = 4; //���������, �� ������� ��� ������������ �� ���
    private Transform _playerTrans; //Transform ������
    [SerializeField] private float _followDistance; //���������, �� ������� ��� ��������� ������������ ������
    [Header("��������� �����")]
    [SerializeField] private AudioClip _startRun;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        OnStart();
    }
    public virtual void OnStart() //��������� ��� �������� �������
    {    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            //�������� ��������
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
            //���������� �� �������
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
            //������� �� ����� �������� (��������� ��� ����, ����� �������� ����� ������� �������� ����� ������ ��������. ����� - ������� ��������)
            if (_currentIdleTime <= 0) _currentIdleTime += 0.02f;
        }
        else
        {
            if (_currentIdleTime >= 0)
            {
                _currentIdleTime -= 0.02f;
            }
            else if (_currentIdleTime < 0) //����� ��������� ��������
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
        //���� �������� ������� ������, �� �������� �� ��� ���������
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
        //���� ������������ � ����������, �� ���������� ����
        if (collision.gameObject.CompareTag("Player")) StartFight();              
    }
    private void StartFight()
    {
        string[] name = this.name.Split(' ');
        LocationController.S.StartFight(name[0]);
        Destroy(gameObject);
    }
}
