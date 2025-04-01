using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager; // 게임매니저 선언
    public Inventory playerInventory; // 플레이어 인벤토리 선언
    public MiniGameManager miniGameManager; // 미니게임매니저 선언
    public DataBase dataBase; // 세이브로드 선언

    // <UI GameObject 선언>
    public GameObject playerSightUI; // 플레이어 시야 유아인
    public GameObject inventoryUI; // 인벤토리 유아인

    public GameObject inventoryIcon; // 인벤토리 아이콘
    public GameObject flashlightIcon; // 후레쉬 아이콘

    public GameObject bloodEffect; // 피 튀기는 거

    public GameObject menuUI; // 메뉴 유아인
    public GameObject cutSceneUI; // 컷신 유아인
    public GameObject paperReadingUI; // 문서 사용시 뜨는 유아인
    public GameObject paperReadingUI1;
    public Text guideText; // 안내 문구
    public Text actionGuideText; // 액션 가이드 문구
    public Text questText; // 퀘스트 텍스트
    private float actionGuideTextTime;



    // <활성화 여부>
    public bool isOnPlayerSightUI;
    public bool isOnInventoryUI;
    public bool isOnMenuUI;
    public bool isOnCutSceneUI;
    public bool isOnPaperUI;
    public bool isOnExam;
    public bool isOnPassKeyPad;

    public bool buttonDelay; // 버튼 중복입력 방지

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
    public int wannaUseItem; // 아이템은 n, 문서는 10+n으로 설정

    public Text itemCapacity;
    public Text paperCapacity;

    public GameObject saveBtn;
    public GameObject loadBtn;

    public int activeArrayNum;
    int maxArrayNum; // 서류 끝 페이지
    public GameObject[] paper1;
    public Button secretPaper;
    public bool foundSecretPaper;

    // <서류 배열>
    // 플레이어 이력서 (연구실) 1
    public Sprite[] profile;

    // 과학자의 일기 (연구실) 2
    public Sprite[] scientistsDiary;

    // 연구 일지(사무실) 빨간 열쇠 얻는 문서 5
    public Sprite[] redPaper;
    public GameObject redKey;
    public GameObject redKeyImage;
    public bool foundRedKey;

    // 직원 교육용 문서 3
    public Sprite[] eduPaper;

    // 괴물문서 (문서 보관실) 수학 쪽지 4
    public Sprite[] textImage13; 
    public GameObject examination;


    // 키패드 관련
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
    public void onInventoryUI() // 인벤토리 UI 활성화
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
    public void offInventoryUI() // 인벤토리 UI 비활성화
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

    public void setImageAndAmount() // 인벤토리 Image 및 수량 셋팅
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
    public void onMenuUI() // 메뉴 UI 활성화
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
    public void offMenuUI() // 메뉴 UI 비활성화
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

    public void onCutSceneUI() // 컷신 UI 활성화
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
    public void offCutSceneUI() // 컷신 UI 비활성화
    {
        actionGuideTextTime = 0;
        offGuideText();
        cutSceneUI.SetActive(false);
        isOnCutSceneUI = false;


        playerSightUI.SetActive(true);
        onQuestText();
        isOnPlayerSightUI = true;
    }
    
    public void onGuideText(string text) // 안내문구 출력
    {
        StopCoroutine(FadeTextToZero());
        guideText.text = text;
        StartCoroutine(FadeTextToZero());
    }
    public IEnumerator FadeTextToZero()  // 가이드 텍스트 알파값 1에서 0으로 전환
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

    IEnumerator delayTimer() // 버튼 중복입력 방지 코루틴
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
            case 1: // 사원증
                inventext.GetComponent<Text>().text = "-누군가의 사원증-\n 이걸로 문을 열 수 있을 것 같다.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 2: // 탕비실 열쇠
                inventext.GetComponent<Text>().text = "-탕비실 열쇠-";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 3: // 락픽
                inventext.GetComponent<Text>().text = "-락픽-\n 간단한 자물쇠는 이걸로 열 수 있을 것 같다.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 4: // 레드키
                inventext.GetComponent<Text>().text = "Red Key";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 5: // 문서 보관실 열쇠
                inventext.GetComponent<Text>().text = "-문서 보관실 열쇠-";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 6: // 도장
                inventext.GetComponent<Text>().text = "-보안 문서용 도장-\n스캐너는 이 도장을 인식한다고 한다.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 7: // 빈 서류
                inventext.GetComponent<Text>().text = "-보안 문서-\n도장을 찍을 수 있는 보안문서";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 8: // 보안 문서
                inventext.GetComponent<Text>().text = "-도장이 찍힌 보안문서-\n스캐너에 넣으면 보안열쇠를 획득할 수 있는 문서이다.";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 9: // 블루키
                inventext.GetComponent<Text>().text = "Blue Key";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
            case 10: // 그린키
                inventext.GetComponent<Text>().text = "Green Key";
                clickImage.GetComponent<Image>().sprite = itemImageSprite[_itemcode];
                wannaUseItem = _itemcode;
                break;
                // case 8: // 비밀문서(수학문제)
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
            case 1: // 플레이어 이력서
                inventext.GetComponent<Text>().text = "-실험체의 이력서-\n 나를 비롯한, 실험체의 신상이 적혀있다.";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[1]; // 서류 아이콘 
                wannaUseItem = 20 + _itemcode;
                break;
            case 2: // 과학자 일기
                inventext.GetComponent<Text>().text = "-과학자의 일기-\n";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[2]; // 서류 아이콘 
                wannaUseItem = 20 + _itemcode;
                break;
            case 3: // 교육용
                inventext.GetComponent<Text>().text = "-직원 교육용 문서-\n";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[3]; // 서류 아이콘 
                wannaUseItem = 20 + _itemcode;
                break;
            case 4: // 괴물 문서 < 수학 쪽지
                inventext.GetComponent<Text>().text = "-괴물 정보 문서-\n";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[4]; // 서류 아이콘 
                wannaUseItem = 20 + _itemcode;
                break;
            case 5: // 레드키 임시코드
                inventext.GetComponent<Text>().text = "-연구 일지-\n 연구 일지다. 만져보니, 페이지 사이에 무언가 끼워져있다."; 
                clickImage.GetComponent<Image>().sprite = paperImageSprite[5]; // 서류 아이콘 
                wannaUseItem = 20 + _itemcode;
                break;
            case 6: // 수학문제
                inventext.GetComponent<Text>().text = "-접혀진 쪽지-\n 안에는 어떤 문제가 적혀있다.";
                clickImage.GetComponent<Image>().sprite = paperImageSprite[6]; // 서류 아이콘 
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
                case 25: // 빨간 열쇠 얻는 문서의 임시 코드
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
            case 25: // redPaper 임시 코드
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


    public void ClickSecretPaperBtn() // 수학문제 버튼 누름
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

    // <수학문제>
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

    // <키패드>
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

        if(pass == "6088") // 비밀번호
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
            gameManager.database.SaveBtn(); // 저장
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
