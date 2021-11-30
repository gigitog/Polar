using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotation : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform polytech;
    [SerializeField] private float speed;
    [SerializeField] private float timerNum;
    [SerializeField] private float maxRotation = 90;
    private bool isChanged;
    private bool isDragging;
    private Rigidbody rb;
    private float timer;

    private void Start()
    {
        rb = polytech.GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (isDragging)
        {
            if (Input.touchCount != 1) return;

            var t = Input.GetTouch(0);

            var rotation = polytech.rotation;

            var deltaRotationX = t.deltaPosition.x * Time.fixedDeltaTime * speed;
            var deltaRotationY = t.deltaPosition.y * Time.fixedDeltaTime * speed;

            rb.AddTorque(Vector3.right * deltaRotationY, ForceMode.VelocityChange);
            rb.AddTorque(Vector3.down * deltaRotationX, ForceMode.VelocityChange);
        }
        else
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                Debug.Log(timer);
            }
            else
            {
                if (isChanged)
                    SetOriginalPos();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        isChanged = true;
        timer = timerNum;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void SetOriginalPos()
    {
        Debug.Log("Rotate Orig");
        polytech.DORotate(new Vector3(-35, 0, 0), 1);
        isChanged = false;
    }
}