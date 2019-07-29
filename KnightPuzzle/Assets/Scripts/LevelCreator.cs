using System;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField]
    private TextAsset _levelFile;
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

    private char[,] _playingField;

    private void Start()
    {
        if(_verifyPlayingGrid())
        {
            _generatePlayingGrid();
        }
    }

    private bool _verifyPlayingGrid()
    {
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
                        Instantiate(_knightPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    case 'C':
                        Instantiate(_coinPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    case '#':
                        Instantiate(_wallPrefab, new Vector3(i, 0, j), Quaternion.identity);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
