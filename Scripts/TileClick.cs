using UnityEngine;
using UnityEngine.EventSystems;

public class TileClick : MonoBehaviour, IPointerClickHandler
{
    private MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = GameObject.Find("Map").GetComponent<MapGenerator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var pos = transform.position;
        Debug.Log($"Clicked on tile with type {mapGenerator.Tiles[(int) pos.x, (int) pos.y].Type}");
    }
}