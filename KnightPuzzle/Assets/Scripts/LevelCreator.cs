using System;
using UnityEditor;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{ 
    [SerializeField]
    private GameObject _whiteBlockPrefab;
    [SerializeField]
    private GameObject _blackBlockPrefab;
    [SerializeField]
    private GameObject _knightPrefab;
    [SerializeField]
    private GameObject _coinPrefab;
    [SerializeField]
    private GameObject _wallPrefab;

    private TextAsset _levelFile;
    private char[,] _playingField;

    private void Start()
    {
        _levelFile = Resources.Load<TextAsset>("TextFiles/Level");

        if(_verifyPlayingGrid())
        {
            _generatePlayingGrid();
        }
    }

    public void RecreateGrid()
    {
        AssetDatabase.ImportAsset("Assets/Resources/TextFiles/Level.txt");
        _levelFile = Resources.Load<TextAsset>("TextFiles/Level");
        if (_verifyPlayingGrid())
        {
            _clearExistingGrid();
            _generatePlayingGrid();
        }
    }

    private bool _verifyPlayingGrid()
    {
        if(_levelFile == null)
        {
            Debug.LogError("No level file!");
            return false;
        }

        string readFile = _levelFile.text;
        string[] stringSeparators = new string[] { "\r\n" };
        string[] rows = readFile.Split(stringSeparators, StringSplitOptions.None);

        for(int i = 0; i < rows.Length; i++)
        {
            if(rows[i].Length != rows.Length)
            {
                Debug.Log("Grid format invalid!");
                return false;
            }
        }

        Debug.Log("Grid format valid.");

        _convertInto2DArray(rows);

        return true;
    }

    private void _convertInto2DArray(string[] rows)
    {
        _playingField = new char[rows.Length, rows.Length];

        for(int i = 0; i < rows.Length; i++)
        {
            char[] rowSplit = rows[i].ToCharArray();

            for(int j = 0; j < rowSplit.Length; j++)
            {
                _playingField[i, j] = rowSplit[j];
            }
        }
    }

    private void _generatePlayingGrid()
    {
        for(int i = 0; i < _playingField.GetLength(0); i++)
        {
            for(int j = 0; j < _playingField.GetLength(1); j++)
            {
                if((i + j) % 2 == 0)
                {
                    Instantiate(_blackBlockPrefab, new Vector3(i, -0.5f, j), Quaternion.identity);
                }
                else
                {
                    Instantiate(_whiteBlockPrefab, new Vector3(i, -0.5f, j), Quaternion.identity);
                }

                switch(_playingField[i, j])
                {
                    case 'K':
                    case 'k':
                        GameObject temp = Instantiate(_knightPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        FindObjectOfType<GameManager>().PlayerPosition = temp.transform;
                        break;
                    case 'C':
                    case 'c':
                        GameObject tempCoin = Instantiate(_coinPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        FindObjectOfType<GameManager>().Coins.Add(tempCoin);
                        break;
                    case '#':
                        Instantiate(_wallPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    default:
                        break;
                }
            }
        }

        FindObjectOfType<GameManager>().PlayingField = _playingField;
    }

    private void _clearExistingGrid()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach(GameObject destroyable in walls)
        {
            GameObject.Destroy(destroyable);
        }
        foreach (GameObject destroyable in blocks)
        {
            GameObject.Destroy(destroyable);
        }

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        foreach(GameObject coin in FindObjectOfType<GameManager>().Coins)
        {
            FindObjectOfType<GameManager>().Coins.Remove(coin);
            GameObject.Destroy(coin);
        }
    }
}
