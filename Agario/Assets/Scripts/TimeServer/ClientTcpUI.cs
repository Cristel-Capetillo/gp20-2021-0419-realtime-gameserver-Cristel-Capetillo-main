using UnityEngine;
using UnityEngine.UI;

namespace TimeServer {
    public class ClientTcpUI : ClientTcp {
        [SerializeField] Button getTimeButton;
        [SerializeField] Text dataReceiverText;
    
    
        void Awake() {
            getTimeButton.onClick.AddListener(base.StartClient);

            OnClientStarted = () => {
                getTimeButton.interactable = false;
            };

            OnClientClosed = () => {
                getTimeButton.interactable = true;
            };
        }

        protected override void DateAndTimeDataFromServer(string message, Color color) {
            base.DateAndTimeDataFromServer(message, color);
            dataReceiverText.text += '\n' + message;
        }
    
        protected override void DateAndTimeDataFromServer(string message) {
            base.DateAndTimeDataFromServer(message);
            dataReceiverText.text += '\n' + message;
        }
    }
}
