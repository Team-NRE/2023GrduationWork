using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// ����Ʈ�� ����
/// 1. ��ƼŬ �� �ð�ȿ��
/// 2. �˹� ��
/// -----
/// 1. PhotonNetwork�ڵ带 �𸣴��� ������ �ٷ� ���� �����ϵ��� ó��
/// 2. �޼��� �������̵��� ���� �پ��� ��Ÿ�ӵ� �ɼ� ����
/// </summary>
public class EffectManager : MonoBehaviour
{
    private Transform   objectTransform;
    private Rigidbody   rb;
    private PhotonView  pv;

    private void Start()
    {
        objectTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    #region ExpEffect

    //ź���� ����Ʈ�� ������ Ư�� �������� ��ƼŬ�� ���ư����� targetVec�� ���⺤�͸� �����ڰ�
    //�׳� �����̸� identity�� ���������� ��

    //��� �ߵ���, ���ڸ�
    public void ExplosiveEffect(GameObject expEffect, Transform tr, float persist)
    {
        GameObject tmp;
        tmp = PhotonNetwork.Instantiate("expEffect", tr.position, Quaternion.identity);
        Destroy(tmp, persist);

        //Ŭ���̾�Ʈ���� �������� Ȯ�ν� ��Ʈ��ũ������ ����
        if (tmp == null)
            PhotonNetwork.Destroy(tmp);
    }


    //��Ÿ�� ������, ���ڸ�
    public IEnumerator ExplosiveEffect(GameObject expEffect, float coolTime, float persist)
    {
        GameObject tmp;
        Transform tr = this.transform;
        //Ư�� ��Ÿ�� ���� �� ����Ʈ �ߵ��ϵ��� ����
        yield return new WaitForSeconds(coolTime);
        tmp = PhotonNetwork.Instantiate("expEffect", tr.position, Quaternion.identity);

        Destroy(tmp, persist);
        if (tmp == null)
            PhotonNetwork.Destroy(tmp);
    }

    //��Ÿ�� ������, ź������
    public IEnumerator ExplosiveEffect(GameObject expEffect, float coolTime, Transform targetVec)
    {
        yield return new WaitForSeconds(coolTime);
    }

    //�˹� �߻���
    public void ExplosiveEffect(GameObject expEffect, Transform tr, float radius, float persist)
    {
        GameObject tmp;
        
    }

    #endregion

    public void TrailEffect(GameObject trailEffect)
    {
        
    }

    #region FloorEffect

    public void FloorEffect(GameObject floorEffect, float persist)
    {

    }

    public void FloorEffect(GameObject floorEffect)
    {

    }

    #endregion

    #region CharParticle

    //���ο���
    public void CharParticle(GameObject particleEffect, float persist)
    {
        GameObject tmp = PhotonNetwork.Instantiate("particleEffect", this.transform.position, Quaternion.identity);
        Destroy(tmp, persist);

        if (tmp == null)
            PhotonNetwork.Destroy(tmp);
    }

    //Ư�� ��󿡰�
    public void CharParticle(GameObject particleEffect, GameObject target, float persist)
    {
        Transform targetTr = target.transform;
        GameObject tmp = PhotonNetwork.Instantiate("particleEffect", targetTr.position, targetTr.rotation);

        Destroy(tmp, persist);
        if (tmp == null)
            PhotonNetwork.Destroy(tmp);
    }

    #endregion

    #region FromSky

    //���� ���
    public void FromSkyEffect(GameObject effect)
    {

    }

    //����
    public void FromSkyEffect(GameObject effect, GameObject skillGuid)
    {

    }

    #endregion

    //��ó�� ��ų ������ �˷��ִ� ����Ʈ
    public void SkillAreaEffect(GameObject skillGuide)
    {
        
    }

    //�°ų� �������� ����Ʈ
    public void HitEffect(GameObject hitEffect, Vector3 pos, Quaternion rot, Transform tr)
    {
        GameObject tmp = PhotonNetwork.Instantiate("hitEffect", pos, rot);
        Destroy(tmp, 1.0f); //�������ϱ� ��Ʈ����Ʈ�� 1�ʷ� ����

        if (tmp == null)
            PhotonNetwork.Destroy(tmp);
    }
}