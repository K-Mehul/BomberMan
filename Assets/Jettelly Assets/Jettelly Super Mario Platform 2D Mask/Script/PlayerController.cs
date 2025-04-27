using System;
using UnityEngine;
using UnityEngine.Serialization;
// ReSharper disable All

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MeshMaskGroup
    {
        public MeshRenderer Renderer;
        public bool IsFullTransparent;
    }

    [SerializeField] private MeshMaskGroup[] _group;
    [SerializeField] [Range(1.0f, 10.0f)] private float _playerSpeed = 5f;

    private int _maskPlayerPosition = Shader.PropertyToID("_MaskPlayerPosition");
    private int _maskIsFullTransparent = Shader.PropertyToID("_IsFullTransparent");
    
    private Vector3 _oldPlayerPosition;
    private Vector2 _currentPlayerPosition;
    private Vector2 _input = Vector2.zero;
    private Vector2 _movement = Vector2.zero;
    private int _length;

    private Material[] _materialInstance;
    private bool[] _instanceTransparencyValue;
   
    void Start()
    {
        SetMaterialInstance();
        SetMaskPosition();
    }
    
    void Update()
    {
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _movement = new Vector3(_input.x, _input.y, 0f) * (_playerSpeed * Time.deltaTime);
        
        transform.Translate(_movement);
        if (transform.position != _oldPlayerPosition)
        {
            SetMaskPosition();
        }
    }

    private void SetMaterialInstance()
    {
        if (_group != null && _group.Length > 0)
        {
            _length = _group.Length;
            _materialInstance = new Material[_length];
            _instanceTransparencyValue = new bool[_length];
            
            for (int i = 0; i < _group.Length; i++)
            {
                if (_group[i] != null )
                {
                    MeshRenderer meshRenderer = _group[i].Renderer.GetComponent<MeshRenderer>();
                    bool isFullTransparent = _group[i].IsFullTransparent;

                    if (meshRenderer != null)
                    {
                        _materialInstance[i] = Instantiate(meshRenderer.material);
                        meshRenderer.material = _materialInstance[i];
                        _instanceTransparencyValue[i] = isFullTransparent;
                    }
                }
            }
        }
        else
        {
            Debug.Log($"You need to assign the meshes to the mesh group!");
        }
    }

    public void SetMaskPosition()
    {
        if (_group != null && _group.Length > 0)
        {
            for (int i = 0; i < _group.Length; i++)
            {
                _currentPlayerPosition = new Vector2(transform.position.x, transform.position.y);
                _materialInstance[i].SetVector(_maskPlayerPosition, _currentPlayerPosition);
                _materialInstance[i].SetInt(_maskIsFullTransparent, Convert.ToInt16(_instanceTransparencyValue[i]));
                _oldPlayerPosition = transform.position;
            }
        }
    }
}
