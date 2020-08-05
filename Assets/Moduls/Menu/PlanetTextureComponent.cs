using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetTextureComponent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite _sprite;
    public void OnPointerClick(PointerEventData eventData)
    {
        MenuClickSystem.OnPlanetClick(_sprite);
    }
}
