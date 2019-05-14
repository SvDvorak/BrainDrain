using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Brain
{
    internal class MousePointerEntity : SpriteEntity
    {
        private Entity carrying;
        private Vector2 carryOffset;
        private ConnectionLine connecting;
        private Entity startConnection;
        private bool IsCarrying => carrying != null;
        private bool IsConnecting => connecting != null;

        public MousePointerEntity()
        {
            Depth = 0.2f;
            SetTexture("hand");

            Add(new MouseFollowBehavior());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var interactable = FindInteractableUnderMouse(ImprovedMouse.Position);

            if (!IsConnecting)
            {
                HandleCarry(interactable);
                HandleDisconnect(interactable);
            }
            if(!IsCarrying)
            {
                HandleConnect(interactable);
            }
        }

        private void HandleCarry(MouseOverInteractable interactable)
        {
            if (ImprovedMouse.DidJustLeftClick)
            {
                if (interactable.Type == InteractableType.Node)
                {
                    carrying = interactable.Entity;
                    carryOffset = carrying.Position - Position;
                }

                SetTexture("handcarry");
            }

            if (ImprovedMouse.DidJustLeftRelease)
            {
                carrying = null;
                SetTexture("hand");
            }

            if (IsCarrying && !GameState.GameOver)
            {
                carrying.LocalPosition = Position + carryOffset;
            }
        }

        private void HandleConnect(MouseOverInteractable interactable)
        {
            if (ImprovedMouse.DidJustRightClick && !GameState.GameOver)
            {
                if (interactable.Type == InteractableType.Node)
                {
                    startConnection = interactable.Entity;
                    if(IsAlive(startConnection))
                    {
                        var connectionLine = new ConnectionLine(startConnection, this);
                        Parent.Add(connectionLine);
                        connecting = connectionLine;
                    }
                }

                SetTexture("handclick");
            }

            if (IsConnecting && ImprovedMouse.DidJustRightRelease)
            {
                if (interactable.Type == InteractableType.Node && CanConnect(connecting, startConnection, interactable.Entity))
                {
                    connecting.Connected(startConnection, interactable.Entity);
                    ConnectionHelper.RealignAllConnections();
                }
                else
                {
                    Parent.Remove(connecting);
                }
                SetTexture("hand");
                connecting = null;
            }
        }

        private void HandleDisconnect(MouseOverInteractable interactable)
        {
            if (interactable.Type != InteractableType.Connection || GameState.GameOver)
                return;

            if (ImprovedMouse.DidJustRightClick)
            {
                SetTexture("handclick");
            }
            (interactable.Entity as ConnectionLine).IsHoveringOver = true;

            if(ImprovedMouse.DidJustRightRelease)
            {
                Parent.Remove(interactable.Entity);
                ConnectionHelper.RealignAllConnections();
                SetTexture("hand");
            }
        }

        private bool CanConnect(ConnectionLine connection, Entity e1, Entity e2)
        {
            var areSame = e1 == e2;
            var twoContracts = e1 is ContractEntity && e2 is ContractEntity;
            var areBothAlive = IsAlive(e1) && IsAlive(e2);
            return !areSame &&
                   !twoContracts &&
                   areBothAlive &&
                   !connection.AlreadyHasConnection(e1, e2);
        }

        private bool IsAlive(Entity entity)
        {
            return !(entity is Interactable interactable) || interactable.IsInteractable;
        }

        public enum InteractableType
        {
            Node,
            Connection,
            None
        }

        public class MouseOverInteractable
        {
            public InteractableType Type;
            public SpriteEntity Entity;
        }

        protected MouseOverInteractable FindInteractableUnderMouse(Point currentMousePoint)
        {
            var collided = GameScene.Instance
                .AllChildrenOfType<BoxBehavior>()
                .FirstOrDefault(x => x
                    .Contains(currentMousePoint));

            if (collided != null)
                return new MouseOverInteractable()
                    {Entity = collided.Parent as SpriteEntity, Type = InteractableType.Node};

            var connections = new List<ConnectionLine>();
            GameScene.Instance.Visit<ConnectionLine>(e =>
            {
                e.IsHoveringOver = e.HasPointOnLine(currentMousePoint);
                if(e.IsHoveringOver)
                    connections.Add(e);
            });

            if (connections.Any())
                return new MouseOverInteractable()
                    {Entity = connections[0], Type = InteractableType.Connection};

            return new MouseOverInteractable() { Type = InteractableType.None };
        }
    }
}
