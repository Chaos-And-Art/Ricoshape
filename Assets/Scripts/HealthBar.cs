using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image Filler = null;
    [SerializeField] Entity entity = null;
    private void Update()
    {
        Filler.fillAmount = Mathf.Lerp(Filler.fillAmount, entity.Health / entity.MaxHealth, 0.2f);
    }
}
