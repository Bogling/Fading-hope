using UnityEngine;

public class SetBaseMap : StateMachineBehaviour
{
    public Material material;
    public Texture2D baseMap;

    // This will be called when the animator first transitions to this state.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        material.SetTexture("_BaseMap", baseMap);
    }
}
