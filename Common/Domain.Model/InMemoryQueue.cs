﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPopCoffee.Common.Domain.Model {
    public interface IQueue {
        void Put(object item);
        void Pop(Action<object> popAction);
    }

    public class InMemoryQueue : IQueue {
        private readonly Queue<object> _itemQueue;
        private readonly Queue<Action<object>> _listenerQueue;

        public InMemoryQueue() {
            _itemQueue = new Queue<object>(32);
            _listenerQueue = new Queue<Action<object>>(32);
        }

        public void Put(object item) {
            if (_listenerQueue.Count == 0) {
                _itemQueue.Enqueue(item);
                return;
            }

            var listener = _listenerQueue.Dequeue();
            listener(item);
        }

        public void Pop(Action<object> popAction) {
            if (_itemQueue.Count == 0) {
                _listenerQueue.Enqueue(popAction);
                return;
            }

            var item = _itemQueue.Dequeue();
            popAction(item);
        }
    }
}
