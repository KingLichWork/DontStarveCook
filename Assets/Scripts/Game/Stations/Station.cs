using UnityEngine;

public abstract class Station : MonoBehaviour
{
    public abstract void SetFood(FoodViewUI foodView);

    public abstract void StartCook();

    public abstract void ClearStation();
}
