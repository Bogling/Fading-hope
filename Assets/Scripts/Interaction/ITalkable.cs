using UnityEngine;

public interface ITalkable
{
    public void Talk(TextAsset inkJSON);

    public void OperateChoice(int qID, int cID);
}
