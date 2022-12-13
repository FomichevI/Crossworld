using UnityEngine;

public class BuildingHide : MonoBehaviour //ќтвечает за скрытие зданий и больших объектов, если они наход€тс€ между персонажем и камерой
{
    [SerializeField] private Transform _playerTransform;
    private GameObject _currentBuilding;
    [SerializeField] private float _distCalculate;

    private Ray _castRay;
    private RaycastHit _castHit;

    void Update()
    {
        Vector3 direction = _playerTransform.position - Camera.main.transform.position;
        _castRay = new Ray(Camera.main.transform.position - direction, direction); //”величиваем рассто€ние луча на случай, когда камера заходит внутрь колайдера здани€
        _castHit = new RaycastHit();
        _distCalculate = Vector3.Distance(_playerTransform.position, Camera.main.transform.position);

        if (Physics.Raycast(_castRay, out _castHit, _distCalculate*2-1f)) //“ак же увеличиваем дистанцию луча
        {
            if (_castHit.collider != null)
            {
                if (_castHit.collider.CompareTag("Buildings"))
                {
                    if (_currentBuilding != null)
                    {
                        if (_currentBuilding != _castHit.collider.gameObject)
                        {
                            EnableBuilding(true);
                            _currentBuilding = _castHit.collider.gameObject;
                            EnableBuilding(false);
                        }
                    }
                    else
                    {
                        _currentBuilding = _castHit.collider.gameObject;
                        EnableBuilding(false);
                    }
                }
                else
                {
                    if (_currentBuilding != null)
                    {
                        EnableBuilding(true);
                        _currentBuilding = null;
                    }
                }
            }
        }
    }

    public void EnableBuilding(bool boolOperation)
    {
        if (_currentBuilding.transform.parent != null && _currentBuilding.transform.parent.CompareTag("Buildings"))
        {
            for (int i = 0; i < _currentBuilding.transform.parent.childCount; i++)
            {
                MeshRenderer buildingMesh = _currentBuilding.transform.parent.GetChild(i).GetComponent<MeshRenderer>() ?? null;

                if (buildingMesh != null)
                {
                    buildingMesh.enabled = boolOperation;
                }
            }
        }       
        else
        {
            MeshRenderer buildingMesh = _currentBuilding.GetComponent<MeshRenderer>() ?? null;
            if (buildingMesh != null)
            {
                buildingMesh.enabled = boolOperation;
            }
        }
        if (_currentBuilding.transform.childCount > 0)
        {
            for (int i = 0; i < _currentBuilding.transform.childCount; i++)
            {
                ParticleSystem ps = _currentBuilding.transform.GetChild(i).GetComponent<ParticleSystem>() ?? null;
                if (ps != null)
                {
                    if (boolOperation)
                        ps.Play();
                    else
                        ps.Stop();
                }
                MeshRenderer buildinMesh = _currentBuilding.transform.GetChild(i).GetComponent<MeshRenderer>() ?? null;
                if (buildinMesh != null)
                {
                    buildinMesh.enabled = boolOperation;
                }
            }
        }
    }
}
