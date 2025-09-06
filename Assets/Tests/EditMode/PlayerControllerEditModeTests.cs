using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerControllerEditModeTests
{
    private GameObject player;
    private PlayerController controller;
    private IGameManager mockGameManager;
    private IScoreSystem mockScoreSystem;
    private IAudioManager mockAudioManager;

    [SetUp]
    public void Setup()
    {
        player = new GameObject("Player");
        player.AddComponent<Rigidbody>();
        player.AddComponent<BoxCollider>();
        player.AddComponent<Animator>();

        controller = player.AddComponent<PlayerController>();

        mockGameManager = Substitute.For<IGameManager>();
        mockScoreSystem = Substitute.For<IScoreSystem>();
        mockAudioManager = Substitute.For<IAudioManager>();

        controller.GameManagerService = mockGameManager;
        controller.ScoreSystemService = mockScoreSystem;
        controller.AudioManagerService = mockAudioManager;

        mockGameManager.IsGameRunning.Returns(true);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(player);
    }

    [Test]
    public void ResetLine_SetsLineAndX()
    {
        controller.ResetLine();
        controller.MoveForward();
        Assert.AreEqual(0, controller.line);
        Assert.AreEqual(3.2f, controller.transform.position.x, 0.01f);
    }

    [Test]
    public void Die_SetsIsDead()
    {
        controller.Die();
        Assert.IsTrue(controller.IsDead);
    }

    [Test]
    public void StartAndEndSliding_DoNotCrash()
    {
        Assert.DoesNotThrow(() =>
        {
            controller.StartSliding();
            controller.EndSliding();
        });
    }

    [Test]
    public void ScoreSystem_UpdateScore_CanBeCalledPublicly()
    {
        controller.ScoreSystemService.UpdateScore();
        mockScoreSystem.Received().UpdateScore();
    }

    [Test]
    public void MoveForward_IncreasesZPosition()
    {
        float startZ = controller.transform.position.z;
        controller.MoveForward();
        Assert.Greater(controller.transform.position.z, startZ);
    }

    [Test]
    public void StartAndEndSliding_EditMode_DoesNotCrash()
    {
        BoxCollider col = player.GetComponent<BoxCollider>();
        float originalHeight = col.size.y;
        Assert.DoesNotThrow(() =>
        {
            controller.StartSliding();
            controller.EndSliding();
        });

        controller.StartSliding();
        Assert.AreEqual(originalHeight, col.size.y, 0.01f);
    }
}
