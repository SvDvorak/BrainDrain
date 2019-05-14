using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Brain
{
    class Entity
    {
        public Vector2 Position => (Parent?.Position ?? Vector2.Zero) + LocalPosition;
        public Vector2 LocalPosition;
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public List<Entity> Children { get; set; } = new List<Entity>();

        public Entity Parent { get; private set; }

        private readonly List<DelayedAction> delayedActions = new List<DelayedAction>();
        protected bool HasQueuedActions => delayedActions.Any();

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.Update(gameTime);
            }

            UpdateAndRemoveActions(gameTime);
        }

        public void Add(Entity e)
        {
            Children.Add(e);
            Visit(c => c.onAdded());
            e.onParentChanged(this);
        }

        public void Remove(Entity e)
        {
            Visit(c => c.onRemoved());
            Children.Remove(e);
            e.onParentChanged(null);
        }

        public void Visit(EntityEvent action)
        {
            action(this);
            foreach (var item in Children)
                item.Visit(action);
        }

        public void Visit<T>(Action<T> action) where T : class
        {
            if (this is T)
            {
                var obj = this as T;
                action(obj);
            }
            foreach (var item in Children)
                item.Visit(action);
        }

        public void Delay(float time, Action action)
        {
            delayedActions.Add(new DelayedAction(time, action));
        }

        protected virtual void onAdded()
        {
        }

        protected virtual void onRemoved()
        {
        }

        protected virtual void onParentChanged(Entity parent)
        {
            this.Parent = parent;
        }

        public T First<T>() where T : class
        {
            T output = null;
            Visit(e =>
            {
                if (output == null && e is T)
                    output = e as T; //TODO: Break
            });
            return output;
        }

        protected List<T> childrenOfType<T>() where T : Entity
        {
            return Children.OfType<T>().ToList();
        }

        public List<T> AllChildrenOfType<T>() where T : Entity
        {
            var children = new List<T>();
            Visit<T>(x =>
            {
                children.Add(x);
            });
            return children;
        }

        public T childOfType<T>() where T : Entity
        {
            foreach (var child in Children)
            {
                if (child is T)
                    return child as T;
            }
            return null;
        }

        private void UpdateAndRemoveActions(GameTime gameTime)
        {
            var finishedActions = new List<DelayedAction>();
            foreach (var delayedAction in delayedActions)
            {
                delayedAction.Time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (delayedAction.Time < 0)
                {
                    finishedActions.Add(delayedAction);
                }
            }

            foreach (var finished in finishedActions)
            {
                finished.Action();
                delayedActions.Remove(finished);
            }
        }

        protected void FinishAllActions()
        {
            foreach (var delayed in delayedActions)
            {
                delayed.Action();
            }

            delayedActions.Clear();
        }

        public delegate void EntityEvent(Entity entity);
    }
}
