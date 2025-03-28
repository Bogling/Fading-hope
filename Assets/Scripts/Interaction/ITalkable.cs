using UnityEngine;

public interface ITalkable
{
    public void Focus();
    public void Talk(TextAsset inkJSON);

    public void OperateChoice(int qID, int cID);

    public void UponExit();
    public void ChangeSprite(string spriteID);
}
