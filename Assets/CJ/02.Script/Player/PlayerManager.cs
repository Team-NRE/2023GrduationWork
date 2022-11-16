using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PlayerSetting
{
    #region List 모음
    [Header("---PlayerCard List---")]
    //플레이어 덱
    [SerializeField] List<GameObject> PlayerCardDeck = new List<GameObject>();
    //들고있는 카드 (List[0~3] = Q,W,E,R 카드 / List[4] = Next Card) 
    [SerializeField] List<GameObject> HoldCard = new List<GameObject>();
    //사용하고 난 버려진 카드 모음
    [SerializeField] List<GameObject> Discard = new List<GameObject>();
    //Q,W,E,R 카드 UI 위치 리스트
    [SerializeField] List<GameObject> SkillPosition = new List<GameObject>();


    [Header("---StoreCard List---")]
    //상점 카드들의 리스트
    [SerializeField] List<GameObject> StoreCardList = new List<GameObject>();


    [Header("---Mana List---")]
    [SerializeField] List<GameObject> ManaList = new List<GameObject>();
    #endregion


    #region 변수 
    [Header("---Use Card Info---")]
    //카드 이펙트 판별 숫자
    [SerializeField] int CardNumber;
    //카드 마나 코스트
    [SerializeField] int CardCost;


    [Header("---Player Move---")]
    //rigidbody 초기화
    [SerializeField] public Rigidbody rigidbody;
    //움직임 포인트
    Vector3 movePoint;
    //속도
    Vector3 velocity = Vector3.zero;
    //이동할 위치값
    Vector3 thisUpdatePoint;
    //회전 값
    float rotDegree;


    [Header("---Camera---")]
    //카메라 z축
    [Range(2.0f, 100.0f)]
    [SerializeField] float Cam_Z = 10.0f;
    //카메라 y축
    [Range(0.0f, 100.0f)]
    [SerializeField] float Cam_Y = 15.0f;
    [SerializeField] Vector3 MousePos;


    [Header("---Move Ignore Layer---")]
    [SerializeField] LayerMask Ignorelayer;


    [Header("---Store---")]
    //스토어 On/Off
    bool CheckStoreImg = false;


    [Header("---Purchase---")]
    //구입 버튼
    Button PurchaseButton;
    #endregion


    #region PlayerStats
    [Header("---Stats---")]
    [Header("- 공격 분야")]
    [SerializeField] float attackPower;
    [SerializeField] float absoluteAttackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalPower;

    [Header("- 방어 분야")]
    [SerializeField] float maxHealth;
    [SerializeField] float nowHealth;
    [SerializeField] float healthRegeneration;
    [SerializeField] float defensePower;

    [Header("- 버프 분야")]
    [SerializeField] float protectiveShield;
    [SerializeField] float protectiveShieldTime;

    [Header("- 디버프 분야")]
    [SerializeField] float moveSpeedReduction;
    [SerializeField] float moveSpeedReductionTime;
    [SerializeField] float skillSilenceTime;
    [SerializeField] float poisonDamage;
    [SerializeField] float poisonDamageTime;

    [Header("- 기타")]
    [SerializeField] float NowMana;
    [SerializeField] float ManaRegenerationTime;
    [SerializeField] float MaxMana;
    [SerializeField] float level;
    [SerializeField] float experience;
    [SerializeField] float moveSpeed;
    [SerializeField] float globalToken;
    [SerializeField] float rangeToken;
    [SerializeField] float resource;
    [SerializeField] float resourceRange;
    [SerializeField] float recognitionRange;
    #endregion

    //플레이어 이동
    public void PlayerMove()
    {
        if (Vector3.Distance(rigidbody.position, movePoint) >= 0.1f)
        {
            // thisUpdatePoint 는 이번 업데이트(프레임) 에서 이동할 포인트를 담는 변수
            // 이동할 방향(이동할 곳-현재 위치) 곱하기 속도를 해서 이동할 위치값을 계산
            thisUpdatePoint = (movePoint - rigidbody.position).normalized * GetStats("이동속도");

            //플레이어 이동 및 회전
            rigidbody.MovePosition(rigidbody.position + thisUpdatePoint * Time.fixedDeltaTime);
            rigidbody.MoveRotation(Quaternion.Euler(0f, rotDegree, 0f));
        }
    }

    //카메라 이동
    public void CameraMove()
    {
        MousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        
        Vector3 position_x = new Vector3(55f, rigidbody.position.y + Cam_Y, Camera.main.transform.position.z);
        Vector3 position_x_0 = new Vector3(-55f, rigidbody.position.y + Cam_Y, Camera.main.transform.position.z);
        
        Vector3 position_z = new Vector3(Camera.main.transform.position.x, rigidbody.position.y + Cam_Y, 55f);
        Vector3 position_z_0 = new Vector3(Camera.main.transform.position.x, rigidbody.position.y + Cam_Y, -55f);
        
        if(MousePos.x >= 1)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_x, Time.deltaTime);
        }

        if(MousePos.x <= 0)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_x_0, Time.deltaTime);
        }

        if(MousePos.y >= 1)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_z, Time.deltaTime);
        }

        if(MousePos.y <= 0)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position_z_0, Time.deltaTime);
        }
    }

    //고정 카메라 이동
    public void FixedCameraMove()
    {
        //Space (고정 카메라 이동)
        if (Input.GetButton("CameraFixed"))
        {
            //카메라 이동
            Vector3 pos = new Vector3(rigidbody.position.x, rigidbody.position.y + Cam_Y, rigidbody.position.z - Cam_Z);
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, pos, ref velocity, 0.25f);
        }
    }

    //키 맵핑
    public void KeyMapping()
    {
        //마우스 오른쪽 (플레이어 움직임)
        if (Input.GetMouseButtonDown(1))
        {
            // ray로 마우스 위치 world 좌표로 받기.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //광선 그려주기
            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 1f);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, ~(Ignorelayer)))
            {
                //레어와 닿은 곳이 이동 포인트
                movePoint = raycastHit.point;

                //각도
                movePoint.y = 0f;
                float dx = movePoint.x - rigidbody.position.x;
                float dz = movePoint.z - rigidbody.position.z;
                rotDegree = -(Mathf.Rad2Deg * Mathf.Atan2(dz, dx) - 90); //tan-1(dz/dx) = 각도
            }
        }

        //esc
        if (Input.GetButtonDown("Option"))
        {
            Debug.Log("옵션");
        }

        //Q
        if (Input.GetButtonDown("Q"))
        {
            UseCard(0);
        }

        //W
        if (Input.GetButtonDown("W"))
        {
            UseCard(1);
        }

        //E
        if (Input.GetButtonDown("E"))
        {
            UseCard(2);
        }

        //R
        if (Input.GetButtonDown("R"))
        {
            UseCard(3);
        }

        //TAB
        if (Input.GetButtonDown("Info"))
        {
            Debug.Log("캐릭터 정보");
        }

        //P  
        if (Input.GetButtonDown("Store"))
        {
            switch (CheckStoreImg)
            {
                case true:
                    {
                        CheckStoreImg = !CheckStoreImg;
                        ObjectSetting("Store");
                        return;
                    }

                case false:
                    {
                        CheckStoreImg = !CheckStoreImg;
                        ObjectSetting("Store");
                        return;
                    }
            }
        }

        //B
        if (Input.GetButtonDown("Home"))
        {
            Debug.Log("귀한");
        }

        //A
        if (Input.GetButtonDown("Attack"))
        {
            Debug.Log("공격");
        }
    }

    //스텟 읽기
    public float GetStats(string variableName)
    {
        if (variableName == "attackPower" || variableName == "공격력") return attackPower;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력") return absoluteAttackPower;
        if (variableName == "attackSpeed" || variableName == "공격속도") return attackSpeed;
        if (variableName == "attackRange" || variableName == "공격범위") return attackRange;
        if (variableName == "criticalChance" || variableName == "크리확률") return criticalChance;
        if (variableName == "criticalPower" || variableName == "크리공격력") return criticalPower;

        if (variableName == "maxHealth" || variableName == "최대 체력") return maxHealth;
        if (variableName == "nowHealth" || variableName == "현재 체력") return nowHealth;
        if (variableName == "healthRegeneration" || variableName == "체력재생") return healthRegeneration;
        if (variableName == "defensePower" || variableName == "방어력") return defensePower;

        if (variableName == "protectiveShield" || variableName == "방어막") return protectiveShield;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간") return protectiveShieldTime;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트") return moveSpeedReduction;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") return moveSpeedReductionTime;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간") return skillSilenceTime;
        if (variableName == "poisonDamage" || variableName == "독데미지") return poisonDamage;
        if (variableName == "poisonDamageTime" || variableName == "독시간") return poisonDamageTime;

        if (variableName == "mana" || variableName == "현재 마나") return NowMana;
        if (variableName == "ManaRegenerationTime" || variableName == "마나회복시간") return ManaRegenerationTime;
        if (variableName == "maxmana" || variableName == "최대 마나") return MaxMana;
        if (variableName == "level" || variableName == "레벨") return level;
        if (variableName == "experience" || variableName == "경험치") return experience;
        if (variableName == "moveSpeed" || variableName == "이동속도") return moveSpeed;
        if (variableName == "globalToken" || variableName == "전역골드") return globalToken;
        if (variableName == "rangeToken" || variableName == "범위골드") return rangeToken;
        if (variableName == "resource" || variableName == "자원") return resource;
        if (variableName == "resourceRange" || variableName == "획득범위") return resourceRange;
        if (variableName == "recognitionRange" || variableName == "인식범위") return recognitionRange;

        return 0;
    }

    //스텟 쓰기
    public void SetStats(string variableName, float value)
    {
        if (variableName == "attackPower" || variableName == "공격력") attackPower = value;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력") absoluteAttackPower = value;
        if (variableName == "attackSpeed" || variableName == "공격속도") attackSpeed = value;
        if (variableName == "attackRange" || variableName == "공격범위") attackRange = value;
        if (variableName == "criticalChance" || variableName == "크리확률") criticalChance = value;
        if (variableName == "criticalPower" || variableName == "크리공격력") criticalPower = value;

        if (variableName == "maxHealth" || variableName == "최대 체력") maxHealth = value;
        if (variableName == "nowHealth" || variableName == "현재 체력") nowHealth = value;
        if (variableName == "healthRegeneration" || variableName == "체력재생") healthRegeneration = value;
        if (variableName == "defensePower" || variableName == "방어력") defensePower = value;

        if (variableName == "protectiveShield" || variableName == "방어막") protectiveShield = value;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간") protectiveShieldTime = value;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트") moveSpeedReduction = value;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") moveSpeedReductionTime = value;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간") skillSilenceTime = value;
        if (variableName == "poisonDamage" || variableName == "독데미지") poisonDamage = value;
        if (variableName == "poisonDamageTime" || variableName == "독시간") poisonDamageTime = value;

        if (variableName == "mana" || variableName == "현재 마나") NowMana = value;
        if (variableName == "ManaRegenerationTime" || variableName == "마나회복시간") ManaRegenerationTime = value;
        if (variableName == "maxmana" || variableName == "최대 마나") MaxMana = value;
        if (variableName == "level" || variableName == "레벨") level = value;
        if (variableName == "experience" || variableName == "경험치") experience = value;
        if (variableName == "moveSpeed" || variableName == "이동속도") moveSpeed = value;
        if (variableName == "globalToken" || variableName == "전역골드") globalToken = value;
        if (variableName == "rangeToken" || variableName == "범위골드") rangeToken = value;
        if (variableName == "resource" || variableName == "자원") resource = value;
        if (variableName == "resourceRange" || variableName == "획득범위") resourceRange = value;
    }

    //스텟 추가 / 감소
    public void AddStats(string variableName, float value)
    {
        if (variableName == "attackPower" || variableName == "공격력") attackPower += value;
        if (variableName == "absoluteAttackPower" || variableName == "절대공격력") absoluteAttackPower += value;
        if (variableName == "attackSpeed" || variableName == "공격속도") attackSpeed += value;
        if (variableName == "attackRange" || variableName == "공격범위") attackRange += value;
        if (variableName == "criticalChance" || variableName == "크리확률") criticalChance += value;
        if (variableName == "criticalPower" || variableName == "크리공격력") criticalPower += value;

        if (variableName == "maxHealth" || variableName == "최대 체력") maxHealth += value;
        if (variableName == "nowHealth" || variableName == "현재 체력") nowHealth += value;
        if (variableName == "healthRegeneration" || variableName == "체력재생") healthRegeneration += value;
        if (variableName == "defensePower" || variableName == "방어력") defensePower += value;

        if (variableName == "protectiveShield" || variableName == "방어막") protectiveShield += value;
        if (variableName == "protectiveShieldTime" || variableName == "방어막시간") protectiveShieldTime += value;

        if (variableName == "moveSpeedReduction" || variableName == "이동속도감소퍼센트") moveSpeedReduction += value;
        if (variableName == "moveSpeedReductionTime" || variableName == "이동속도감소시간") moveSpeedReductionTime += value;
        if (variableName == "skillSilenceTime" || variableName == "스킬침묵시간") skillSilenceTime += value;
        if (variableName == "poisonDamage" || variableName == "독데미지") poisonDamage += value;
        if (variableName == "poisonDamageTime" || variableName == "독시간") poisonDamageTime += value;

        if (variableName == "mana" || variableName == "현재 마나") NowMana -= 4 * value;
        if (variableName == "ManaRegenerationTime" || variableName == "마나회복시간") ManaRegenerationTime += value;
        if (variableName == "maxmana" || variableName == "최대 마나") MaxMana += value;
        if (variableName == "level" || variableName == "레벨") level += value;
        if (variableName == "experience" || variableName == "경험치") experience += value;
        if (variableName == "moveSpeed" || variableName == "이동속도") moveSpeed += value;
        if (variableName == "globalToken" || variableName == "전역골드") globalToken += value;
        if (variableName == "rangeToken" || variableName == "범위골드") rangeToken += value;
        if (variableName == "resource" || variableName == "자원") resource += value;
        if (variableName == "resourceRange" || variableName == "획득범위") resourceRange += value;
    }

    //오브젝트 초기화
    public void ObjectSetting(string ObjectName)
    {
        Transform ObjectChild = GameObject.Find(ObjectName).transform;
        switch (ObjectName)
        {
            //구입버튼
            case "StoreImage":
                {
                    PurchaseButton = ObjectChild.GetChild(0).GetComponent<Button>();
                    PurchaseButton.onClick.AddListener(PurChase);

                    return;
                }

            //상점 이미지 On/Off
            case "Store":
                {
                    ObjectChild.GetChild(0).gameObject.SetActive(CheckStoreImg);

                    return;
                }

            //핸드 카드 List 초기 세팅
            case "PlayerCardUI":
                {
                    for (int i = 0; i < ObjectChild.childCount; i++)
                    {
                        //Plane - PlayerCard의 하위 객체 (Q,W,E,R,Next Card의 gameobject 리스트에 저장)
                        GameObject CardChild = ObjectChild.GetChild(i).gameObject;
                        SkillPosition.Add(CardChild);

                        //들고있는 카드 리스트 초기값 = null로 설정.
                        HoldCard.Add(null);
                    }

                    return;
                }

            //상점 카드 세팅
            case "Cards":
                {
                    for (int i = 0; i < ObjectChild.childCount; i++)
                    {
                        //Plane - Store - StoreImg- Cards의 하위 객체
                        GameObject child = ObjectChild.GetChild(i).gameObject;
                        StoreCardList.Add(child);
                    }

                    return;
                }

            //마나시스템 세팅
            case "Mana":
                {
                    for (int i = 0; i < MaxMana; i++)
                    {
                        GameObject ManaChild = ObjectChild.GetChild(i).gameObject;
                        ManaList.Add(ManaChild);

                        ManaList[i].GetComponent<Slider>().minValue = GetStats("마나회복시간") * i;
                        ManaList[i].GetComponent<Slider>().maxValue = GetStats("마나회복시간") * (i + 1);
                    }

                    return;
                }
        }
    }

    //카드 구입
    public void PurChase()
    {
        //상점 카드 리스트 중 Toggle.isOn이 True인 경우만 PlayerCardDeck에 추가
        for (int i = 0; i < StoreCardList.Count; i++)
        {
            Toggle toggle = StoreCardList[i].GetComponent<Toggle>();

            if (toggle.isOn == true)
            {
                Debug.Log(StoreCardList[i] + "구입");

                PlayerCardDeck.Add(StoreCardList[i].gameObject);
            }

        }
    }

    //마나 플레이
    public void ManaPlay()
    {
        //NowMana = 마나회복시간 * 최대 마나
        NowMana += Time.deltaTime;

        //마나 회복 (한칸 차면 다음 칸 참)
        for (int i = 0; i < MaxMana; i++)
        {
            ManaList[i].GetComponent<Slider>().value = Mathf.Lerp(NowMana, GetStats("마나회복시간") * (i + 1), Time.deltaTime);
        }

        //NowMana의 최대값
        if (NowMana >= GetStats("마나회복시간") * GetStats("최대 마나"))
        {
            NowMana = GetStats("마나회복시간") * GetStats("최대 마나");
        }

        //NowMana의 최소값
        else if (NowMana <= 0)
        {
            NowMana = 0;
        }
    }

    //들고있는 카드 만드는 함수
    public void CreateHoldCard(int j) //j = 0 (HoldCard[0] = Q) / j = 1 (HoldCard[1] = W) .... / j = 4 (HoldCard[4] = Next Card) 
    {
        for (int i = j; i < 5; i++)
        {
            //덱 안 카드 중 랜덤으로 불러오기
            int RandomCardNum = Random.Range(0, PlayerCardDeck.Count);

            //덱 안 랜덤 카드를 (해당 키) 들고있는 카드로 옮기기 
            HoldCard[i] = PlayerCardDeck[RandomCardNum];
            //카드 UI도 함께.......
            SkillPosition[i].GetComponent<Image>().sprite = HoldCard[i].GetComponentInChildren<Image>().sprite;

            //덱 안 카드 제거
            PlayerCardDeck.RemoveAt(RandomCardNum);
        }
    }

    //덱 안 카드가 없는 지 체크
    public void CardCheck()
    {
        //덱 안 카드가 없으면
        if (PlayerCardDeck.Count == 0)
        {
            for (int i = 0; i < Discard.Count; i++)
            {
                //덱 리스트로 버려진 카드 리셋
                PlayerCardDeck.Add(Discard[i]);
            }

            //버려진 카드 리셋
            Discard.Clear();
        }
    }

    //카드 사용하기
    public void UseCard(int Key)
    {
        FindCard(HoldCard[Key]); //해당 키의 카드 찾기

        //현재 마나 >= 카드 코스트
        if (NowMana >= CardCost * ManaRegenerationTime)
        {
            //마나 코스트 사용
            AddStats("현재 마나", CardCost);
            CardEffect(CardNumber); //해당 키의 카드 효과 발행

            Discard.Add(HoldCard[Key]); //해당 키의 카드 버리기 

            HoldCard[Key] = HoldCard[4]; //해당 키의 들고있는 카드 = Next Card
            SkillPosition[Key].GetComponent<Image>().sprite = HoldCard[Key].GetComponentInChildren<Image>().sprite; //UI도..

            HoldCard[4] = null; //Next Card 잠시 비우기
            CreateHoldCard(4); //Next Card에 랜덤카드 받기
        }

        else { Debug.Log("마나가 부족합니다."); }
    }

    //해당하는 카드 찾기
    public void FindCard(GameObject HoldCard)
    {
        //들고있는 카드가 상점 카드 리스트판별
        for (int i = 0; i < StoreCardList.Count; i++)
        {
            //들고있는 카드가 상점 카드 리스트와 같은 것을 찾으면 Swtich문으로 가면서 반복문 탈출
            if (StoreCardList[i] == HoldCard)
            {
                CardNumber = i;
                CardCost = StoreCardList[CardNumber].layer - 12;

                break;
            }
        }
    }

    //카드 효과
    public void CardEffect(int CardNumber)
    {
        //각 카드의 효과
        switch (CardNumber)
        {
            case 0:
                {
                    Debug.Log("Card_0의 효과 발동");
                    return;
                }

            case 1:
                {
                    Debug.Log("Card_1의 효과 발동");
                    return;
                }

            case 2:
                {
                    Debug.Log("Card_2의 효과 발동");
                    return;
                }

            case 3:
                {
                    Debug.Log("Card_3의 효과 발동");
                    return;
                }

            case 4:
                {
                    Debug.Log("Card_4의 효과 발동");
                    return;
                }

            case 5:
                {
                    Debug.Log("Card_5의 효과 발동");
                    return;
                }

            case 6:
                {
                    Debug.Log("Card_6의 효과 발동");
                    return;
                }

            case 7:
                {
                    Debug.Log("Card_7의 효과 발동");
                    return;
                }

            case 8:
                {
                    Debug.Log("Card_8의 효과 발동");
                    return;
                }
        }
    }
}


