using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;

    public void UpdateCoins(int coins)
    {
        coinText.text = coins + "";
    }

}
