using UnityEngine;
namespace Nave_Project
{
public class ButtonClick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int click(bool ans)
    {

        if($"{name}"=="Save"){
            ans=true;
        }
        else
        {
            ans=false;
        }
        return 0;
    }
}
}