using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private Vector3 offset;  // Nesnenin hareket etmesi i�in ofset
    private float zDistance;  // Kameradan mesafe
    private Rigidbody rb;  // Nesnenin rigidbody'si
    private Vector3 smoothVelocity;  // Hareketi yumu�atmak i�in h�z

    [Header("Movement Ranges")]
    public Vector2 xRange = new Vector2(-4f, 4f);  // X eksenindeki s�n�rlar
    public Vector2 yRange = new Vector2(0.5f, 3.5f);  // Y eksenindeki s�n�rlar
    public Vector2 zRange = new Vector2(-4f, 4f);  // Z eksenindeki s�n�rlar
    public float yOffset = 0.5f;  // Y�kseklik ofseti

    private float yOffsetOnMouseDown = 2f; // Y ekseninde fare ile bas�ld���nda eklenen offset (2 birim)

    private bool isDragging = false;  // Nesne s�r�kleniyor mu?

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody'yi al
    }

    void OnMouseDown()
    {
        zDistance = Camera.main.WorldToScreenPoint(transform.position).z;  // Kameradan mesafeyi al
        offset = transform.position - GetMouseWorldPosition();  // Fare ile nesne aras�ndaki mesafeyi hesapla
        isDragging = true;  // S�r�klemeye ba�la
        rb.useGravity = false;  // S�r�klerken yer�ekimini devre d��� b�rak
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + offset;  // Hareket ettirilecek hedef pozisyon

            // Fareyi yukar�-a�a�� hareket ettirirken Y eksenini manuel olarak ayarla
            float mouseY = Input.mousePosition.y;
            targetPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, mouseY, zDistance)).y + yOffsetOnMouseDown;

            // X ve Z eksenlerinde nesneyi s�n�rland�r
            targetPosition.x = Mathf.Clamp(targetPosition.x, xRange.x, xRange.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, zRange.x, zRange.y);

            // Hareketi yumu�at
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 0.1f);

            // Nesneyi ta��
            rb.MovePosition(smoothedPosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;  // S�r�kleme bitiyor
        rb.useGravity = true;  // Yer�ekimini tekrar devreye sok
    }

    // Fare pozisyonunu d�nya koordinatlar�na �evir
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
