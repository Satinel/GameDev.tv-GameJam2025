using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageSplash : MonoBehaviour
{
    [SerializeField] float _destroyDelay = 2.5f;

    [SerializeField] Image _outlineImage, _insideImage;
    [SerializeField] TextMeshProUGUI _text;

    void Start()
    {
        Destroy(gameObject, _destroyDelay);
    }

    public void Setup(Color outline, Color inside, Color font, string text)
    {
        _outlineImage.color = outline;
        _insideImage.color = inside;
        _text.color = font;
        _text.text = text;
    }
}
