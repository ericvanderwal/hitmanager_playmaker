// Custom Action by DumbGameDev
// www.dumbgamedev.com
// All rights reserved Eric Vander Wal 2017

using UnityEngine;
using MLSpace;
using HutongGames;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Custom")]
    [Tooltip("Enble ragdoll collision on a rigidbody game object.")]

	public class enableRagdollCollision : FsmStateAction

    {
	    [Note ("Only works with on collision enter, exit, stay and particle collision.")]
	    
	    [RequiredField]
     	[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The gameobject with the collider.")]
		[TitleAttribute ("GameObject")]
		public FsmOwnerDefault gameObject;
	    
	    [Tooltip("The type of collision to detect.")]
	    public CollisionType collision;

	    public FsmFloat hitInterval;        // do not hit every frame. give it a little buffer.
	    
	    public FsmFloat massMultiplier;
	    
	    [ActionSection ("Output")]
	    
	    [UIHint(UIHint.Variable)]
	    [Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
	    public FsmGameObject storeCollider;
	    
	    [UIHint(UIHint.Variable)]
	    [Tooltip("Store the force of the collision. NOTE: Use Get Collision Info to get more info about the collision.")]
	    public FsmFloat storeForce;
	    
	    [ActionSection ("Event")]
	    
	    public FsmEvent hit;
	    
	    // Private variables
	    private Rigidbody rb;
	    private float hitTimer = 0.0f;
	    
        public override void Reset()
	    {
		    
		    collision = CollisionType.OnCollisionEnter;
		    gameObject = null;
		    storeForce = null;
		    storeCollider = null;
		    massMultiplier = null;
		    hit = null;
		    hitInterval = 1;
	    }
	    
	    public override void OnPreprocess()
	    {
		    switch (collision)
		    {
		    case CollisionType.OnCollisionEnter:
			    Fsm.HandleCollisionEnter = true;
			    break;
		    case CollisionType.OnCollisionStay:
			    Fsm.HandleCollisionStay = true;
			    break;
			case CollisionType.OnParticleCollision:
			    Fsm.HandleParticleCollision = true;
				break;
		    case CollisionType.OnCollisionExit:
			    Fsm.HandleCollisionExit = true;
			    break;
			}
		    
	    }
	    
	    void StoreCollisionInfo(Collision collisionInfo)
	    {
		    storeCollider.Value = collisionInfo.gameObject;
		    storeForce.Value = collisionInfo.relativeVelocity.magnitude;
	    }

        public override void OnEnter()
	    {
		    
          var go = Fsm.GetOwnerDefaultTarget(gameObject);
	     rb = go.GetComponent<Rigidbody>();
		hitTimer = hitInterval.Value; 
		    
		    if (!rb) 
		    { 
			    Debug.Log("Rigidbody is missing");
			    Finish();
		    }

        }

        public override void OnUpdate()
        {
	        hitTimer += Time.deltaTime;
        }


		public override void DoCollisionEnter(Collision collisionInfo)
		 {
		    if (collision == CollisionType.OnCollisionEnter)
		    {

			    StoreCollisionInfo(collisionInfo);
	    
			    if (hitTimer > hitInterval.Value)
			    {
				    BodyColliderScript bcs = collisionInfo.collider.GetComponent<BodyColliderScript>();
				    if (bcs && rb)
				    {
					    var mass = rb.mass * massMultiplier.Value;
					    int[] indices = new int[] { bcs.index };
					    bcs.ParentRagdollManager.startHitReaction(indices, rb.velocity * mass);
				    }
				    
				    hitTimer = 0.0f;
				    Fsm.Event(hit);
				    
			    }
			}
		 }
	    
	    public override void DoCollisionExit(Collision collisionInfo)
	    {
		    if (collision == CollisionType.OnCollisionExit)
		    {
			    
			    StoreCollisionInfo(collisionInfo);
			    
			    if (hitTimer > hitInterval.Value)
			    {
				    BodyColliderScript bcs = collisionInfo.collider.GetComponent<BodyColliderScript>();
				    if (bcs && rb)
				    {
					    var mass = rb.mass * massMultiplier.Value;
					    int[] indices = new int[] { bcs.index };
					    bcs.ParentRagdollManager.startHitReaction(indices, rb.velocity * mass);
				    }
				    
				    hitTimer = 0.0f;
				    Fsm.Event(hit);
				    
			    }
		    }
	    }
	    
	    public override void DoCollisionStay(Collision collisionInfo)
	    {
		    if (collision == CollisionType.OnCollisionStay)
		    {
			    
			    StoreCollisionInfo(collisionInfo);
			    
			    if (hitTimer > hitInterval.Value)
			    {
				    BodyColliderScript bcs = collisionInfo.collider.GetComponent<BodyColliderScript>();
				    if (bcs && rb)
				    {
					    var mass = rb.mass * massMultiplier.Value;
					    int[] indices = new int[] { bcs.index };
					    bcs.ParentRagdollManager.startHitReaction(indices, rb.velocity * mass);
				    }
				    
				    hitTimer = 0.0f;
				    Fsm.Event(hit);
				    
			    }
		    }
	    }
	    
	    public override void DoParticleCollision(GameObject other)
	    {
		    if (collision == CollisionType.OnParticleCollision)
		    {
			    
		    
			    if (hitTimer > hitInterval.Value)
			    {
				    BodyColliderScript bcs = other.GetComponent<Collider>().GetComponent<BodyColliderScript>();
				    if (bcs && rb)
				    {
					    var mass = rb.mass * massMultiplier.Value;
					    int[] indices = new int[] { bcs.index };
					    bcs.ParentRagdollManager.startHitReaction(indices, rb.velocity * mass);
				    }
				    
				    hitTimer = 0.0f;
				    Fsm.Event(hit);
				    storeForce.Value = 0;
				    storeCollider.Value = other;
				    
			    }
		    }
	    }
	}
}