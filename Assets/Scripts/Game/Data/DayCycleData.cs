using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DayCycleData", menuName = "GameData/DayCycleData")]
public class DayCycleData : ScriptableObject
{
    [SerializeField] private float _maxDayTime;
    [SerializeField] private DayPhaseConfig[] _phases = new DayPhaseConfig[3];

    public float MaxDayTime => _maxDayTime;
    public DayPhaseConfig[] Phases => _phases;
}

[Serializable]
public class DayPhaseConfig
{
    [SerializeField] private DayPhase _phase;
    [SerializeField] private int _percent;

    public DayPhase Phase => _phase;
    public int Percent => _percent;
}
