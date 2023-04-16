using UnityEngine;

namespace WeaponsSystem
{
    public class FloatingWeapon : MonoBehaviour
    {
        [SerializeField] private float distanceFromParent = 0.5f;
        [SerializeField] private Transform firingPoint;
        
        private Camera _mainCamera;
        private SpriteRenderer _renderer;
        private Transform _parent;

        private Vector2 _initialScale;

        public Transform FiringPoint => firingPoint;

        private void Start()
        {
            _mainCamera = Camera.main;
            _renderer = GetComponent<SpriteRenderer>();
            _parent = transform.parent;
            _initialScale = transform.localScale;
        }
        
        private void Update()
        {
            RotateTowardsCamera();
        }

        private void RotateTowardsCamera()
        {
            if (_mainCamera is null || _parent is null)
            {
                return;
            }
        
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
            var directionToMouse = (mousePosition - (Vector2)_parent.position).normalized;
            transform.right = directionToMouse; // makes object looks at the mouse
            transform.localPosition = directionToMouse * distanceFromParent; // moves the object in circle around the parent
        
            // Have to flip the sprite's y axis so its not upside down when its on the player's left
            var scale = transform.localScale;
            scale.y = _initialScale.y * (directionToMouse.x < 0.0f ? -1 : 1);
            transform.localScale = scale;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Just wanted to make sure the designer got instant feedback for this.
            transform.localPosition = new Vector3(distanceFromParent, 0, 0);
        }

        private void OnDrawGizmos()
        {
            if (firingPoint != null)
            {
                UnityEditor.Handles.DrawWireDisc(FiringPoint.transform.position, Vector3.back, 0.1f);
            }
        }
#endif
    }
}
