
using UnityEngine;
public interface IProjectile
{
    float moveSpeed { get; set; }
    float damage { get; set; }

    float liveTime { get; set; }
    int maxTargetHit { get; set; }

    bool isAlliedForPlayer { get; set; }

    bool isAlliedForEnemy { get; set; }

    void AddExistTime(float existTime);

    bool IsNeedTODestroy();

    void Destroy();

    void Move();
    
}
