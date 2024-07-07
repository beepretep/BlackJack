using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class DiceThrowing : MonoBehaviour
{
    public DiceRolling dicetoThrow;
    public SpeechRecognitionEngine speech;
    public int amountOfDice;
    public float throwForce;
    public float rollForce;

    private List<GameObject> _spawnedDice = new List<GameObject>();
    GameObject One, Two, Three, Four, Five;
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            One = GameObject.Find("Bid100");
            Two = GameObject.Find("Bid500");
            Three = GameObject.Find("Bid1000");
            Four = GameObject.Find("Bid2000");
            Five = GameObject.Find("Allin");
            One.SetActive(false);
            Two.SetActive(false);
            Three.SetActive(false);
            Four.SetActive(false);
            Five.SetActive(false);
            RollDice();
        }

    }
    private async Task RollDice()
    {
        if (dicetoThrow == null) return;

        foreach (var die in _spawnedDice)
        {
            Destroy(die);
        }

        for(int i = 0; i < amountOfDice; i++)
        {
            DiceRolling dice = Instantiate(dicetoThrow, transform.position, transform.rotation);
            _spawnedDice.Add(dice.gameObject);
            dice.RollDice(throwForce, rollForce, i);
            await Task.Yield();
        }
    }
}
