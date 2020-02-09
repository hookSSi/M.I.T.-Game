using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Pigeon : Enemy
    {
        protected override void Initialization()
        {
            base.Initialization();
            int rndNum = Random.Range(0, 100);

            if(rndNum < 50)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.4f, 0.0f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if( other.tag == "Player" )
            {
                if(_isAlive)
                {
                    Attack();
                    StartCoroutine(DestoySelf());
                }
            }
        }

        public override void Hitted(Transform collisionObjectTransform)
        {
            if(_isAlive)
            {
                ScoreUpEvent.Trigger(_score);
                Instantiate(_hittedEffect, collisionObjectTransform);
                StartCoroutine(DestoySelf(true, 0.33f));
            }
        }
    }
}
