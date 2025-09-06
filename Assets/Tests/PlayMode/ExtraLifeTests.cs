using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;
using NSubstitute;

public class ExtraLifePlayModeTests
{
    private GameObject player;
    private ExtraLife extraLife;
    private IPlayerController mockPlayer;
    private IGameManager mockGameManager;
    private IObstaclesGenerating mockObstacles;
    private Image heartUIImage;
    private GameObject usingHeartUI;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        player = new GameObject("Player");
        player.AddComponent<Rigidbody>();
        player.AddComponent<BoxCollider>();
        player.AddComponent<Animator>();
        //player.AddComponent<PlayerController>();
        extraLife = player.AddComponent<ExtraLife>();
        var pc = player.AddComponent<PlayerController>();

        heartUIImage = new GameObject("HeartUI").AddComponent<Image>();
        extraLife.SetHeartUI(heartUIImage);

        usingHeartUI = new GameObject("UsingHeartUI");
        extraLife.SetHeartUIObject(usingHeartUI);

        mockPlayer = Substitute.For<IPlayerController>();
        mockPlayer.IsDead.Returns(true);
        extraLife.PlayerControllerService = mockPlayer;

        mockObstacles = Substitute.For<IObstaclesGenerating>();
        mockObstacles.getLastPosition().Returns(Vector3.zero); // define default behavior
        extraLife.ObstaclesGeneratingService = mockObstacles;

        mockGameManager = Substitute.For<IGameManager>();
        mockGameManager.IsGameRunning = true; // initial state
        extraLife.GameManagerService = mockGameManager;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        if (player != null) Object.Destroy(player);
        if (usingHeartUI != null) Object.Destroy(usingHeartUI);
        if (heartUIImage != null) Object.Destroy(heartUIImage.gameObject);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ActivateHeart_SetsHeartTrueAndUIFull()
    {
        // Act
        extraLife.ActivateHeart();
        yield return null;

        // Assert
        Assert.IsTrue(extraLife.HasExtraLife());
        Assert.AreEqual(extraLife.HeartUI().sprite, extraLife.HeartUI().sprite);
    }

    [UnityTest]
    public IEnumerator ReviveCharacter_WithHeart_ResetsAfterCoroutine()
    {
        extraLife.SetHeart(true);
        mockPlayer.IsDead.Returns(true);

        extraLife.ReviveCharacter();

        yield return new WaitForSeconds(6.1f);

        Assert.IsFalse(extraLife.HasExtraLife());
        Assert.IsFalse(usingHeartUI.activeSelf);
        mockPlayer.Received().ResetLine();
        Assert.IsFalse(mockPlayer.IsDead);
    }
}