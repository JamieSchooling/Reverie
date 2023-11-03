using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DashGhost : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 1f;

    private SpriteRenderer _spriteRenderer;
    private Color _currentColour;

    public Sprite Sprite
    {
        get => _spriteRenderer.sprite;
        set => _spriteRenderer.sprite = value;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentColour = _spriteRenderer.color;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _currentColour = _spriteRenderer.color;
        _currentColour.a = 1;
    }

    private void Update()
    {
        _currentColour.a -= _fadeSpeed * Time.deltaTime;
        _spriteRenderer.color = _currentColour;

        if ( _currentColour.a <= 0 )
            gameObject.SetActive(false);
    }
}
