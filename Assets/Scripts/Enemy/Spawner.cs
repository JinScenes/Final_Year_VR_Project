using UnityEngine;

namespace Enemy.AI
{
    public class Spawner : MonoBehaviour
    {

        public Vector3 Size;
        public GameObject[] Zombie;

        /// <summary>
        /// ////////////////////
        /// </summary>

        Movment M;

        ///////////// MovmentState //////////////////////
        public enum Movment { Walk, Run, Random };
        public Movment ZombieMovementState;
        ///////////// HealthBar State //////////////////////
        public enum HealthBar { Hidden, Unhidden, Random };
        public HealthBar ZombieHealthBarState;

        ///////////// Destroy State //////////////////////
        public enum DestroyED { Enable, Disable, Random };
        public DestroyED ZombieDestroyState;

        ///////////// Destroy State //////////////////////
        public enum AttackActivation { Enable, Disable, Random };
        public AttackActivation ZombieAttackActivationState;
        int NumberOfZombieSpawn;
        public int NumberOfZombies = 1;
        float SpawnTime;
        public float Timer = 1.0f;
        public bool RandomHealth;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (SpawnTime > 0.0f)
            {
                SpawnTime -= Time.deltaTime;
            }


            if (SpawnTime <= 0.0f && NumberOfZombieSpawn < NumberOfZombies)
            {
                Spawn();
                NumberOfZombieSpawn++;
                SpawnTime = Timer;
            }

        }


        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 0.5f, 0.3f, 0.7f);
            Gizmos.DrawWireCube(transform.position, Size);
            Gizmos.DrawIcon(transform.position, "SpawnerIcon.png");

        }

        void Spawn()
        {

            Vector3 Pos = transform.position + new Vector3(Random.Range(-Size.x / 2, Size.x / 2), Random.Range(-Size.y / 2, Size.y / 2), Random.Range(-Size.z / 2, Size.z / 2));

            GameObject ZombieSpawn = Instantiate(Zombie[Random.Range(0, Zombie.Length)], Pos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360f), 0.0f));

            //ZombieSpawn.GetComponent<Zombie_CS>().Damage = 10.0f;

            //////////////////////////////////////////////////////////////////////

            ZombieSpawn.GetComponent<ZombieAI>().Spawn = true;




            switch (ZombieMovementState)

            {
                case Movment.Walk:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberMovment = 0;
                        break;
                    }

                case Movment.Run:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberMovment = 1;
                        break;
                    }



                case Movment.Random:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberMovment = Random.Range(0, 2);
                        break;
                    }


            }


            ////////////////////////////////  ZombieAttackActivationState ///////////////////////////////


            switch (ZombieAttackActivationState)

            {
                case AttackActivation.Enable:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberAttackActivation = 0;
                        break;
                    }

                case AttackActivation.Disable:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberAttackActivation = 1;
                        break;
                    }



                case AttackActivation.Random:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberAttackActivation = Random.Range(0, 2);
                        break;
                    }


            }




            ////////////////////////////////  ZombieHealthBarState  ///////////////////////////////


            switch (ZombieHealthBarState)

            {
                case HealthBar.Unhidden:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberHealth = 0;
                        break;
                    }

                case HealthBar.Hidden:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberHealth = 1;
                        break;
                    }



                case HealthBar.Random:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberHealth = Random.Range(0, 2);
                        break;
                    }


            }




            ////////////////////////////////  ZombieDestroyState ///////////////////////////////


            switch (ZombieDestroyState)

            {
                case DestroyED.Enable:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberDestroy = 0;
                        break;
                    }

                case DestroyED.Disable:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberDestroy = 1;
                        break;
                    }



                case DestroyED.Random:

                    {

                        ZombieSpawn.GetComponent<ZombieAI>().NumberDestroy = Random.Range(0, 2);
                        break;
                    }


            }

            if (RandomHealth == true)
            {
                ZombieSpawn.GetComponent<ZombieAI>().Health = Random.Range(5.0f, 100.0f);
            }

        }


    }
}