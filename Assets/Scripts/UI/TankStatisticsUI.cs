using UnityEngine;
using UnityEngine.UI;

public class TankStatisticsUI : MonoBehaviour
{
    public Slider oxigen;
    public Slider temperature;
    public Slider algaeLevel;
    public Slider pHLevel;
    public Slider salinity;
    public Slider light;
    public TankDatabase database;
    void Update()
    {
        oxigen.value = database.activeTank.currentAttibutes.oxigen;
        temperature.value = database.activeTank.currentAttibutes.temperature;
        algaeLevel.value = database.activeTank.currentAttibutes.algaeLevel;
        pHLevel.value = database.activeTank.currentAttibutes.pHLevel;
        salinity.value = database.activeTank.currentAttibutes.salinity;
        light.value = database.activeTank.currentAttibutes.light;
    }
}
