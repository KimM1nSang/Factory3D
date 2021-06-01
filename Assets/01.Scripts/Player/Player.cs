using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;      // 캐릭터 움직임 스피드.
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.

    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.

    [SerializeField] private Camera cam;
    [SerializeField] private Transform camTrm;
    [SerializeField] private Transform camDownTrm;
    [SerializeField] private Transform camUpTrm;
    [SerializeField] private bool isCamUp;


    public float turnSpeed = 4.0f; // 마우스 회전 속도
    public float moveSpeed = 2.0f; // 이동 속도

    private float xRotate = 0.0f; // 카메라 위 아래

    void Start()
    {
        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();

        cam.transform.position = camTrm.position;
        cam.transform.eulerAngles = camTrm.localEulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
    }
  
    void Update()
    {
        Move();
        MouseRotation();
        CamMoveCheck();
        CamMove();
    }

    public bool IsCamUp()
    {
        return isCamUp && cam.transform.position != camDownTrm.position;
    }

    private void CamMoveCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isCamUp = !isCamUp;
            camTrm = isCamUp ? camUpTrm : camDownTrm;
            Cursor.lockState = isCamUp ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    private void CamMove()
    {
        if (cam.transform.position == camTrm.position) return;
        cam.transform.position = Vector3.Lerp(cam.transform.position, camTrm.position,0.5f);
        cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, camTrm.localEulerAngles, 0.5f);
    }
    void MouseRotation()
    {
        if (IsCamUp()) return;
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        cam.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
    private void Move()
    {
        // 현재 캐릭터가 땅에 있는가?
        if (controller.isGrounded)
        {
            // 상하, 좌우 움직임 셋팅. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= speed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeed;

        }

        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        controller.Move(MoveDir * Time.deltaTime);
    }
}
