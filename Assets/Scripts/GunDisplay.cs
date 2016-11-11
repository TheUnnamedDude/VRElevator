using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.UI;

public class GunDisplay : MonoBehaviour {

    private BasePlayer _basePlayer;


    Slider energyBar;
    public Text ammovalue;
    public Text Single;
    public Text Auto;
    public Text Burst;
    public Text Explosive;
    int shotsLeft;

    //Color active = new Color(233, 169, 32);
    

	// Use this for initialization
	void Start () {
        energyBar = GetComponentInChildren<Slider>();
        _basePlayer = GetComponentInParent<BasePlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        shotsLeft = (int) (_basePlayer.currentEnergy / _basePlayer.energyDecrease);
        UpdateGunDisplay();
	}

    public void UpdateGunDisplay()
    {
        energyBar.value = _basePlayer.currentEnergy;
        ammovalue.text = shotsLeft.ToString();
        switch(_basePlayer.FiringMode)
        {
            case 0:
                Single.color = Color.green;
                Auto.color = Color.black;
                Burst.color = Color.black;
                Explosive.color = Color.black;
                break;
            case 1:
                Single.color = Color.black;
                Auto.color = Color.green;
                Burst.color = Color.black;
                Explosive.color = Color.black;
                break;
            case 2:
                Single.color = Color.black;
                Auto.color = Color.black;
                Burst.color = Color.green;
                Explosive.color = Color.black;
                break;
            case 3:
                Single.color = Color.black;
                Auto.color = Color.black;
                Burst.color = Color.black;
                Explosive.color = Color.green;
                break;
        }
        
    }
}
