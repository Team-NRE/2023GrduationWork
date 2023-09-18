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

    enum Icon
    {
        EffectIcon,
        BgmIcon,
    }

    enum Slash
    {
        EffectSlash,
        BgmSlash,
    }

    public override void Init()
    {
        Bind<Slider>(typeof(Progress));
        Bind<GameObject>(typeof(Icon));
        Bind<Image>(typeof(Slash));

        Get<Slider>((int)Progress.EffectSlider).onValueChanged.AddListener(SetEffectSound);
		Get<Slider>((int)Progress.BgmSlider)   .onValueChanged.AddListener(SetBgmSound);
        
        Get<GameObject>((int)Icon.EffectIcon)  .BindEvent(MuteEffectSound);
        Get<GameObject>((int)Icon.BgmIcon)     .BindEvent(MuteBgmSound);
        
        Get<Image>((int)Slash.EffectSlash).gameObject.SetActive(false);
        Get<Image>((int)Slash.BgmSlash)   .gameObject.SetActive(false);
    }

    public override void UpdateInit() { }

    void SetEffectSound(float value)
    {
        Managers.Sound.EffectVolume = value;

        Get<Image>((int)Slash.EffectSlash).gameObject
            .SetActive(Managers.Sound.EffectVolume == 0);
    }

    void SetBgmSound(float value)
    {
        Managers.Sound.BgmVolume = value;

        Get<Image>((int)Slash.BgmSlash).gameObject
            .SetActive(Managers.Sound.BgmVolume == 0);
    }

    void MuteEffectSound(PointerEventData data)
    {
        if (Managers.Sound.EffectVolume != 0)
            Managers.Sound.EffectVolume = 0;
        else
            Managers.Sound.EffectVolume = Get<Slider>((int)Progress.EffectSlider).value;

        Get<Image>((int)Slash.EffectSlash).gameObject
            .SetActive(Managers.Sound.EffectVolume == 0);
    }

    void MuteBgmSound(PointerEventData data)
    {
        if (Managers.Sound.BgmVolume != 0)
            Managers.Sound.BgmVolume = 0;
        else
            Managers.Sound.BgmVolume = Get<Slider>((int)Progress.BgmSlider).value;

        Get<Image>((int)Slash.BgmSlash).gameObject
            .SetActive(Managers.Sound.BgmVolume == 0);
    }
}
