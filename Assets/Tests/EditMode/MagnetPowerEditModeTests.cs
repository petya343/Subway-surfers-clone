using NUnit.Framework;
using UnityEngine;
using NSubstitute;

public class MagnetPowerEditModeTests
{
    private GameObject magnetGO;
    private MagnetPower magnet;
    private IBarUI mockBarUI;
    private IBoots mockBoots;

    [SetUp]
    public void Setup()
    {
        magnetGO = new GameObject("MagnetPower");
        magnet = magnetGO.AddComponent<MagnetPower>();

        magnet.magnetUI = new GameObject("MagnetUI");

        mockBarUI = Substitute.For<IBarUI>();
        magnet.BarUIService = mockBarUI;

        mockBoots = Substitute.For<IBoots>();
        magnet.BootsService = mockBoots;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(magnetGO);
        Object.DestroyImmediate(magnet.magnetUI);
    }

    [Test]
    public void PosUI_DefaultIsZero()
    {
        Assert.AreEqual(0, magnet.PosUI());
    }

    [Test]
    public void ActivateMagnet_UpdatesPosUIToOne_WhenBootsPosUIZero()
    {
        mockBoots.PosUI().Returns(0);

        magnet.ActivateMagnet();

        Assert.AreEqual(1, magnet.PosUI());
    }

    [Test]
    public void ActivateMagnet_UpdatesPosUIToTwo_WhenBootsPosUIOne()
    {
        mockBoots.PosUI().Returns(1);

        magnet.ActivateMagnet();

        Assert.AreEqual(2, magnet.PosUI());
    }
}