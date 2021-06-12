
namespace Lazy.StateManagement
{
    public interface IState
    {
        void OnUpdate(float delta);
        void OnEnter();
        void OnExit();
    }
}