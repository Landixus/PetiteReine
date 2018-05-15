using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
[SerializeField]
private int hp;
[SerializeField]
private int hpmax;
private Image healthbar;

void Start ()
{
healthbar = GetComponent<Image>();
}

// Inflige des dégâts
public void TakeDamage(int damages)
{
hp -= damages;
UpdateHealth();
}
// Soigne le joueur
public void Heal(int heal)
{
hp += heal;
UpdateHealth();
}

// Actualise les points de vie pour rester entre 0 et hpmax
private void UpdateHealth()
{
hp = Mathf.Clamp(hp, 0, hpmax);
float amount = (float)hp / hpmax;
healthbar.fillAmount = amount;
}
}
