using UnityEngine;

namespace Client.Project.Infrastructure
{
  public abstract class MonoInstaller : MonoBehaviour
  {
    public abstract void Install(Context context);
  }
}