using UnityEngine;

namespace Core
{
    public class Hit
    {
        public UniqueID    BulletID => bulletID;
        public float       Damage => damage;
        public GameObject  Initiator => initiator;
        public float       Radius => radius;

        private UniqueID    bulletID;
        private float       damage;
        private GameObject  initiator;
        private float       radius;

        // PLANNED add & implement yield modifier
        // eg 1.0 for the drill and 0.1 for really destructive weapon

        public Hit (float damage, GameObject initiator, float radius, UniqueID bulletID)
        {
            this.bulletID   = bulletID;
            this.damage     = damage;
            this.initiator  = initiator;
            this.radius     = radius;
        }

        public Hit (Hit copy, float damage = -1, GameObject initiator = null, float radius = -1)
        {
            this.bulletID = copy.BulletID;
            this.damage = damage == -1 ? copy.Damage : damage;
            this.initiator = initiator ?? copy.Initiator;
            this.radius = radius == -1 ? copy.Radius : radius;
        }
    } 
}