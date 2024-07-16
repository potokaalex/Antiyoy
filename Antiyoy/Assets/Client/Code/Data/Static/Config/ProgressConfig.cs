using UnityEngine;

namespace ClientCode.Data.Static.Config
{
    [CreateAssetMenu(menuName = "Configs/Progress", fileName = "ProgressConfig", order = 0)]
    public class ProgressConfig : ScriptableObject
    {
        public int MaxMapsSavesCount;
    }
}