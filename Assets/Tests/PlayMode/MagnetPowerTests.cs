using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NSubstitute;

public class MagnetPowerPlayModeTests
{
    private GameObject magnetGO;
    private MagnetPower magnet;

    private GameObject magnetUI;
    private IBarUI mockBarUI;
    private IBoots mockBoots;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        magnetGO = new GameObject("MagnetPower");
        magnet = magnetGO.AddComponent<MagnetPower>();

        magnetUI = new GameObject("MagnetUI");
        magnetUI.transform.parent = magnetGO.transform;
        magnet.magnetUI = magnetUI;

        mockBarUI = Substitute.For<IBarUI>();
        magnet.BarUIService = mockBarUI;

        mockBoots = Substitute.For<IBoots>();
        magnet.BootsService = mockBoots;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(magnetGO);
        Object.Destroy(magnetUI);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ActivateMagnet_ActivatesAndDeactivatesAfterDuration()
    {
        mockBoots.PosUI().Returns(0);
        magnet.ActivateMagnet();

        yield return null;

        Assert.IsTrue(magnet.MagnetActive());
        Assert.IsTrue(magnetUI.activeSelf);
        Assert.AreEqual(1, magnet.PosUI());

        yield return new WaitForSeconds(10.2f);

        Assert.IsFalse(magnet.MagnetActive());
        Assert.IsFalse(magnetUI.activeSelf);
        Assert.AreEqual(0, magnet.PosUI());
    }

    [UnityTest]
    public IEnumerator ActivateMagnet_BootsPosUI1_SetsUIPositionAndPosUI2()
    {
        mockBoots.PosUI().Returns(1);
        magnet.ActivateMagnet();

        yield return null;

        Assert.AreEqual(2, magnet.PosUI());
        Assert.AreEqual(160f, magnetUI.transform.position.x, 0.01f);
    }

    [UnityTest]
    public IEnumerator ActivateMagnet_SecondActivation_CallsBarUIResetTimer()
    {
        mockBoots.PosUI().Returns(0);

        magnet.ActivateMagnet();
        yield return null;

        mockBarUI.Received().ResetTimer();
    }
}