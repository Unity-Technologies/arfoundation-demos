using UnityEngine;

public class KeyUIHandler : MonoBehaviour
{
    [SerializeField]
    GameObject m_UI;

    public GameObject ui
    {
        get => m_UI;
        set => m_UI = value;
    }

    public void ToggleVisability()
    {
        m_UI.SetActive(!m_UI.activeSelf);
    }
}
