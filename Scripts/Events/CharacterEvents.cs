using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents : MonoBehaviour
{
    //public static UnityEvent<GameObject, int> characterDamaged;
    //public static UnityEvent<GameObject, int> characterHealed;

    public static UnityAction<GameObject, int> s_characterDamaged;
    public static UnityAction<GameObject, int,string> s_characterHealed;
    public static UnityAction s_playerDie;

    // UnityEvent�� MonoBehaviour������ ����ϴ°Ű� ������ UnityAction ���
}
