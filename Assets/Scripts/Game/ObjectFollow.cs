using System.Collections;
using UnityEngine;

public class AutoMovePlayer : MonoBehaviour
{
    public float moveSpeed; // Velocidade de movimento do jogador

    public Vector3 Movement, Move;
    public bool Testwe;

    public GameObject Player;

    public void Update()
    {
        if (Testwe)
        {
            Vector3 newPosition = Player.transform.position + Movement * moveSpeed * Time.deltaTime;
            Player.transform.position = Vector2.MoveTowards(Player.transform.position, newPosition, 0.5f * Time.deltaTime);
            Player.GetComponent<PlayerMovement>().MoveInput = Move;
        }
    }

    public IEnumerator GoBack()
    {
        yield return new WaitForSeconds(0.1f);        
        // Movimento automático do jogador
        Testwe = true;
        yield return new WaitForSeconds(0.5f);
        Testwe = false;
        Player.GetComponent<PlayerMovement>().MoveInput = Move * 0;
    }

    public void Test()
    {
        StartCoroutine(GoBack());
    }
}

