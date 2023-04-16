using UnityEngine;

namespace PlayerComponents
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10.0f;
        [SerializeField] private float slowTime = 0.2f; //in seconds
        [SerializeField] private float accelerateTime = 0.1f; // in seconds

        private float _currentSpeed = 0.0f;
    
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 inputValue)
        {
            inputValue = Vector2.ClampMagnitude(inputValue, 1.0f);
        
            if (inputValue.magnitude > 0.1f)
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, movementSpeed, (1.0f / accelerateTime) * Time.deltaTime);
            
                var currentVelocity = inputValue * _currentSpeed;
                _rigidbody.velocity = currentVelocity;
            }
            else
            {
                _currentSpeed = Mathf.Lerp(_currentSpeed, 0.0f, (1.0f / slowTime) * Time.deltaTime);
                _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, Vector2.zero, (1.0f / slowTime) * Time.deltaTime);
            }

        }
    }
}

