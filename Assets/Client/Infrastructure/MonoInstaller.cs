using UnityEngine;

namespace Client.Infrastructure
{
  public abstract class MonoInstaller : MonoBehaviour
  {
    public abstract void Install(Context context);
  }
}