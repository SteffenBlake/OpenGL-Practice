using System;
using System.Collections.Generic;
using OpenTK.Input;

namespace OpenGL_Practice.Services.Interfaces
{
    public interface IInputService
    {
        void HandleKey(Key key, bool pressed);
        void Subscribe(Key key, Action viewEvent);
        void Subscribe(List<Key> keys, Action viewEvent);
        void Unsubscribe(Key key);
        void Unsubscribe(List<Key> keys);
        void Wipe();
    }
}