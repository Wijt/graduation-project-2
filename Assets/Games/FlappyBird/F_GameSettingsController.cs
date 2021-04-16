using UnityEngine;

[CreateAssetMenu(fileName = "Game Setting", menuName = "Game Settings")]
public class F_GameSettingsController : ScriptableObject
{
    public float birdJumpForce = 200;
    public float spaceBetweenPipes = 4;
    public float gapBetweenPipe = 1.5f;
    public float gameSpeed = 1;
}