using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Team06
{

    public class GameUI : MonoBehaviour
    {
        public Text healthText;
        public Text moneyText;

        void Start()
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.OnChangedHealth += () => { healthText.text = $"Health: {player.Health}"; };

                player.OnChangedMoney += () => { moneyText.text = $"Money: {player.Money}"; };
            }
        }


    }
}
