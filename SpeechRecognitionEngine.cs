using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Cards
{
    public GameObject[] cards;
}
public class SpeechRecognitionEngine : MonoBehaviour
{
    [SerializeField] private TMP_Text diceOneText, diceTwoText, diceThreeText;
    public string[] keywords = new string[] { };
    public List<Cards> Cards;
    public int[] CardNumber;
    public int[] CardValues;
    GameObject[] Objectlist;
    GameObject HitButton, CheckButton, Refills, One, Two, Three, Four, Five;
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float Xcoords, PlayerMoney, BetMoney, DealerXcoords, dealerRotation;
    int CardDraw, Randomize, NumberOfHits, randomDealerNum, DealerNumberOfHits;
    public int TotalNumber, dealerNum, TotalDiceNum = 0;
    public TMP_Text results;
    bool DealerAce, ace, Hitted;
    protected PhraseRecognizer recognizer;
    protected string word = "N/A";
    bool hardmode, enabledHM;
    private void Start()
    {
        enabledHM = false;
        ace = false;
        hardmode = false;
        Hitted = false;
        BetMoney = 0;
        PlayerMoney = PlayerPrefs.GetFloat("PlayerStoredMoney");
        HitButton = GameObject.Find("Hit");
        CheckButton = GameObject.Find("CheckDealerCards");
        CheckButton.SetActive(false);
        HitButton.SetActive(false);
        GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
        Xcoords = -0.119f;
        NumberOfHits = 0;
        dealerRotation = 0;
        DealerXcoords = -0.118f;
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
        if(PlayerMoney > 100)
        {
            Refills = GameObject.Find("Refill");
            Refills.SetActive(false);
        }
    }
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results.text = "Your Action: <b>" + word + "</b>";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Randomize = Random.Range(0, 52);
            CardDraw = Randomize;
            word = "N/A";
            CardRolling();
            if (dealerNum < 16)
            {
                DealerCard();
            }
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
            CheckButton.SetActive(true);
            Hitted = true;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            GameObject StartingCard = GameObject.Find("Blue_PlayingCards_Club01_00");
            Destroy(StartingCard);
            GameObject StartingCard1 = GameObject.Find("DealerCard");
            Destroy(StartingCard1);
            Randomize = Random.Range(0, 52);
            HitButton.SetActive(true);
            GameObject RevealButton = GameObject.Find("Reveal");
            RevealButton.SetActive(false);
            CardDraw = Randomize;
            word = "N/A";
            CardRolling();
            DealerCard();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (dealerNum < 16)
            {
                DealerCard();
            }
            Gameover();
        }
        switch (word)
        {
            case "Enable hard mode":
                if(enabledHM == false)
                {
                TotalNumber += 2;
                hardmode = true;
                enabledHM = true;
                }
                break;
            case "Reveal the cards":
                GameObject StartingCard = GameObject.Find("Blue_PlayingCards_Club01_00");
                Destroy(StartingCard);
                GameObject StartingCard1 = GameObject.Find("DealerCard");
                Destroy(StartingCard1);
                Randomize = Random.Range(0, 52);
                HitButton.SetActive(true);
                GameObject RevealButton = GameObject.Find("Reveal");
                RevealButton.SetActive(false);
                CardDraw = Randomize;
                word = "N/A";
                CardRolling();
                DealerCard();
                break;
            case "Hit me":
                Randomize = Random.Range(0, 52);
                if(PlayerMoney >= 5000)
                {
                    Randomize = Random.Range(0, 26);
                }
                CardDraw = Randomize;
                word = "N/A";
                CardRolling();
                if (dealerNum < 16)
                {
                    DealerCard();
                }
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
                CheckButton.SetActive(true);
                Hitted = true;
                    break;
            case "Check the cards":
                if (dealerNum < 16)
                {
                    DealerCard();
                }
                else
                    Gameover();
                PlayerPrefs.SetFloat("PlayerStoredMoney", PlayerMoney);
                break;
            case "Reload":
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
                word = "N/A";
                break;
            case "Top Up":
                TopUp();
                word = "N/A";
                break;
            case "All in":
                if (Hitted == false)
                {
                    if(PlayerMoney > 0)
                    BetMoney += PlayerMoney;
                    GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
                    PlayerMoney -= PlayerMoney;
                    GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
                    word = "N/A";
                }
                break;
            case "Bet 100":
                if (Hitted == false)
                {
                    if (PlayerMoney >= 100)
                    {
                        BetMoney += 100;
                        GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
                        PlayerMoney -= 100;
                        GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
                        word = "N/A";
                    }
                }
                break;
            case "Bet 500":
                if (Hitted == false)
                {
                    if (PlayerMoney >= 500)
                    {
                        BetMoney += 500;
                        GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
                        PlayerMoney -= 500;
                        GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
                        word = "N/A";
                    }
                }
                break;
            case "Bet 1000":
                if (Hitted == false)
                {
                    if (PlayerMoney >= 1000)
                    {
                        BetMoney += 1000;
                        GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
                        PlayerMoney -= 1000;
                        GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
                        word = "N/A";
                    }
                }
                break;
            case "Bet 2000":
                if (Hitted == false)
                {
                    if (PlayerMoney >= 2000)
                    {
                        BetMoney += 2000;
                        GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
                        PlayerMoney -= 2000;
                        GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
                        word = "N/A";
                    }
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            hardmode = true;
        }
        if (TotalNumber > 21)
        {
            Gameover();
        }
        if(dealerNum > 21)
        {
            Gameover();
        }
    }
    private void FixedUpdate()
    {
        randomDealerNum = Random.Range(0, 52);
        GameObject.Find("ScoreNumber").GetComponent<TextMeshProUGUI>().SetText(TotalNumber.ToString());
        if (Input.GetKeyDown(KeyCode.O))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
            word = "N/A";
        }
    }
    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
    public void CardRolling()
    {
        if (NumberOfHits == 1)
        {
            Xcoords = -0.034f;
        }
        else if (NumberOfHits >= 2)
        {
            Xcoords += 0.124f;
        }
        if (word == "N/A")
        {
            Debug.Log("Check");
                    Objectlist = Cards[0].cards;
                    GameObject PlayerDrawedCard = Instantiate(Objectlist[CardDraw], new Vector3(Xcoords, 0.775f, -0.452f), Quaternion.identity);
                    PlayerDrawedCard.transform.rotation = Quaternion.Euler(-36, 0, 0);
                    TotalNumber += CardValues[CardDraw];
                if(CardDraw == 0 || CardDraw == 13 || CardDraw == 26 || CardDraw == 39)
                {
                ace = true;
                if (TotalNumber < 10)
                {
                    TotalNumber += 11;
                }
                }
                NumberOfHits += 1;     
            if(ace == true && TotalNumber > 21)
            {
                TotalNumber -= 10;
                ace = false;
            }
        }
    }
    public void DealerCard() //Dealer's code
    { 
            if (DealerNumberOfHits >= 1)
            {
                DealerXcoords += 0.126f; ;
                dealerRotation = -180;
            }
            Objectlist = Cards[0].cards;
            GameObject DrawedCard = Instantiate(Objectlist[randomDealerNum], new Vector3(DealerXcoords, 0.64f, 0.207f), Quaternion.identity);
            DrawedCard.transform.rotation = Quaternion.Euler(dealerRotation, 0, 0);
            dealerNum += CardValues[randomDealerNum];
            if (CardDraw == 0 || CardDraw == 13 || CardDraw == 26 || CardDraw == 39)
            { 
                DealerAce = true;
                if (dealerNum < 10)
                {
                    dealerNum += 11;
                }
                if(dealerNum > 21)
                {
                    dealerNum += 1;
                }
            }
        DealerNumberOfHits += 1;
        if (DealerAce == true && dealerNum > 21)
        {
                dealerNum -= 10;
                DealerAce = false;
        }
    }

    public void Gameover()
    {
        if (TotalNumber == 21 || TotalNumber> dealerNum && TotalNumber <22 || dealerNum > 21)
        {
            GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>().SetText("You win!".ToString());
            GameObject.Find("DealerEndHand").GetComponent<TextMeshProUGUI>().SetText("Dealer's hand was:".ToString());
            GameObject.Find("DealerEndHandNum").GetComponent<TextMeshProUGUI>().SetText(dealerNum.ToString());
            if(hardmode == true)
            {
                PlayerMoney = BetMoney * 4;
            }
            else
            {
                PlayerMoney = BetMoney * 2;
            }
            PlayerPrefs.SetFloat("PlayerStoredMoney", PlayerMoney);
        }
        if (TotalNumber > 21 || TotalNumber <= dealerNum && dealerNum < 22)
        {
            GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>().SetText("You Lost, try again!".ToString());
            GameObject.Find("DealerEndHand").GetComponent<TextMeshProUGUI>().SetText("Dealer's hand was:".ToString());
            GameObject.Find("DealerEndHandNum").GetComponent<TextMeshProUGUI>().SetText(dealerNum.ToString());
            BetMoney = 0;
            PlayerPrefs.SetFloat("PlayerStoredMoney", PlayerMoney);
        }
    }
    public void ButtonHit()
    {
        Randomize = Random.Range(0, 52);
        CardDraw = Randomize;
        word = "N/A";
        CardRolling();
        if (dealerNum < 16)
        {
            DealerCard();
        }
        else
        {
            Gameover();
        }
        if (TotalNumber == 21)
        {
            GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>().SetText("You win!".ToString());
            GameObject.Find("DealerEndHand").GetComponent<TextMeshProUGUI>().SetText("Dealer's hand was:".ToString());
            GameObject.Find("DealerEndHandNum").GetComponent<TextMeshProUGUI>().SetText(dealerNum.ToString());
            PlayerMoney = BetMoney * 2;
            PlayerPrefs.SetFloat("PlayerStoredMoney", PlayerMoney);
        }
        if (TotalNumber > 21)
        {
            GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>().SetText("You Lost, try again!".ToString());
            GameObject.Find("DealerEndHand").GetComponent<TextMeshProUGUI>().SetText("Dealer's hand was:".ToString());
            GameObject.Find("DealerEndHandNum").GetComponent<TextMeshProUGUI>().SetText(dealerNum.ToString());
            BetMoney = 0;
            PlayerPrefs.SetFloat("PlayerStoredMoney", PlayerMoney);
        }
        One = GameObject.Find("Bid100");
        Two = GameObject.Find("Bid500");
        Three = GameObject.Find("Bid1000");
        Four = GameObject.Find("Bid2000");
        Five = GameObject.Find("Allin");
        CheckButton.SetActive(true);
        One.SetActive(false);
        Two.SetActive(false);
        Three.SetActive(false);
        Four.SetActive(false);
        Five.SetActive(false);
    }
    public void ButtonReveal()
    {
        CheckButton.SetActive(true);
        HitButton.SetActive(true);
        GameObject StartingCard = GameObject.Find("Blue_PlayingCards_Club01_00");
        Destroy(StartingCard);
        GameObject StartingCard1 = GameObject.Find("DealerCard");
        Destroy(StartingCard1);
        Randomize = Random.Range(0, 52);
        CardDraw = Randomize;
        word = "N/A";
        CardRolling();
        DealerCard();
        GameObject RevealButton = GameObject.Find("Reveal");
        RevealButton.SetActive(false);
    }
    public void ButtonCheck()
    {
        if (dealerNum < 16)
        {
            DealerCard();
        }
        else
          Gameover();
    }
    public void Bid100()
    {
        if (PlayerMoney >= 100)
        {
            BetMoney += 100;
            GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
            PlayerMoney -= 100;
            GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
            word = "N/A";
        }
    }
    public void Bid500()
    {
        if (PlayerMoney >= 500)
        {
            BetMoney += 500;
            GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
            PlayerMoney -= 500;
            GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
            word = "N/A";
        }
    }
    public void Bid1000()
    {
        if (PlayerMoney >= 1000)
        {
            BetMoney += 1000;
            GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
            PlayerMoney -= 1000;
            GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
            word = "N/A";
        }
    }
    public void Bid2000()
    {
        if (PlayerMoney >= 2000)
        {
            BetMoney += 2000;
            GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
            PlayerMoney -= 2000;
            GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
            word = "N/A";
        }
    }
    public void Allin()
    {
        if (PlayerMoney > 0)
            BetMoney += PlayerMoney;
        GameObject.Find("BetText").GetComponent<TextMeshProUGUI>().SetText(BetMoney.ToString());
        PlayerMoney -= PlayerMoney;
        GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
        word = "N/A";
    }
    public void TopUp()
    {
        if (PlayerMoney < 100)
        {
            PlayerMoney += 100;
            GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText(PlayerMoney.ToString());
            
        }
        Refills = GameObject.Find("Refill");
        Refills.SetActive(false);
    }
    private void OnEnable()
    {
        DiceRolling.OnDiceResult += SetText;
    }
    private void OnDisable()
    {
        DiceRolling.OnDiceResult -= SetText;
    }
    private void SetText(int diceIndex, int diceResult)
    {
        if(diceIndex == 0)
        {
            diceOneText.SetText($"Dice One rolled a {diceResult}");
            TotalDiceNum += diceResult;
        }
        else
        {
            diceTwoText.SetText($"Dice Two rolled a {diceResult}");
            TotalDiceNum += diceResult;
        }
    }
}
