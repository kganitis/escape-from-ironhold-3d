using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// A class that is used to handle the win condition and display the win condition panel.
/// </summary>
public class WinCondition : MonoBehaviour
{
    public GameObject winConditionPanel;
    public MessageHandler messageHandler;
    public CursorHandler cursorHandler;
    public GameObject player;
    public GuardPatrolState guardPatrol;
    public Transform winPoint;

    private void Start()
    {
        winConditionPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messageHandler.DisplayMessage("Congratulations! You have escaped the prison!");
            
            winConditionPanel.SetActive(true);
            cursorHandler.SetHand();
            guardPatrol.gameObject.SetActive(false);
            player.GetComponent<PlayerController>().active = false;
            player.GetComponent<NavMeshAgent>().destination = winPoint.position;
        }
    }
}
