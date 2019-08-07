using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public char[,] PlayingField;
    public Transform PlayerPosition;
    public List<GameObject> Coins;

    private void Start()
    {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        Coins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));

        if(Coins.Count <= 0)
        {
            Debug.LogError("No coins in the playing field");
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                int targetX = Mathf.RoundToInt(hit.transform.position.x);
                int targetZ = Mathf.RoundToInt(hit.transform.position.z);

                if (_checkMoveValidity(targetX, targetZ))
                {
                    PlayerPosition.position = new Vector3(targetX, 0f, targetZ);

                    if(PlayingField[targetX, targetZ] == 'C' || PlayingField[targetX, targetZ] == 'c')
                    {
                        _collectCoin(targetX, targetZ);
                    }
                }
            }
        }
    }

    private bool _checkMoveValidity(int x, int z)
    {
        if(PlayingField[x, z] == '#')
        {
            Debug.Log("Can't move in wall space.");
            return false;
        }

        if(PlayerPosition.position.x + 2 == x && PlayerPosition.position.z + 1 == z)
        {
            return true;
        }
        else if(PlayerPosition.position.x + 2 == x && PlayerPosition.position.z - 1 == z)
        {
            return true;
        }
        else if (PlayerPosition.position.x - 2 == x && PlayerPosition.position.z + 1 == z)
        {
            return true;
        }
        else if (PlayerPosition.position.x - 2 == x && PlayerPosition.position.z - 1 == z)
        {
            return true;
        }
        else if (PlayerPosition.position.x + 1 == x && PlayerPosition.position.z + 2 == z)
        {
            return true;
        }
        else if (PlayerPosition.position.x - 1 == x && PlayerPosition.position.z + 2 == z)
        {
            return true;
        }
        else if (PlayerPosition.position.x + 1 == x && PlayerPosition.position.z - 2 == z)
        {
            return true;
        }
        else if (PlayerPosition.position.x - 1 == x && PlayerPosition.position.z - 2 == z)
        {
            return true;
        }

        return false;
    }

    private void _collectCoin(int posX, int posZ)
    {
        foreach(GameObject coin in Coins)
        {
            if(coin.transform.position.x == posX && coin.transform.position.z == posZ)
            {
                Coins.Remove(coin);
                Destroy(coin.gameObject);
                break;
            }
        }

        if(Coins.Count == 0)
        {
            //TO-DO
            //Win
            Debug.Log("Win");
        }
    }
}
