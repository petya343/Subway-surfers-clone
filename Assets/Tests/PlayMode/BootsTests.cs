using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;
using NSubstitute;

public class BootsPlayModeTests
{
    private GameObject bootsGO;
    private Boots boots;
    private GameObject leftBoot, rightBoot, bootsUI;
    private IBarUI mockBarUI;
    private IAudioManager mockAudio;
    private IMagnetPower mockMagnet;


    [UnitySetUp]
    public IEnumerator Setup()
    {
        bootsGO = new GameObject("Boots");
        boots = bootsGO.AddComponent<Boots>();

        leftBoot = new GameObject("LeftBoot");
        rightBoot = new GameObject("RightBoot");
        bootsUI = new GameObject("BootsUI");

        leftBoot.SetActive(false);
        rightBoot.SetActive(false);

        leftBoot.transform.parent = bootsGO.transform;
        rightBoot.transform.parent = bootsGO.transform;
        bootsUI.transform.parent = bootsGO.transform;

        boots.leftBoot = leftBoot;
        boots.rightBoot = rightBoot;
        boots.bootsUI = bootsUI;

        boots.jumpEffectLeft = new GameObject("JumpEffectLeft").AddComponent<ParticleSystem>();
        boots.jumpEffectRight = new GameObject("JumpEffectRight").AddComponent<ParticleSystem>();

        mockBarUI = Substitute.For<IBarUI>();
        mockAudio = Substitute.For<IAudioManager>();
        mockMagnet = Substitute.For<IMagnetPower>();

        boots.BarUIService = mockBarUI;
        boots.AudioManagerService = mockAudio;
        boots.MagnetService = mockMagnet;

        mockMagnet.PosUI().Returns(0);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(bootsGO);
        Object.Destroy(leftBoot);
        Object.Destroy(rightBoot);
        Object.Destroy(bootsUI);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ActivateBoots_EnablesBootsAndDisablesAfterDuration()
    {

        boots.ActivateBoots();
        yield return null;

        Assert.IsTrue(boots.BootsActive);
        Assert.IsTrue(leftBoot.activeSelf);
        Assert.IsTrue(rightBoot.activeSelf);

        boots.BarUIService.Received().ResetTimer();
        boots.AudioManagerService.Received().PlayPowerCollect();
        boots.MagnetService.Received().PosUI();

        yield return new WaitForSeconds(10.1f);

        Assert.IsFalse(boots.BootsActive);
        Assert.IsFalse(leftBoot.activeSelf);
        Assert.IsFalse(rightBoot.activeSelf);
    }

    [UnityTest]
    public IEnumerator PlayBootsJumpEffects_DoesNotCrash()
    {
        Assert.DoesNotThrow(() =>
        {
            boots.PlayBootsJumpEffects();
        });

        yield return null;
    }
}