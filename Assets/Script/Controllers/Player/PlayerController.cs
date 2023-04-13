using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Enums;

public class PlayerController : BaseController
{
    [Header("---Instance---")]
    public static PlayerController Player_Instance;
    public PlayerMove player_move;
    public PlayerAttack player_att;
    public PlayerKey player_key;
    //public PlayerBullet player_bullet;
    public PlayerStats player_stats;

    [Header("---.etc---")]
    //상태 참조
    public Status status;

    //초기화
    public NavMeshAgent agent;
    public Animator animator;
    public new Transform transform;

    //bool
    bool isDie = false; //플레이어 사망 여부
	bool _used = false;  //Announce GetDeck is first or not

	//PlayerInHandCard
	List<BaseController> _inHand = new List<BaseController>();
    //PlayerDeckBase
    List<string> _baseDeck = new List<string>();

    private void Awake()
    {
        Player_Instance = this;

        //자식객체 player의 컴포넌트 초기화
        animator = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        transform = GetComponent<Transform>();

        agent.acceleration = 80.0f;
        agent.updateRotation = false;

        GetDeckBase(CardDictionary());
    }

    //시작 시
    private void OnEnable()
    {
        status = Status.IDLE;
    }

    public void Update()
    {
        //플레이어 상태 체크
        StartCoroutine(CheckPlayerState());

        //플레이어 애니메이션 체크
        StartCoroutine(PlayerAnim());
    }

    public IEnumerator CheckPlayerState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);
            //남은 거리로 Walk/IDLE 판별
            status = agent.remainingDistance < 0.2f ? Status.IDLE : Status.Walk;

            //어택 판별
            if (player_key.key == "Attack")
            {
                status = Status.Attack;

                yield return new WaitForSeconds(0.2f);
                player_key._key = " ";
            }

            if (player_key.key == "skill")
            {
                status = Status.Throw1;
                yield return new WaitForSeconds(0.6f);
                player_key._key = " ";
            }

            //HP < 0 이면 죽음 상태
            if (player_stats.nowHealth <= 0) { status = Status.DIE; }
        }
    }

    public IEnumerator PlayerAnim()
    {
        while (!isDie)
        {
            switch (status)
            {
                case Status.IDLE:
                    agent.isStopped = false;

                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsWalk", false);
                    animator.SetBool("IsThrow1", false);
                    animator.SetBool("IsFire", false);

                    break;

                case Status.Walk:
                    agent.isStopped = false;

                    animator.SetBool("IsWalk", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1", false);
                    animator.SetBool("IsFire", false);

                    break;

                case Status.Attack:
                    agent.ResetPath();
                    //agent.isStopped = true;

                    animator.SetBool("IsFire", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsThrow1", false);

                    break;

                case Status.Throw1:
                    agent.ResetPath();
                    //agent.isStopped = true;

                    animator.SetBool("IsThrow1", true);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsFire", false);

                    break;

                case Status.Throw2:
                    animator.SetTrigger("Throw2");
                    animator.SetBool("IsIdle", false);

                    break;

                case Status.DIE:
                    animator.SetBool("IsIdle", false);
                    animator.SetTrigger("Die");
                    this.enabled = false;

                    StopAllCoroutines();

                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Deck Initializer, return List<string> names, called only Deck is refreshed or starting game, 완성
    public List<string> GetDeckBase(List<BaseController> originals)
    {
        Dictionary<int, Data.Deck> dict = Managers.Data.DeckDict;
        //Return to InhandDeck
        List<string> cardNames = new List<string>();

        //Init deck all -> make List, that have all of deck
        for(int i = 0; i < originals.Count; i++)
        {
            cardNames.Add(originals[i].name);    //딕셔너리에서 가져와 string list로 만듬
            Debug.Log(cardNames[i]);        //출력을 통한 확인
        }

        //Pick random 4
        for(int i = 0; i < originals.Count; i++)
        {
            //int rand = Random.Range(1, 30); //랜덤값을 받아서 제거
            //여기서 _inHand에 먼저 넣어주기
            //cardNames.RemoveAt(rand);

            cardNames[i] = originals[i].name;
            Debug.Log(cardNames[i].ToString());
		}

        return cardNames;   //Start 에서 이 리턴을 _baseDeck에 저장
    }

    //parameter
    public List<string> UseCard(string cardName)
    {
        List<string> updatedList = new List<string>();
        //if use -> call Card init -> delete from deck

        //Add to empty with Random 1 from deck
        
        return updatedList;
	}

	//Base return Components List for basement of search
	public List<BaseController> CardDictionary()
	{
		Dictionary<string, Card> cards = Managers.Data.CardDict;
		List<BaseController> bcs = new List<BaseController>();
		List<string> cardNames = new List<string>();

		foreach (Card card in cards.Values)
		{
			//Debug.Log(card.name);
			BaseController go = Managers.Resource.Load<BaseController>($"Prefabs/Cards/{card.name}").GetComponent<BaseController>();

			cardNames.Add(card.name);
			if (go.name == card.name)
			{
				bcs.Add(Managers.Resource.Load<BaseController>($"Prefabs/Cards/{card.name}"));

				Debug.Log(Managers.Resource.Load<GameObject>($"Prefabs/Cards/{card.name}"));
			}
		}

		return bcs;
	}
}
