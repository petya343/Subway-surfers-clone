using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using System.Collections;

public class PlayerControllerTests
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
        Object.Destroy(player);
    }

    [UnityTest]
    public IEnumerator PlayerMovesForward_WhenGameRunning()
    {
        float startZ = player.transform.position.z;

        yield return null;

        Assert.Greater(player.transform.position.z, startZ);
    }

    [UnityTest]
    public IEnumerator UpdateScore_IsCalledEveryFrame()
    {
        yield return null;

        mockScoreSystem.Received().UpdateScore();
    }

    [UnityTest]
    public IEnumerator Jump_WhenGrounded_SetsUpwardVelocity()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        var mockBoots = Substitute.For<IBoots>();
        mockBoots.BootsActive.Returns(true);
        mockBoots.BootsJumpForce.Returns(15f);
        controller.BootsService = mockBoots;

        controller.isGrounded = true;

        controller.Jump();

        Assert.AreEqual(15f, rb.velocity.y, 0.01f);
        mockBoots.Received().PlayBootsJumpEffects();

        yield return null;
    }



}
