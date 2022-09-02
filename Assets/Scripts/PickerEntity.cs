namespace Picker3d
{   
    using System.Collections;
    using UnityEngine;
    using DG.Tweening;

    public class PickerEntity : MonoBehaviour
    {
        [SerializeField] float _speed = 10;
        [SerializeField] float _horizontalMoveSensitivity = 10;
        [SerializeField] [Min(0)] float _horizontalMoveLimit = 5;
        [SerializeField] Rigidbody _pusher;
        Rigidbody _rb;
        float _mouseX;
        float _actualPickerSpeed;

        void OnEnable()
        {
            GameManager.Instance.OnGameStart += Move;
            GameManager.Instance.OnNextLevelReady += Move;
        }

        void OnDisable()
        {
            GameManager.Instance.OnGameStart -= Move;
            GameManager.Instance.OnNextLevelReady -= Move;
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if(_actualPickerSpeed == 0) return; // don't move left or right when stopping 
            if(Input.GetMouseButton(0))
            {
                _mouseX = Input.GetAxis("Mouse X");
            }
            else
            {
                _mouseX = 0;
            }

            if(Input.GetKeyDown(KeyCode.A))
            {
                PushCollectables();
            }
        }

        void FixedUpdate()
        {
            float x = _mouseX * Time.deltaTime * _horizontalMoveSensitivity + _rb.position.x; 
            x = Mathf.Clamp(x, -_horizontalMoveLimit, _horizontalMoveLimit);
            _rb.position = new Vector3(x, _rb.position.y, _rb.position.z + _actualPickerSpeed * Time.deltaTime);
        }

        void Move()
        {
            _actualPickerSpeed = _speed;
        }

        public void Stop()
        {
            _actualPickerSpeed = 0;
        }

        public void PushCollectables()
        {
            _pusher.position = transform.position;
            _pusher.DOMoveZ(_pusher.position.z + 1, GameValues.PickerPushCollectablesTime);
        }
    }

}