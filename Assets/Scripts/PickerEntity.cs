namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PickerEntity : MonoBehaviour
    {
        [SerializeField] float _speed = 10;
        [SerializeField] float _horizontalMoveSensitivity = 10;
        [SerializeField] [Min(0)] float _horizontalMoveLimit = 5;
        Rigidbody _rb;
        float _mouseX;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if(Input.GetMouseButton(0))
            {
                _mouseX = Input.GetAxis("Mouse X");
            }
            else
            {
                _mouseX = 0;
            }
        }

        void FixedUpdate()
        {
            float x = _mouseX * Time.deltaTime * _horizontalMoveSensitivity + _rb.position.x; 
            // _rb.position += new Vector3(_mouseX * Time.deltaTime * _horizontalMoveSensitivity, 0, _speed * Time.deltaTime);
            x = Mathf.Clamp(x, -_horizontalMoveLimit, _horizontalMoveLimit);
            _rb.position = new Vector3(x, _rb.position.y, _rb.position.z + _speed * Time.deltaTime);
            // _rb.position += new Vector3(0, 0, _speed * Time.deltaTime);
        }
    }

}