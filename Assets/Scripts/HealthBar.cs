using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	private Image healthbar;
	private HealthPlayer m_healthPlayer;

	void Start ()
	{
		healthbar = GetComponent<Image>();
		m_healthPlayer = GetComponent<HealthPlayer>();
	}

// Actualise les points de vie pour rester entre 0 et hpmax
	private void UpdateHealth(){

		float amount = (float)m_healthPlayer.GetHealth() / m_healthPlayer._hpmax;
		healthbar.fillAmount = amount;
	}
}
