using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Events;
using System.Threading.Tasks;

[RequireComponent(typeof(Rigidbody))]
public class DiceRolling : MonoBehaviour
{
    public Transform[] diceFaces;
    public Rigidbody rb;
    private int diceIndex = 1;
    private bool stoppedRolling, delayFinished;
    public static UnityAction<int, int> OnDiceResult;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!delayFinished) return;
        if(!stoppedRolling && rb.velocity.sqrMagnitude == 0f)
        {
            stoppedRolling = true;
            GetNumOnTopFace();
        }
    }
    [ContextMenu("Get Top Face")]
    private int GetNumOnTopFace()
    {
        if (diceFaces == null) return -1;
        var topFace = 0;
        var lastYPos = diceFaces[0].position.y;

        for (int i =0; i< diceFaces.Length; i++)
        {
            if(diceFaces[i].position.y > lastYPos)
            {
                lastYPos = diceFaces[i].position.y;
                topFace = i;
            }
        }
        Debug.Log($"Dice result {topFace + 1}");
        OnDiceResult?.Invoke(diceIndex, topFace + 1);
        return topFace + 1;
    }
    public void RollDice(float throwForce, float rollForce, int i)
    {
        diceIndex = i;
        var randomVariance = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce + randomVariance), ForceMode.Impulse);

        var randX = Random.Range(0f, 1f);
        var randY = Random.Range(0f, 1f);
        var randZ = Random.Range(0f, 1f);

        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollForce + randomVariance), ForceMode.Impulse);
        _ = Delayresult();
    }

    private async Task Delayresult()
    {
        await Task.Delay(1000);
        delayFinished = true;
    }
}
