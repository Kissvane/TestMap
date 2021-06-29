using UnityEngine.UI;

public class TextObjectPooler : ObjectPooler<Text>
{
    public void DisableAllObject()
    {
        foreach (Text t in pool)
        {
            t.gameObject.SetActive(false);
        }
    }

    public override void Release(Text go)
    {
        base.Release(go);
        go.gameObject.SetActive(false);
    }
}
