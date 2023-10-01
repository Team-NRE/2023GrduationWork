/// ksPark
/// 
/// Setting UI 스크립트

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Setting : UI_Scene
{
    enum Progress
    {
        EffectSlider,
        BgmSlider,
    }

    enum GameObjects
    {
        EffectIcon,
        BgmIcon,
    }

    enum Slash
    {
        EffectSlash,
        BgmSlash,
    }

    enum CanvasFader
    {
        Screen,
    }

    float minVolume = 0.0001f;

    public override void Init()
    {
        if (Get<Slider>((int)Progress.EffectSlider) == null) Bind<Slider>(typeof(Progress));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Slash));
        Bind<UI_CanvasFader>(typeof(CanvasFader));

        Get<Slider>((int)Progress.EffectSlider).onValueChanged.AddListener(SetEffectSound);
		Get<Slider>((int)Progress.BgmSlider)   .onValueChanged.AddListener(SetBgmSound);
        
        Get<GameObject>((int)GameObjects.EffectIcon)  .BindEvent(MuteEffectSound);
        Get<GameObject>((int)GameObjects.BgmIcon)     .BindEvent(MuteBgmSound);
        
        Get<Image>((int)Slash.EffectSlash).gameObject.SetActive(false);
        Get<Image>((int)Slash.BgmSlash)   .gameObject.SetActive(false);
    }

    public override void UpdateInit() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Managers.UI.isOpenedPopup)
            {
                Get<UI_CanvasFader>((int)CanvasFader.Screen).ShowUI();
            }
            else if (Get<UI_CanvasFader>((int)CanvasFader.Screen).gameObject.activeSelf)
            {
                Get<UI_CanvasFader>((int)CanvasFader.Screen).HideUI();
            }
        }
    }

    public override void OnEnable()
    {
        if (Get<Slider>((int)Progress.EffectSlider) == null) 
            Bind<Slider>(typeof(Progress));

        Get<Slider>((int)Progress.EffectSlider).value = Managers.Sound.EffectVolume;
        Get<Slider>((int)Progress.BgmSlider)   .value = Managers.Sound.BgmVolume;
    }

    void SetEffectSound(float value)
    {
        Managers.Sound.EffectVolume = value;

        Get<Image>((int)Slash.EffectSlash).gameObject
            .SetActive(Managers.Sound.EffectVolume == minVolume);
    }

    void SetBgmSound(float value)
    {
        Managers.Sound.BgmVolume = value;

        Get<Image>((int)Slash.BgmSlash).gameObject
            .SetActive(Managers.Sound.BgmVolume == minVolume);
    }

    void MuteEffectSound(PointerEventData data)
    {
        if (Managers.Sound.EffectVolume != minVolume)
            Managers.Sound.EffectVolume = minVolume;
        else
            Managers.Sound.EffectVolume = Get<Slider>((int)Progress.EffectSlider).value;

        Get<Image>((int)Slash.EffectSlash).gameObject
            .SetActive(Managers.Sound.EffectVolume == minVolume);
    }

    void MuteBgmSound(PointerEventData data)
    {
        if (Managers.Sound.BgmVolume != minVolume)
            Managers.Sound.BgmVolume = minVolume;
        else
            Managers.Sound.BgmVolume = Get<Slider>((int)Progress.BgmSlider).value;

        Get<Image>((int)Slash.BgmSlash).gameObject
            .SetActive(Managers.Sound.BgmVolume == minVolume);
    }
}
