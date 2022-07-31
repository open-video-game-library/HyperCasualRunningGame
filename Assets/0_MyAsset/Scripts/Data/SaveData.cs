using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static Dictionary<string, object> data = new Dictionary<string, object>
    {
        {"testInt",1},
        {"testFloat",0.5f},
        {"testString","aaa"},
        {"testBool",true}
    };

    public static void Save(){
        foreach(var key in data){
            
        }
    }
}
