using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementWithCursor : MonoBehaviour
{
    public float speed = 10f; 
    public float acceleration = 10f; 
    public float deceleration = 5f; 
    public float rotationSpeed = 30f; 
        //4시간 똥꼬쇼 - FollowingCursor&CharacterMovement 통합완료 시이이이밟ㄹㄹㄹ
        //FollowingCursor - 후에 투사체 발사(ShootingBullet? 같은걸)로 변경예정

    private Vector2 direction = Vector2.zero; 
    private Vector2 velocity = Vector2.zero; 

    void Update()
    {
       
        if (direction != Vector2.zero)
        {
            velocity = Vector2.MoveTowards(velocity, direction * speed, acceleration * Time.deltaTime);
        }
        else
        {
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;

     
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = transform.position.z;

            Vector3 cursorDirection = mousePosition - transform.position;
            float angle = Mathf.Atan2(cursorDirection.y, cursorDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), rotationSpeed * Time.deltaTime);
        }
        else if (velocity != Vector2.zero) 
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        
        if (direction.x != 0) 
        {
            transform.localScale = new Vector3(direction.x < 0 ? -1f : 1f, 1f, 1f);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
       
        direction = context.ReadValue<Vector2>().normalized;
    }
}
