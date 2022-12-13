using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 pos;

    private void Update()
    {
        //Вращение камеры
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, ((transform.rotation.eulerAngles.y % 360) + 1), 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, ((transform.rotation.eulerAngles.y % 360) - 1), 0));
        }
        if (Input.GetMouseButton(2))
        {
            float X = -Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
            float eulerY = (transform.rotation.eulerAngles.y + X) % 360;
            transform.rotation = Quaternion.Euler(0, eulerY, 0);
        }
    }

    void LateUpdate()
    {
        //Следование за персонажем
        pos = new Vector3(target.position.x, target.position.y, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, pos, 0.5f);
    }
}
