using UnityEngine;

namespace CustomEditor
{
    /// <summary>
    /// Attribute for Tags in Editor
    /// </summary>
    public class TagSelectorAttribute : PropertyAttribute
    {
        public bool UseDefaultTagFieldDrawer = false;
    }
}