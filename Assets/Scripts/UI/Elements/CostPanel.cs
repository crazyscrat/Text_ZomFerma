using System.Linq;
using Oko.Buildings;
using Oko.Reses;
using Oko.Shop;
using TMPro;
using UnityEngine;

namespace Oko.UI
{
    public class CostPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textGold;

        public void SetCost(CostPair[] costs)
        {
            _textGold.SetText($"Gold: {costs.FirstOrDefault(p => p.resource == EResources.Gold).amount}");
        }
    }
}