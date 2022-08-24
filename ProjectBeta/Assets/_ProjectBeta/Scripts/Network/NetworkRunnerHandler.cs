using System;
using System.Collections;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Network
{
    public sealed class NetworkRunnerHandler : MonoBehaviour
    {
        [SerializeField] private NetworkRunner networkRunnerPrefab;
        [SerializeField] private TMP_InputField inputFieldSession;
        [SerializeField] private Button buttonCreate;
        [SerializeField] private Button buttonJoin;
        [SerializeField] private GameObject menu;
        [SerializeField] private TMP_Text statusText;

        private NetworkRunner _networkRunner;

        private void Start()
        {
            _networkRunner = Instantiate(networkRunnerPrefab);
            _networkRunner.name = "NetworkRunner";
            statusText.text = "";
            
            buttonCreate.onClick.AddListener(CreateSessionHandler);
            buttonJoin.onClick.AddListener(JoinSessionHandler);
        }
        
        private void CreateSessionHandler()
        {
            var currentTask = CreateNetworkSession(_networkRunner, GameMode.Client, NetAddress.Any(), (SceneManager.GetActiveScene().buildIndex-1), null);
            StartCoroutine(CreatingSessionCoroutine(currentTask));
        }

        private void JoinSessionHandler()
        {
            var currentTask = CreateNetworkSession(_networkRunner, GameMode.Host, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
            StartCoroutine(JoinSessionCoroutine(currentTask));
        }

        private Task CreateNetworkSession(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
        {
            runner.ProvideInput = true;

            buttonCreate.interactable = false;
            buttonJoin.interactable = false;

            return runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                Address = address,
                Scene = scene,
                SessionName = inputFieldSession.text,
                Initialized = initialized,
            });
        }

        private IEnumerator CreatingSessionCoroutine(Task currentTask)
        {
            var totalTime = 10f;

            var pointTime = 0.5f;
            var amountOfPoints = 0;

            statusText.text = "Creating Session";

            while (totalTime > 0)
            {
                totalTime -= Time.deltaTime;

                pointTime -= Time.deltaTime;

                if (pointTime <= 0)
                {
                    amountOfPoints++;

                    if (amountOfPoints > 3)
                    {
                        statusText.text = "Creating Session";
                        amountOfPoints = 0;
                    }

                    else statusText.text += ".";

                    pointTime = 0.5f;
                }

                if (currentTask.IsCompleted)
                    break;
                
                yield return null;
            }
            menu.SetActive(false);
        }
        
        private IEnumerator JoinSessionCoroutine(Task currentTask)
        {
            var totalTime = 10f;

            var pointTime = 0.5f;
            var amountOfPoints = 0;

            statusText.text = "Joining Session";

            while (totalTime > 0)
            {
                totalTime -= Time.deltaTime;

                pointTime -= Time.deltaTime;

                if (pointTime <= 0)
                {
                    amountOfPoints++;

                    if (amountOfPoints > 3)
                    {
                        statusText.text = "Joining Session";
                        amountOfPoints = 0;
                    }

                    else statusText.text += ".";

                    pointTime = 0.5f;
                }

                if (currentTask.IsCompleted)
                {
                    menu.SetActive(false);
                    break;
                }

                yield return null;
            }

            if (totalTime <= 0) 
                statusText.text = "Couldn't find the session.";

            buttonCreate.interactable = true;
            buttonJoin.interactable = true;

            currentTask.Dispose();

            yield return new WaitForSeconds(3);

            statusText.text = "";
        }
    }
}