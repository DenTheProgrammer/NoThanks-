using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Card : MonoBehaviour, IPunObservable
{
    public int Value { get; private set; }
    [SerializeField]
    private TextMeshProUGUI upText;
    [SerializeField]
    private TextMeshProUGUI downText;


    public void SetValue(int value)
    {
        this.Value = value;
        UpdateText();
    }

    private void Start()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        upText.text = downText.text = Value.ToString();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Value);
        }
        else
        {
            SetValue((int)stream.ReceiveNext());
        }
    }
}
