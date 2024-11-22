using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private Vector3 offset;  // Nesnenin hareket etmesi için ofset
    private float zDistance;  // Kameradan mesafe
    private Rigidbody rb;  // Nesnenin rigidbody'si
    private Vector3 smoothVelocity;  // Hareketi yumuþatmak için hýz

    [Header("Movement Ranges")]
    public Vector2 xRange = new Vector2(-4f, 4f);  // X eksenindeki sýnýrlar
    public Vector2 yRange = new Vector2(0.5f, 3.5f);  // Y eksenindeki sýnýrlar
    public Vector2 zRange = new Vector2(-4f, 4f);  // Z eksenindeki sýnýrlar
    public float yOffset = 0.5f;  // Yükseklik ofseti

    private float yOffsetOnMouseDown = 2f; // Y ekseninde fare ile basýldýðýnda eklenen offset (2 birim)

    private bool isDragging = false;  // Nesne sürükleniyor mu?

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody'yi al
    }

    void OnMouseDown()
    {
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;  // Kameradan mesafeyi al
        offset = transform.position - GetMouseWorldPosition();  // Fare ile nesne arasýndaki mesafeyi hesapla
        isDragging = true;  // Sürüklemeye baþla
        rb.useGravity = false;  // Sürüklerken yerçekimini devre dýþý býrak
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + offset;  // Hareket ettirilecek hedef pozisyon

            // Fareyi yukarý-aþaðý hareket ettirirken Y eksenini manuel olarak ayarla
            float mouseY = Input.mousePosition.y;
            targetPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, mouseY, zDistance)).y + yOffsetOnMouseDown;

            // X ve Z eksenlerinde nesneyi sýnýrlandýr
            targetPosition.x = Mathf.Clamp(targetPosition.x, xRange.x, xRange.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, zRange.x, zRange.y);

            // Hareketi yumuþat
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.1f);

            // Nesneyi taþý
            rb.MovePosition(smoothedPosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;  // Sürükleme bitiyor
        rb.useGravity = true;  // Yerçekimini tekrar devreye sok
    }

    // Fare pozisyonunu dünya koordinatlarýna çevir
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
