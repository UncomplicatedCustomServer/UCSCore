namespace UncomplicatedCustomServerCore.Events
{
    internal interface ICustomEventHandler
    {
        public void OnEnabled();

        public void OnDisabled();
    }
}
