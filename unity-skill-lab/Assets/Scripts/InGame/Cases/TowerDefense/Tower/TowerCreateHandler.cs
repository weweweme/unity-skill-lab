using System;
using InGame.Cases.TowerDefense.System;
using InGame.Cases.TowerDefense.System.Managers;
using UniRx;

namespace InGame.Cases.TowerDefense.Tower
{
    public sealed class TowerCreateHandler : IDisposable
    {
        private readonly TowerDefenseDataManager _dataManager;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public TowerCreateHandler(TowerDefenseManager tdManager)
        {
            _dataManager = tdManager.DataManager;
            
            _dataManager.MainPanel.SelectedTower
                .Subscribe(CreateTower)
                .AddTo(_disposable);
        }

        private void CreateTower(ETowerType type)
        {
            // TODO: TowerPool에게서 GetTower하는 로직 구현
            // TODO: GetTower를 통해 Tower 데이터를 가져와서 활성화시키는 기능 구현
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
