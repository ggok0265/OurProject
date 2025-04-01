using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager; // ���ӸŴ��� ����
    public Inventory playerInventory; // �÷��̾� �κ��丮 ����
    public MiniGameManager miniGameManager; // �̴ϰ��ӸŴ��� ����
    public DataBase dataBase; // ���̺�ε� ����

    // <UI GameObject ����>
    public GameObject playerSightUI; // �÷��̾� �þ� ������
    public GameObject inventoryUI; // �κ��丮 ������

    public GameObject inventoryIcon; // �κ��丮 ������
    public GameObject flashlightIcon; // �ķ��� ������

    public GameObject bloodEffect; // �� Ƣ��� ��

    public GameObject menuUI; // �޴� ������
    public GameObject cutSceneUI; // �ƽ� ������
    public GameObject paperReadingUI; // ���� ���� �ߴ� ������
    public GameObject paperReadingUI1;
    public Text guideText; // �ȳ� ����
    public Text actionGuideText; // �׼� ���̵� ����
    public Text questText; // ����Ʈ �ؽ�Ʈ
    private float actionGuideTextTime;



    // <Ȱ��ȭ ����>
    public bool isOnPlayerSightUI;
    public bool isOnInventoryUI;
    public bool isOnMenuUI;
    public bool isOnCutSceneUI;
    public bool isOnPaperUI;
    public bool isOnExam;
    public bool isOnPassKeyPad;

    public bool buttonDelay; // ��ư �ߺ��Է� ����

    // <Item Images>
    public Image[] itemImage = new Image[10];
    public Text[] itemAmount = new Text[10];

    public Sprite[] itemImageSprite = new Sprite[10];
    public Sprite[] paperImageSprite = new Sprite[10];

    public GameObject newInvenUI;
    public Button[] itemButton = new Button[10];
    public Button[] paperButton = new Button[10];
    public GameObject inventext;
    public GameObject clickImage;
    public GameObject useButton;
    public int wannaUseItem; // �������� n, ������ 10+n���� ����

    public Text itemCapacity;
    public Text paperCapacity;

    public GameObject saveBtn;
    public GameObject loadBtn;

    public int activeArrayNum;
    int maxArrayNum; // ���� �� ������
    public GameObject[] paper1;
    public Button secretPaper;
    public bool foundSecretPaper;

    // <���� �迭>
    // �÷��̾� �̷¼� (������) 1
    public Sprite[] profile;

    // �������� �ϱ� (������) 2
    public Sprite[] scientistsDiary;

    // ���� ����(�繫��) ���� ���� ��� ���� 5
    public Sprite[] redPaper;
    public GameObject redKey;
    public GameObject redKeyImage;
    public bool foundRedKey;

    // ���� ������ ���� 3
    public Sprite[] eduPaper;

    // �������� (���� ������) ���� ���� 4
    public Sprite[] textImage13; 
    public GameObject examination;


    // Ű�е� ����
    public GameObject passwordKeyPad;
    public Text[] passNum = new Text[4];
    public int currentPassOrder;
    public bool isPassCheck;
    public GameObject archiveDoor;
    public GameObject safeKeyPad;

    public GameObject optionBtn;
    public GameObject optionSlider;

    void Start()
    {
        setImageAndAmount();
    }

    void Update()
    {
        onInventoryUI();
        offInventoryUI();
        onMenuUI();
        offMenuUI();
        offExam();
        actionGuideTextManage();
        offPassKeyPad();
        setItemCapacity();
    }

    // Inventory UI
    public void onInventoryUI() // �κ��丮 UI Ȱ��ȭ
    {
        if(!isOnInventoryUI && 
            (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
            && !buttonDelay && !isOnMenuUI && !gameManager.nowCutScene && !miniGameManager.isLockPicking && !isOnPaperUI && !isOnExam && !isOnPassKeyPad)
        {
            actionGuideTextTime = 0;
            offQuestText();
            offGuideText();
            Time.timeScale = 0;
            StartCoroutine("delayTimer");
            newInvenUI.SetActive(true);
            playerSightUI.SetActive(false);
            isOnInventoryUI = true;
            gameManager.mouseControlForUI();
        }
    }
    public void offInventoryUI() // �κ��丮 UI ��Ȱ��ȭ
    {
        if (isOnInventoryUI && (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)) && !buttonDelay && !isOnMenuUI && !isOnPaperUI && !isOnPassKeyPad)
        {
            Time.timeScale = 1;
            StartCoroutine("delayTimer");
            newInvenUI.SetActive(false);
            playerSightUI.SetActive(true);
            onQuestText();
            isOnInventoryUI = false;
            clickImage.GetComponent<Image>().sprite = itemImageSprite[0];
            inventext.GetComponent<Text>().text = "";
            useButton.SetActive(false);
            wannaUseItem = 0;
            gameManager.mouseControlForPlayer();
        }
    }

    public void setImageAndAmount() // �κ��丮 Image �� ���� ����
    {
        for(int i = 0 ; i < 10 ; i++)
        {
            itemImage[i].sprite = itemImageSprite[playerInventory.iteminventory[i]];
            if (playerInventory.itemamount[i] > 1)
            {
                itemAmount[i].text = playerInventory.itemamount[i].ToString();
            }
            else
                itemAmount[i].text = " ";
        }
    }

    public void setItemButton()
    {
        for (int i = 0; i < 10; i++)
        {
            itemButton[i].GetComponentInChildren<Image>().sprite = itemImageSprite[playerInventory.iteminventory[i]];
            itemButton[i].GetComponentInChildren<Text>().text = playerInventory.iteminventory[i].ToString();
        } 
    }
    public void setPaperButton()
    {
        for (int i = 0; i < 10; i++)
        {
            paperButton[i].GetComponentInChildren<Image>().sprite = paperImageSprite[playerInventory.paperinventory[i]];
            paperButton[i].GetComponentInChildren<Text>().text = playerInventory.paperinventory[i].ToString();
        }
    }

    // Menu UI
    public void onMenuUI() // �޴� UI Ȱ��ȭ
    {
        if (!isOnMenuUI && !isOnInventoryUI && Input.GetKeyDown(KeyCode.Escape) && !buttonDelay && !gameManager.nowCutScene && !miniGameManager.isLockPicking && !isOnPaperUI && !isOnExam && !isOnPassKeyPad)
        {
            actionGuideTextTime = 0;
            offGuideText();
            Time.timeScale = 0;
            StartCoroutine("delayTimer");
            menuUI.SetActive(true);
            isOnMenuUI = true;
            gameManager.mouseControlForUI();
        }
    }
    public void offMenuUI() // �޴� UI ��Ȱ��ȭ
    {
        if (isOnMenuUI && Input.GetKeyDown(KeyCode.Escape) && !buttonDelay)
        {
            //saveBtn.SetActive(true);
            //loadBtn.SetActive(true);
            //optionBtn.SetActive(true);
            //optionSlider.SetActive(false);
            Time.timeScale = 1;
            StartCoroutine("delayTimer");
            menuUI.SetActive(false);
            isOnMenuUI = false;
            gameManager.mouseControlForPlayer();
        }
    }

    public void onCutSceneUI() // �ƽ� UI Ȱ��ȭ
    {
        actionGuideTextTime = 0;
        offGuideText();
        cutSceneUI.SetActive(true);
        isOnCutSceneUI = true;

        playerSightUI.SetActive(false);
        isOnPlayerSightUI = false;
        offQuestText();

        inventoryUI.SetActive(false);
        isOnInventoryUI = false;
    }
    public void offCutSceneUI() // �ƽ� UI ��Ȱ��ȭ
    {
        actionGuideTextTime = 0;
        offGuideText();
        cutSceneUI.SetActive(false);
        isOnCutSceneUI = false;


        playerSightUI.SetActive(true);
        onQuestText();
        isOnPlayerSightUI = true;
    }
    
    public void onGuideText(string text) // �ȳ����� ���
    {
        StopCoroutine(FadeTextToZero());
        guideText.text = text;
        StartCoroutine(FadeTextToZero());
    }
    public IEnumerator FadeTextToZero()  // ���̵� �ؽ�Ʈ ���İ� 1���� 0���� ��ȯ
    {
        guideText.color = new Color(guideText.color.r, guideText.color.g, guideText.color.b, 1);
        while (guideText.color.a > 0.0f)
        {
            guideText.color = new Color(guideText.color.r, guideText.color.g, guideText.color.b, guideText.color.a - (Time.unscaledDeltaTime / 2f));
            yield return null;
        }
    }
    public void offGuideText()
    {
        StopCoroutine(FadeTextToZero());
        guideText.color = new Color(guideText.color.r, guideText.color.g, guideText.color.b, 0);
    }

    public void onActionGuideText(string text)
    {
        actionGuideText.gameObject.SetActive(true);
        actionGuideTextTime = 0.15f;
        actionGuideText.text = text;
    }
    public void actionGuideTextManage()
    {
        if(actionGuideTextTime > 0)
        {
            actionGuideTextTime -= Time.unscaledDeltaTime;
        }
        else
        {
            actionGuideText.gameObject.SetActive(false);
        }
    }

    public void onQuestText()
    {
        questText.gameObject.SetActive(true);
    }

    public void setQuestText(string text)
    {
        questText.text = text;
    }

    public void offQuestText()
    {
        questText.gameObject.SetActive(false);
    }

    IEnumerator delayTimer() // ��ư �ߺ��Է� ���� �ڷ�ƾ
    {
        buttonDelay = true;
        yield return new WaitForSecondsRealtime(0.05f);
        buttonDelay = false;
    }
   
    public void setItemCapacity()
    {
        if (isOnInventoryUI)
        {
            itemCapacity.GetComponent<Text>().text = playerInventory.itemcurrentCapacity + " / 10";
            paperCapacity.GetComponent<Text>().text = playerInventory.papercurrentCapacity + " / 10";
        }
    }
    public void itemInventoryClick()
    {
        int _itemcode = int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        if (_itemcode == 0) useButton.SetActive(false);
        else useButton.SetActive(true);
        useButton.GetComponentInChildren<Text>().text = "Use";
        switch (_itemcode) {
            case 1: // �����
                inventext.GetComponent<Text>().text = "-�������� �����-\n �̰ɷ� ���� �� �� ���� �� ����.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 2: // ����� ����
                inventext.GetComponent<Text>().text = "-����� ����-";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 3: // ����
                inventext.GetComponent<Text>().text = "-����-\n ������ �ڹ���� �̰ɷ� �� �� ���� �� ����.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 4: // ����Ű
                inventext.GetComponent<Text>().text = "Red Key";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 5: // ���� ������ ����
                inventext.GetComponent<Text>().text = "-���� ������ ����-";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 6: // ����
                inventext.GetComponent<Text>().text = "-���� ������ ����-\n��ĳ�ʴ� �� ������ �ν��Ѵٰ� �Ѵ�.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 7: // �� ����
                inventext.GetComponent<Text>().text = "-���� ����-\n������ ���� �� �ִ� ���ȹ���";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 8: // ���� ����
                inventext.GetComponent<Text>().text = "-������ ���� ���ȹ���-\n��ĳ�ʿ� ������ ���ȿ��踦 ȹ���� �� �ִ� �����̴�.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 9: // ���Ű
                inventext.GetComponent<Text>().text = "Blue Key";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 10: // �׸�Ű
                inventext.GetComponent<Text>().text = "Green Key";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
                // case 8: // ��й���(���й���)
                //     inventext.GetComponent<Text>().text = "8";
                //     clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                //     wannaUseItem = _itemcode;
                //     break;
        }
    }
    public void paperInventoryClick()
    {
        int _itemcode = int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        if (_itemcode == 0) 
            useButton.SetActive(false);
        else 
            useButton.SetActive(true);
        
        useButton.GetComponentInChildren<Text>().text = "Read";
        switch (_itemcode) 
        {
            case 1: // �÷��̾� �̷¼�
                inventext.GetComponent<Text>().text = "-����ü�� �̷¼�-\n ���� �����, ����ü�� �Ż��� �����ִ�.";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[1]; // ���� ������ 
                wannaUseItem = 20 + _itemcode;
                break;
            case 2: // ������ �ϱ�
                inventext.GetComponent<Text>().text = "-�������� �ϱ�-\n";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[2]; // ���� ������ 
                wannaUseItem = 20 + _itemcode;
                break;
            case 3: // ������
                inventext.GetComponent<Text>().text = "-���� ������ ����-\n";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[3]; // ���� ������ 
                wannaUseItem = 20 + _itemcode;
                break;
            case 4: // ���� ���� < ���� ����
                inventext.GetComponent<Text>().text = "-���� ���� ����-\n";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[4]; // ���� ������ 
                wannaUseItem = 20 + _itemcode;
                break;
            case 5: // ����Ű �ӽ��ڵ�
                inventext.GetComponent<Text>().text = "-���� ����-\n ���� ������. ��������, ������ ���̿� ���� �������ִ�."; 
                clickImage.GetComponent<Image>().sprite = paperImageSprite[5]; // ���� ������ 
                wannaUseItem = 20 + _itemcode;
                break;
            case 6: // ���й���
                inventext.GetComponent<Text>().text = "-������ ����-\n �ȿ��� � ������ �����ִ�.";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[6]; // ���� ������ 
                wannaUseItem = 20 + _itemcode;
                break;
        }
    }

    public void buttonClick()
    {
        switch (wannaUseItem)
        {
            case 1:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 3:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 4:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 5:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 6:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 7:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 8:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 9:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
            case 10:
                ButtonClickInCase();
                playerInventory.useItem(wannaUseItem);
                wannaUseItem = 0;
                break;
        }
    }

    public void backToTitleButton()
    {
        SystemControl.GoToTitleScene();
    }
    private void ButtonClickInCase()
    {
        Time.timeScale = 1;
        StartCoroutine("delayTimer");
        newInvenUI.SetActive(false);
        playerSightUI.SetActive(true);
        isOnInventoryUI = false;
        clickImage.GetComponent<Image>().sprite = itemImageSprite[0];
        inventext.GetComponent<Text>().text = "";
        useButton.SetActive(false);
        gameManager.mouseControlForPlayer();
    }

    public void paperReadBtnClick()
    {
        if(useButton.GetComponentInChildren<Text>().text == "Read")
        {
            newInvenUI.SetActive(false);
            playerSightUI.SetActive(true);
            
            isOnPaperUI = true;
            isOnInventoryUI = false;
            clickImage.GetComponent<Image>().sprite = itemImageSprite[0];
            inventext.GetComponent<Text>().text = "";
            
            useButton.SetActive(false);
            
            if(wannaUseItem != 26)
            {
                paperReadingUI1.SetActive(true);
            }
            secretPaper.gameObject.SetActive(false);
            activeArrayNum = 0;
            switch (wannaUseItem)
            {
                case 21:
                    paperReadingUI1.GetComponentInChildren<Image>().sprite = profile[activeArrayNum];
                    maxArrayNum = profile.Length;
                    break;
                case 22:
                    paperReadingUI1.GetComponentInChildren<Image>().sprite = scientistsDiary[activeArrayNum];
                    maxArrayNum = scientistsDiary.Length;
                    break;
                case 23:
                    paperReadingUI1.GetComponentInChildren<Image>().sprite = eduPaper[activeArrayNum];
                    maxArrayNum = eduPaper.Length;
                    break;
                case 24:
                    paperReadingUI1.GetComponentInChildren<Image>().sprite = textImage13[activeArrayNum];
                    maxArrayNum = textImage13.Length;
                    break;
                case 25: // ���� ���� ��� ������ �ӽ� �ڵ�
                    paperReadingUI1.GetComponentInChildren<Image>().sprite = redPaper[activeArrayNum];
                    maxArrayNum = redPaper.Length;
                    if(!foundRedKey)
                        redKeyImage.gameObject.SetActive(true);
                    break;
                case 26: //
                    onExam();
                    break;
            }
        }
    }

    public void paperLeftBtnClick()
    {
        if (activeArrayNum <= 0) return;
        else
        {
            activeArrayNum--;
            ChangePaperText();
        }
    }
    public void paperRightBtnClick()
    {
        if (activeArrayNum >= maxArrayNum - 1) return;
        else
        {
            activeArrayNum++;
            ChangePaperText();
        }
    }
    public void paperCloseBtnClick()
    {
        Time.timeScale = 1;
        StartCoroutine("delayTimer");
        wannaUseItem = 0;
        redKeyImage.gameObject.SetActive(false);
        paperReadingUI1.SetActive(false);
        redKey.gameObject.SetActive(false);
        isOnPaperUI = false;
        gameManager.mouseControlForPlayer();
    }

    public void ChangePaperText()
    {
        gameObject.GetComponent<SoundManager>().PaperSound();
        switch (wannaUseItem)
        {
            case 21:
                paperReadingUI1.GetComponentInChildren<Image>().sprite = profile[activeArrayNum];
                break;
            case 22:
                paperReadingUI1.GetComponentInChildren<Image>().sprite = scientistsDiary[activeArrayNum];
                break;
            case 23:
                paperReadingUI1.GetComponentInChildren<Image>().sprite = eduPaper[activeArrayNum];
                break;
            case 24:
                paperReadingUI1.GetComponentInChildren<Image>().sprite = textImage13[activeArrayNum];
                if(!foundSecretPaper && activeArrayNum == 1)
                {
                    secretPaper.gameObject.SetActive(true);
                }
                else
                {
                    secretPaper.gameObject.SetActive(false);
                }
                break;
            case 25: // redPaper �ӽ� �ڵ�
                paperReadingUI1.GetComponentInChildren<Image>().sprite = redPaper[activeArrayNum];
                if (activeArrayNum == 3 && !foundRedKey)
                {
                    redKey.gameObject.SetActive(true);
                    redKeyImage.gameObject.SetActive(false);
                }
                else if(!foundRedKey)
                {
                    redKey.gameObject.SetActive(false);
                    redKeyImage.gameObject.SetActive(true);
                }
                else if(foundRedKey)
                {
                    redKey.gameObject.SetActive(false);
                    redKeyImage.gameObject.SetActive(false);
                }
                break;
        }
    }


    public void ClickSecretPaperBtn() // ���й��� ��ư ����
    {
        secretPaper.gameObject.SetActive(false);
        foundSecretPaper = true;
        playerInventory.addItem(26);
        gameObject.GetComponent<SoundManager>().GetItemSound();        
    }

    public void ClickRedKeyPaperBtn()
    {
        foundRedKey = true;
        redKey.gameObject.SetActive(false);
        playerInventory.addItem(4);
        gameObject.GetComponent<SoundManager>().GetItemSound();
    }

    // <���й���>
    public void onExam()
    {
        actionGuideTextTime = 0;
        offGuideText();
        Time.timeScale = 0;
        StartCoroutine("delayTimer");
        examination.SetActive(true);
        isOnExam = true;
        gameManager.mouseControlForUI();
    }
    public void offExam()
    {
        if (isOnExam && Input.GetKeyDown(KeyCode.Escape))
        {
            examination.SetActive(false);
            isOnExam = false;
            Time.timeScale = 1;
            StartCoroutine("delayTimer");
            paperCloseBtnClick();
            gameManager.mouseControlForPlayer();
        }
    }

    // <Ű�е�>
    public void onPassKeypad()
    {
        Time.timeScale = 0;
        isOnPassKeyPad = true;
        StartCoroutine("delayTimer");
        offGuideText();
        passwordKeyPad.SetActive(true);
        gameManager.mouseControlForUI();
    }
    public void offPassKeyPad()
    {
        if (isOnPassKeyPad && !isOnMenuUI && !isOnInventoryUI && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Escape) && !buttonDelay && !gameManager.nowCutScene && !miniGameManager.isLockPicking && !isOnPaperUI && !isOnExam)
        {
            Time.timeScale = 1;
            StartCoroutine("delayTimer");
            passwordKeyPad.SetActive(false);
            isOnPassKeyPad = false;
            gameManager.mouseControlForPlayer();
        }
    }
    public void passNumBtnClick()
    {
        gameObject.GetComponent<SoundManager>().KeyPadButton();
        int num = int.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);

        for(int i = 0; i < 4; i++)
        {
            if(passNum[i].GetComponent<Text>().text == "")
            {
                passNum[i].GetComponent<Text>().text = num.ToString();
                currentPassOrder++;
                break;
            }
        }
    }
    public void passBackspaceKeyClick()
    {
        gameObject.GetComponent<SoundManager>().KeyPadButton();
        if (passNum[0].GetComponent<Text>().text == "") return;
        else
        {
            passNum[currentPassOrder].GetComponent<Text>().text = "";
            currentPassOrder--;
        }
    }
    public void passEnterKeyClick()
    {
        string pass;
        pass = passNum[0].GetComponent<Text>().text + passNum[1].GetComponent<Text>().text + passNum[2].GetComponent<Text>().text + passNum[3].GetComponent<Text>().text;

        if(pass == "6088") // ��й�ȣ
        {
            Time.timeScale = 1;
            StartCoroutine("delayTimer");
            wannaUseItem = 0;
            isPassCheck = true;
            isOnPassKeyPad = false;
            safeKeyPad.GetComponent<BoxOpen>().isLocked = false;
            passwordKeyPad.SetActive(false);
            gameManager.missionCheck[7] = true;
            gameManager.mouseControlForPlayer();
            gameObject.GetComponent<SoundManager>().KeyPadSuccess();
            gameManager.database.SaveBtn(); // ����
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                passNum[i].GetComponent<Text>().text = "";
            }
            currentPassOrder = -1;
            gameObject.GetComponent<SoundManager>().KeyPadDenial();
        }
    }

    public void menuOptionBtn()
    {
        saveBtn.SetActive(false);
        loadBtn.SetActive(false);
        optionBtn.SetActive(false);
        optionSlider.SetActive(true);
    }

    public void LoadData()
    {
        offCutSceneUI();
        bloodEffect.SetActive(false);
        setItemButton();
        setPaperButton();
        setImageAndAmount();
    }
}
