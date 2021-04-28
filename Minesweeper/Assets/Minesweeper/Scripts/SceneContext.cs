using UnityEngine;
using Zenject;

public class SceneContext : MonoInstaller
{
    [SerializeField] private MinesweeperManager _minesweepermanager;
    [SerializeField] private GameTimer _gametimer;
    [SerializeField] private SmileButtonStates _smilestates;
    [SerializeField] private Statistics _stats;
    [SerializeField] private PostRequests _postrequests;

    public override void InstallBindings()
    {
        Container.Bind<GridOperations>().AsSingle().NonLazy();
        Container.Bind<MinesweeperManager>().FromInstance(_minesweepermanager);
        Container.Bind<GameTimer>().FromInstance(_gametimer);
        Container.Bind<SmileButtonStates>().FromInstance(_smilestates);
        Container.Bind<Statistics>().FromInstance(_stats);
        Container.Bind<PostRequests>().FromInstance(_postrequests);
    }
}