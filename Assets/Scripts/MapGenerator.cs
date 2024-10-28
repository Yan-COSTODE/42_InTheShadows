using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapTile tile;
    [SerializeField] private GameObject path;
    [SerializeField] private float fPathLength = 3.0f;
    private LevelTemplateElem[] database;
    private List<GameObject> tiles = new();

    public void GenerateMap(LevelTemplateElem[] _database)
    {
        database = _database;
        
        for (int i = 0; i < database.Length; i++)
            GenerateTile(i, i < database.Length - 1);
    }

    public void DestroyMap()
    {
        foreach (GameObject _tile in tiles)
            Destroy(_tile);

        tiles.Clear();
    }
    
    private void GenerateTile(int _index, bool _path)
    {
        Vector3 _pos = new Vector3(_index * fPathLength, 0, 0);
        MapTile _tile = Instantiate(tile, _pos, Quaternion.identity, transform);
        LevelTemplateElem _elem = database[_index];
        _tile.Init(_elem.Completed, _elem.Query, _elem.FastestTime, _index);
        tiles.Add(_tile.gameObject);
        
        if (_elem.Completed)
            GameManager.Instance.UIManager.Selection.SetLevel(_index + 1);
            
        if (!_path)
            return;
        
        _pos.x += fPathLength / 2;
        tiles.Add(Instantiate(path, _pos, Quaternion.identity, transform));
    }
}
