using UnityEngine;
using Zenject;

namespace Client.Code.Project
{
    public class Bootstrapper : MonoBehaviour
    {
        public SceneContext SceneContext;

        public void Awake()
        {
#if UNITY_EDITOR
            DontDestroyOnLoad(this);
#endif
            SceneContext.Run();
            //see ProjectInstaller next
        }
    }
}