public class PlayerManager : MonoBehaviour
{
    public PlayerSetting m_PlayerSetting = new PlayerSetting();

    void Awake()
    {
        m_PlayerSetting.rigidbody = transform.GetComponent<Rigidbody>(); //RigidBody 초기화

        m_PlayerSetting.SetStats("마나회복시간", 4); //마나 회복 시간 4초로 설정
        m_PlayerSetting.SetStats("최대 마나", 3); //최대 마나 수정 3개
        m_PlayerSetting.SetStats("현재 마나", 0); //현재 마나 0으로 설정
        m_PlayerSetting.SetStats("이동속도", 8.0f); //이동속도 설정

        m_PlayerSetting.ObjectSetting("StoreImage"); //구입버튼
        m_PlayerSetting.ObjectSetting("PlayerCardUI"); //핸드 카드 List 초기 세팅
        m_PlayerSetting.ObjectSetting("Cards"); //상점 카드 세팅
        m_PlayerSetting.ObjectSetting("Mana"); //마나시스템 세팅
        m_PlayerSetting.ObjectSetting("Store"); //상점 이미지 On/Off

        m_PlayerSetting.CreateHoldCard(0); //들고있는 카드 초기 세팅
    }

    void Update()
    {
        m_PlayerSetting.KeyMapping(); //키 맵핑
        m_PlayerSetting.ManaPlay(); //마나 수정 플레이
        m_PlayerSetting.CardCheck(); //버려진 카드 리셋
    }

    void FixedUpdate()
    {
        m_PlayerSetting.PlayerMove(); //플레이어 움직임
    }

    void LateUpdate()
    {
        m_PlayerSetting.CameraMove(); //카메라 움직임
        m_PlayerSetting.FixedCameraMove(); //space 바 카메라 고정
    }
}
