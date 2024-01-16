using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider) {
        RubyController controller = collider.GetComponent<RubyController>();
        if(controller != null) 
        {
            controller.changeHealth(-1);
        }
    }
}
