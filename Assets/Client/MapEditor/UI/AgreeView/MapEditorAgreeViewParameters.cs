using System;
using UnityEngine;

namespace Client.MapEditor.UI.AgreeView
{
  public struct MapEditorAgreeViewParameters
  {
    public Action AgreeAction;
    public string Header;
    public Color CancelColor;
    public string CancelText;
    public Color AgreeColor;
    public string AgreeText;
  }
}