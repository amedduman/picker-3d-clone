namespace Picker3d
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PickerEntity : MonoBehaviour
    {
        [SerializeField] float _horizontalMoveSensitivity = 10;
        Rigidbody _rb;
        float _mouseX;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            _mouseX = Input.GetAxis("Mouse X");
        }

        void FixedUpdate()
        {
            _rb.position += new Vector3(_mouseX * Time.deltaTime * _horizontalMoveSensitivity, 0, 0);
        }
    }

}