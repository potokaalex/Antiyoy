using ClientCode.Infrastructure.States.Project;
using ClientCode.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace ClientCode.Client.Code.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        public SceneContext SceneContext;
        private IProjectStateMachine _projectStateMachine;

        [Inject]
        public void Construct(IProjectStateMachine projectStateMachine) => _projectStateMachine = projectStateMachine;

        public void Awake()
        {
#if UNITY_EDITOR
            DontDestroyOnLoad(this);
#endif
            SceneContext.Run();
            _projectStateMachine.SwitchTo<ProjectLoadState>(); //? - можно начинать в projectInstaller
        }
    }
}