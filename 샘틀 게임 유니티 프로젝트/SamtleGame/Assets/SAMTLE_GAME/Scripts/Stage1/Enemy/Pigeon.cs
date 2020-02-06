<<<<<<< .merge_file_a01820
﻿public class Pigeon : Enemy
{
    protected override void Initialization()
    {
        base.Initialization();

        _enemySpeed = 2.0f;
=======
﻿using UnityEngine;

namespace MIT.SamtleGame.Stage1
{
    public class Pigeon : Enemy
    {
        protected override void Initialization()
        {
            base.Initialization();

            _enemySpeed = 3f;
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
>>>>>>> .merge_file_a04608
    }
}
