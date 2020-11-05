using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : CurrentStateController
{
    protected override void Die()
    {
        base.Die();
        Messenger.Broadcast(GameEvents.PLAYER_DEAD);
    }
}
