using System.Collections;
using UnityEngine;

namespace Client.Project
{
  public interface ICoroutineRunner
  {
    Coroutine StartCoroutine(IEnumerator enumerator);
  }
}