// using UnityEngine;
// using UnityEngine.InputSystem;

// public class CursorFollowing : MonoBehaviour
// {
//     public float rotationSpeed = 30f; 

//     void Update()
//     {
       
//         if (Input.GetMouseButton(0))
//         {
            
//             Vector3 mousePosition = Mouse.current.position.ReadValue();

//             mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

//             mousePosition.z = transform.position.z;

//             Vector3 direction = mousePosition - transform.position;

//             float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//             transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle -90), rotationSpeed * Time.deltaTime); 
//         }
//     }
// }

// using UnityEngine;
// using UnityEngine.InputSystem;

// public class CharacterMovement : MonoBehaviour
// {
//     public float speed = 5f; 
//     public float acceleration = 10f; 
//     public float deceleration = 5f; 
//     public float rotationSpeed = 30f;

//     private Vector2 direction = Vector2.zero; 
//     private Vector2 velocity = Vector2.zero; 

//     void Update()
//     {
        
//         if (direction != Vector2.zero)
//         {
//             velocity = Vector2.MoveTowards(velocity, direction * speed, acceleration * Time.deltaTime);              
//         }
//         else
//         {
           
//             velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
//         }

//         transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;

//         Vector3 mousePosition = Mouse.current.position.ReadValue();

//         mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

//         mousePosition.z = transform.position.z;

//         Vector3 direction = mousePosition - transform.position;
    
//         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//         transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle -90), rotationSpeed * Time.deltaTime); 

      
//         if (velocity != Vector2.zero)
//         {
//             float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
//             transform.rotation = Quaternion.Euler(0, 0, angle - 90);

//             transform.localScale = new Vector3(velocity.x < 0 ? -1f : 1f, 1f, 1f);
//         }
//     }

//     public void OnMove(InputAction.CallbackContext context)
//     {
        
//         direction = context.ReadValue<Vector2>().normalized; 
//     }
// }

