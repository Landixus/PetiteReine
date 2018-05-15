using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	[SerializeField]
	public int hp;
	[SerializeField]
	public int hpmax;
	private Image healthbar;

	void Start ()
	{
		healthbar = GetComponent<Image>();
		m_healthPlayer = GetComponent<HealthPlayer>();
	}




// Actualise les points de vie pour rester entre 0 et hpmax
	private void UpdateHealth(){
		
		float amount = (float)m_healthPlayer.getHealth() / m_healthPlayer._hpmax;
		healthbar.fillAmount = amount;
	}
}
