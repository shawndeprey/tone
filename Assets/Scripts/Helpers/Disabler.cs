using UnityEngine;

public class Disabler : MonoBehaviour
{
	public void Enable()
    {
        foreach (Component component in GetComponents<Component>())
        {
            if (component is Rigidbody2D)
            {
                ((Rigidbody2D)component).isKinematic = false;
            }
            else if (component is Transform)
            {
                continue;
            }
            else if (component is Renderer)
            {
                ((Renderer)component).enabled = true;
            }
            else
            {
                ((Behaviour)component).enabled = true;
            }
        }
    }

    public void Disable()
    {
        foreach (Component component in GetComponents<Component>())
        {
            if (component is Disabler)
            {
                continue;
            }
            else if (component is Transform)
            {
                continue;
            }
            else if (component is Rigidbody2D)
            {
                ((Rigidbody2D)component).isKinematic = true;
            }
            else if (component is Renderer)
            {
                ((Renderer)component).enabled = false;
            }
            else
            {
                ((Behaviour)component).enabled = false;
            }
        }
    }
}