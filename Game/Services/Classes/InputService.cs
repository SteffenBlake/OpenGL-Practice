using System;
using System.Collections.Generic;
using System.Linq;
using OpenGL_Practice.Services.Interfaces;
using OpenTK.Input;

namespace OpenGL_Practice.Services.Classes
{
    public class InputService : IInputService
    {
        private List<Key> _keyList;
        private readonly Dictionary<List<Key>, Action> _subscriptions;

        public InputService()
        {
            _keyList = new List<Key>();
            _subscriptions = new Dictionary<List<Key>, Action>();
        }

        public void HandleKey(Key key, bool pressed)
        {
            if (pressed)
            {
                //ignore repeated triggers while held
                if (_keyList.Contains(key)) return;

                //Add it to list of keys
                _keyList.Add(key);
                //Check subscriptions for match, ordered by biggest combos first
                foreach (var keys in _subscriptions.Keys.OrderByDescending(k => k.Count))
                {
                    if (keys.All(k => _keyList.Contains(k))) _subscriptions[keys].Invoke();
                }
            }
            else
            {
                if (_keyList.Contains(key)) _keyList.Remove(key);
            }
        }

        public void Wipe()
        {
            _keyList = new List<Key>();
        }

        public void Subscribe(Key key, Action viewEvent)
        {
            _subscriptions[new List<Key> { key }] = viewEvent;
        }
        public void Subscribe(List<Key> keys, Action viewEvent)
        {
            _subscriptions[keys] = viewEvent;
        }

        public void Unsubscribe(Key key)
        {
            _subscriptions.Remove(new List<Key> { key });
        }
        public void Unsubscribe(List<Key> keys)
        {
            _subscriptions.Remove(keys);
        }
    }
}
