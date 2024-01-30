using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;
using VR.Player;

namespace Enemy.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class ZombieAI : MonoBehaviour
    {
        [Header("AI States")]
        [Tooltip("Checks if the AI is within a given range for the AI to chase the player"),
        SerializeField] private Movment MovementState;
        [HideInInspector] public int NumberMovment;

        [Tooltip("Checks if the AI meets the requirements to be destroyed"),
        Space(5), SerializeField] private DestroyedState DestroyState;
        [HideInInspector] public int NumberDestroy;
        
        [Tooltip("Check if thte UI should be hidden depending on the states"),
        Space(5), SerializeField] private HealthBar HealthBarState;
        [HideInInspector] public int NumberHealth;

        [Tooltip("Checks if the AI is ready to attack the player depending on the range"),
        Space(5), SerializeField] private AttackActivation AttackActivationState;
        [HideInInspector] public int NumberAttackActivation;


        /// <summary>
        /// ENUMS FOR STATES
        /// </summary>

        //MOVEMENT STATE
        public enum Movment 
        { 
            Walk, Run 
        };

        //HEALTH BAR UI
        public enum HealthBar 
        { 
            Hidden, Unhidden 
        };

        //DESTROYED CHECK
        public enum DestroyedState 
        { 
            Enable, Disable 
        };

        //ATTACKING STATE
        public enum AttackActivation 
        { 
            Enable, Disable 
        };

        /// <summary>
        /// KEY COMPONENTS
        /// </summary>

        private NavMeshAgent navMesh;
        private Transform player;
        private Transform AICenterPos;
        private Slider healthBarUI;
        private Animator anim;
        
        /// <summary>
        /// AI VARIABLES
        /// </summary>

        [Space(5), Header("AI Stats")]
        [Tooltip("The name of the Player's gameobject")]
        public string playerObjectName = "XR Rig";

        [Range(0f, 200f), Tooltip("The enemy AI's health amount")]
        public float Health;

        [Range(0f, 10f), Tooltip("The strength of ")]
        public float Loudness;

        [Range(0f, 100f), Tooltip("The amount of damage given to a player")]
        public float Damage;
        

        private float readyTime;
        private float lookAtSpeed = 3.0f;
        private float randomDestroy;
        private bool ready;
        private float distanceToPlayer;

        /// <summary>
        /// BOOLEANS CHECK
        /// </summary>

        [HideInInspector] public bool IsMove;
        [HideInInspector] public bool Spawn;
        [HideInInspector] public bool canHit;
        [HideInInspector] public bool canSee;

        #region Start

        private void Start()
        {
            healthBarUI = transform.Find("HealthBar_Canvas/Slider").GetComponent<Slider>();
            AICenterPos = transform.Find("Pos").GetComponent<Transform>();
            navMesh = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();

            player = GameObject.Find(playerObjectName).transform;
            randomDestroy = Random.Range(5.0f, 8.0f);
            canHit = false;

            if (Spawn == false)
            {
                //ATTACKING STATE
                switch (AttackActivationState)
                {
                    case AttackActivation.Enable:
                    {
                        NumberAttackActivation = 0;
                        break;
                    }

                    case AttackActivation.Disable:
                    {
                        NumberAttackActivation = 1;

                        break;
                    }
                }

                //MOEVMENT STATE
                switch (MovementState)
                {
                    case Movment.Walk:
                    {
                        NumberMovment = 0;
                        break;
                    }

                    case Movment.Run:
                    {

                        NumberMovment = 1;
                        break;
                    }
                }

                //HEALTH STATE
                switch (HealthBarState)
                {
                    case HealthBar.Unhidden:
                    {

                        NumberHealth = 0;
                        break;
                    }

                    case HealthBar.Hidden:
                    {

                        NumberHealth = 1;
                        break;
                    }

                }

                //DESTROY STATE
                switch (DestroyState)
                {
                    case DestroyedState.Enable:
                    {

                        NumberDestroy = 0;
                        break;
                    }

                    case DestroyedState.Disable:
                    {

                        NumberDestroy = 1;
                        break;
                    }
                }
            }

            if (NumberAttackActivation == 0)
            {
                canHit = true;
            }

            if (NumberAttackActivation == 1)
            {
                canHit = false;
            }
        }

        #endregion

        #region Update

        private void Update()
        {
            anim.SetLayerWeight(anim.GetLayerIndex("UpperBody"), 1);

            ReadyBoolSwitch();
            HearingRange();
            AttackDistance();
            AIHealth();
            CheckDistance();
            AngleSights();

            //ENUM STATES
            MovementFunc();
            DeathFunc();
            HealthUIFunc();
            MakingNoise();
        }

        #endregion

        #region EnumState Functions

        private void MovementFunc()
        {
            if (NumberMovment == 0)
            {
                if (IsMove == true)
                {
                    navMesh.speed = 1.5f;
                    navMesh.destination = player.position;

                    anim.SetBool("Walk", true);
                }
                if (IsMove == false)
                {
                    anim.SetBool("Walk", false);
                    navMesh.speed = 0;
                }
            }
        }

        private void DeathFunc()
        {
            if (Health <= 0.0f)
            {
                Death();
            }
        }

        private void HealthUIFunc()
        {
            if (NumberHealth == 0 && Health > 0.0f)
            {
                healthBarUI.gameObject.SetActive(true);
            }

            if (NumberHealth == 1 && Health > 0.0f)
            {
                healthBarUI.gameObject.SetActive(false);
            }
        }

        private void MakingNoise()
        {
            MakeNoise ZombieNoise = player.GetComponent<MakeNoise>();
            if (ZombieNoise != null)
            {
                if (ready == true && ZombieNoise.Noise == true)
                {
                    canHit = true;
                }
            }
        }

        #endregion

        private void CheckDistance()
        {
            distanceToPlayer = Vector3.Distance(player.position, transform.position);
        }

        private void AngleSights()
        {
            Vector3 targetDirection = player.position - transform.position;
            float angle = Vector3.Angle(targetDirection, transform.forward);

            if (angle < 45f && distanceToPlayer < 10f && canSee == true)
            {
                canHit = true;
            }
        }

        private void LookAtTarget()
        {
            if (Health > 0.0f)
            {
                var rotation = Quaternion.LookRotation(player.position - transform.position);
                rotation.x = 0; //This is for limiting the rotation to the y axis. I needed this for my project so just
                rotation.z = 0;           //      delete or add the lines you need to have it behave the way you want.
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookAtSpeed);
            }
        }

        private void AttackDistance()
        {
            if (distanceToPlayer < 2 && canHit == true)
            {
                LookAtTarget();
            }
        }

        #region Function States

        private void CheckAttack()
        {

            RaycastHit hit;
            float range = 1.3f;
            Debug.DrawRay(AICenterPos.position, AICenterPos.transform.TransformDirection(Vector3.forward) * range, Color.green);

            if (Physics.Raycast(AICenterPos.position, AICenterPos.transform.TransformDirection(Vector3.forward), out hit, range))
            {

                if (hit.transform.gameObject.name == playerObjectName)
                {
                    anim.SetBool("Attack", true);
                    IsMove = false;
                    lookAtSpeed = 2.0f;
                }
            }
            else
            {
                anim.SetBool("Attack", false);
                if (canHit == true)
                {
                    IsMove = true;
                    lookAtSpeed = 3.0f;
                }

            }
        }

        public void EventAttack()
        {
            RaycastHit hit;
            float range = 1.3f;
            Debug.DrawRay(AICenterPos.position, AICenterPos.transform.TransformDirection(Vector3.forward) * range, Color.red);

            if (Physics.Raycast(AICenterPos.position, AICenterPos.transform.TransformDirection(Vector3.forward), out hit, range))
            {
                if (hit.transform.gameObject.name == playerObjectName)
                {
                    hit.transform.gameObject.SendMessage("ApplyDamage", Damage);
                }
            }
        }

        public void AIHealth()
        {
            healthBarUI.value = Health / 100;

            if (Health > 0.0f)
            {
                CheckAttack();
                SeePlayer();
            }
        }

        private void Death()
        {
            anim.SetBool("Attack", false);
            healthBarUI.gameObject.SetActive(false);
            anim.SetBool("Death", true);
            int RandomDeath = Random.Range(1, 5);
            anim.SetInteger("Death_Int", RandomDeath);
            navMesh.speed = 0.0f;
            this.GetComponent<Collider>().enabled = false;
            if (NumberDestroy == 0)
            {
                StartCoroutine(TimeToDestroy());
            }
        }

        #endregion

        private IEnumerator TimeToDestroy()
        {
            yield return new WaitForSeconds(randomDestroy);
            Destroy(gameObject);
        }

        public void MakeNoise(float Loudness)
        {
            if (distanceToPlayer < Loudness)
            {
                IsMove = true;
            }
        }

        private void HearingRange()
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, Loudness);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.name == playerObjectName)
                {
                    ready = true;
                }
            }
        }

        private void ReadyBoolSwitch()
        {
            if (readyTime > 0.0f)
            {
                readyTime -= Time.deltaTime;
            }
            if (readyTime <= 0.0f)
            {
                ready = false;
                readyTime = 1.0f;
            }
        }

        private void SeePlayer()
        {
            RaycastHit hit;
            float range = 1000f;
            Vector3 fromPosition = AICenterPos.transform.position;
            Vector3 toPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            Vector3 direction = toPosition - fromPosition;

            Debug.DrawRay(AICenterPos.position, direction, Color.cyan);

            if (Physics.Raycast(AICenterPos.position, direction, out hit, range))
            {

                if (hit.transform.gameObject.name == playerObjectName)
                {
                    canSee = true;
                }
                if (hit.transform.gameObject.name != playerObjectName)
                {
                    canSee = false;
                }
            }
        }

        public void EnemyDamage(float Damage)
        {
            Health -= Damage;
            IsMove = true;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Loudness);
        }
    }
}