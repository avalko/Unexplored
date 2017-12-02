using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game.Structures;
using Unexplored.Game.GameObjects;
using Unexplored.Game.Attributes;
using System;
using Unexplored.Core.Attributes;
using Unexplored.Android.Core.Types;

namespace Unexplored.Game.Components
{

    public class LeverControllerComponent : BehaviorComponent
    {
        private const double AnimationDurationDefault = 75;

        enum LeverDirection
        {
            LEFT,
            RIGHT
        }

        public CameraControllerComponent CurrentCameraController;

        ObjectStateComponent state;
        SpriteAnimatorComponent animator;
        SpriteRendererComponent renderer;
        TriggerControllerComponent trigger;
        LeverDirection currentDirection;
        bool stateChanged, cameraWasMoved, nextState;
        bool waitTimeout;
        double currentTimeout;

        private readonly static SpriteAnimation AnimationLeft = new SpriteAnimation(AnimationDurationDefault, true, 169, 170, 171, 172, 173);
        private readonly static SpriteAnimation AnimationRight = new SpriteAnimation(AnimationDurationDefault, true, 173, 172, 171, 170, 169);

        [CustomProperty]
        bool IsComesBack;
        [CustomProperty]
        float TimeoutComeBack;


        public override void Initialize()
        {
            state = GetComponent<ObjectStateComponent>();
            trigger = GetComponent<TriggerControllerComponent>();
            animator = GetComponent<SpriteAnimatorComponent>();
            renderer = GetComponent<SpriteRendererComponent>();
            currentDirection = LeverDirection.LEFT;            
        }

        public override void Update(GameTime gameTime)
        {
            if (stateChanged && animator.Completed)
            {
                stateChanged = false;
                state.State = nextState;
            }

            if (IsComesBack && animator.Completed && state.State)
            {
                currentTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentTimeout >= TimeoutComeBack)
                    LeverChangeState();
            }
        }

        public override void OnEventStay(GameEvent gameEvent)
        {
            switch (gameEvent.Type)
            {
                case "lever_change_state": LeverChangeState(); break;
                case "camera_go_to":
                    if (nextState && !cameraWasMoved)
                    {
                        cameraWasMoved = true;
                        CurrentCameraController.GoTo(gameEvent.GameObject, 5, 2000);
                    }
                    break;
            }
        }

        private void LeverChangeState()
        {
            if (!animator.Completed)
                return;

            animator.Reset();
            if (currentDirection == LeverDirection.LEFT)
            {
                animator.SetAnimation(AnimationRight);
                currentDirection = LeverDirection.RIGHT;                
            }
            else
            {
                animator.SetAnimation(AnimationLeft);
                currentDirection = LeverDirection.LEFT;
            }

            currentTimeout = 0;
            stateChanged = true;
            cameraWasMoved = false;
            nextState = currentDirection == LeverDirection.RIGHT;
        }
    }
}
