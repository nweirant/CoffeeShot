using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] _tileTypes;
    public bool corner = false;
    public bool stopTile;

    private void Start()
    {
        if(!stopTile)
        {
            int randomTile = Random.Range(0, _tileTypes.Length);
            Instantiate(_tileTypes[randomTile], transform.position, transform.rotation);
        }
    }
}
