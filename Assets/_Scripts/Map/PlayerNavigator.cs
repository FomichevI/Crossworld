using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerNavigator : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeed;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine("EnableNavMeshAgain"); //Нужно для того, чтобы персонаж не перескачил в рандомную точку на карте (пока нет другого решения)
    }

    private void FixedUpdate()
    {
        if (_agent.velocity.magnitude < 0.5f)
            _animator.SetInteger("Speed", 0);
        else
        {
            _animator.SetInteger("Speed", 2);
        }
    }
    private IEnumerator EnableNavMeshAgain()
    {
        _agent.enabled = false;
        yield return null;
        _agent.enabled = true;
    }

    void Update()
    {
        //Находим точку на поверхности земли, где был совершен клик и отправляем персонажа бежать к этой точке
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = LayerMask.GetMask("Ground");

                if (Physics.Raycast(ray, out hit, 100, mask))
                {
                    if (hit.transform.gameObject.layer == 3)
                    {
                        _agent.destination = hit.point;
                        _agent.speed = _runSpeed;
                    }
                }
            }
        }
    }
}
