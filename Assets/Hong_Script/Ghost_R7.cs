using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_R7 : MonoBehaviour
{

    GameObject[] players;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
         players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != 4)
            return;

        if (IsAllRun() && IsAllLeft())
        {
            transform.Translate(Vector3.left * 3f * Time.deltaTime);
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }

        else if (IsAllRun() && IsAllRight())
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (IsAllJump())
            transform.Translate(Vector3.up * 3f * Time.deltaTime);
        else if (IsAllUnder())
            transform.Translate(Vector3.down * 3f * Time.deltaTime);
    }

    

    public bool IsAllRun()
    {
        if (players.Length == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                // isRun false�� �ϳ��� ������ false ����
                if (!players[i].transform.GetComponent<PlayerScript>().isRun)
                    return false;
            }
            return true;
        }
        return false;
    }

    public bool IsAllLeft()
    {      
        if (players.Length == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                // ���� Ű�� ���� ��� True ��ȯ ������ Ű�� ������ ��� False ��ȯ
                if (!players[i].transform.GetComponent<PlayerScript>().SR.flipX)
                    return false;
            }
            return true;
        }
        return false;
    }

    public bool IsAllRight()
    {
        if (players.Length == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                // ���� Ű�� ���� ��� True ��ȯ ������ Ű�� ������ ��� False ��ȯ
                if (players[i].transform.GetComponent<PlayerScript>().SR.flipX)
                    return false;
            }
            return true;
        }
        return false;
    }

    public bool IsAllJump()
    {
        if (players.Length == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                // ���� Ű�� ���� ��� True ��ȯ ������ Ű�� ������ ��� False ��ȯ
                if (players[i].transform.GetComponent<PlayerScript>().isGround)
                    return false;
            }
            return true;
        }
        return false;
    }

    public bool IsAllUnder()
    {
        if (players.Length == 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                // ���� Ű�� ���� ��� True ��ȯ ������ Ű�� ������ ��� False ��ȯ
                if (!players[i].transform.GetComponent<PlayerScript>().isUnder)
                    return false;
            }
            return true;
        }
        return false;
    }

}
