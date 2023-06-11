using UnityEngine;
using Define;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public bool isGameEnd {get; set;}


    CameraController mainCamera;
    public Vector3 endingCamPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (isGameEnd)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, endingCamPos, Time.deltaTime * 2f);
        }
    }

    public static GameManager Instance
    {
        get { return instance; }
    }

    public void setGameEnd(Vector3 _endCamPos)
    {
        isGameEnd = true;

        /// 카메라 비활성화
        mainCamera = Camera.main.GetComponent<CameraController>();
        endingCamPos = _endCamPos;
        mainCamera.enabled = false;

        /// 오브젝트 비활성화
        ObjectController[] objects = FindObjectsOfType<ObjectController>();

        foreach (ObjectController obj in objects)
        {
            switch(obj._type)
            {
                case ObjectType.Nexus:
                    obj.GetComponent<MinionSummoner>().enabled = false;
                    break;
                case ObjectType.Melee:
                case ObjectType.Range:
                case ObjectType.Super:
                    obj.GetComponent<NavMeshAgent>().enabled = false;
                    obj.GetComponent<Animator>().enabled = false;
                    break;
                case ObjectType.Turret:
                case ObjectType.Neutral:
                    obj.GetComponent<Animator>().enabled = false;
                    break;
            }

            obj.enabled = false;
        }
    }
}
