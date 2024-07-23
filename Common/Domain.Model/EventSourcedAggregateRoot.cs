﻿using System;
using System.Collections.Generic;
using FirstPopCoffee.Common.Events;
using ReflectionMagic;

namespace FirstPopCoffee.Common.Domain.Model {
    public abstract class EventSourcedAggregateRoot : Entity {

        private readonly List<Event> _changes = new List<Event>();

        public Guid Id { get; protected set; }
        public int Version { get; internal set; }

        public IEnumerable<Event> GetUncommittedChanges() {
            return _changes;
        }

        public void MarkChangesAsCommitted() {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<Event> history) {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(Event @event) {
            ApplyChange(@event, true);
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(Event @event, bool isNew) {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}
