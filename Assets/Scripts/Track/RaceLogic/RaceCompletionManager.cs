using UnityEngine;

public class RaceCompletionManager : MonoBehaviour
{
    [Header("Prize Money by Position")]
    public int firstPlacePrize = 1000;
    public int secondPlacePrize = 750;
    public int thirdPlacePrize = 500;
    public int fourthPlacePrize = 300;
    public int fifthPlacePrize = 100;
    
    public void OnRaceCompleted(int playerPosition, string trackName, string raceTime)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        
        int prizeMoney = CalculatePrizeMoney(playerPosition);
        
        if (prizeMoney > 0)
        {
            GameManager.Instance.AddMoney(prizeMoney);
            Debug.Log($"Race completed! Position: {playerPosition}, Prize: {prizeMoney}");
        }
        
        Car currentCar = GameManager.Instance.GetCurrentCar();
        string carName = currentCar != null ? currentCar.Name : "Unknown Car";
        
        RaceResult result = new RaceResult
        {
            Week = GetCurrentWeek(),
            Position = playerPosition,
            Revenue = prizeMoney,
            Map = trackName,
            Car = carName,
            Time = raceTime
        };
        
        SaveRaceResult(result);
    }
    
    private int CalculatePrizeMoney(int position)
    {
        return position switch
        {
            1 => firstPlacePrize,
            2 => secondPlacePrize,
            3 => thirdPlacePrize,
            4 => fourthPlacePrize,
            5 => fifthPlacePrize,
            _ => 0
        };
    }
    
    private int GetCurrentWeek()
    {
        return System.DateTime.Now.DayOfYear / 7;
    }
    
    private void SaveRaceResult(RaceResult result)
    {
        Debug.Log($"Race result saved: {result.Position} place on {result.Map} with {result.Car}");
    }
}