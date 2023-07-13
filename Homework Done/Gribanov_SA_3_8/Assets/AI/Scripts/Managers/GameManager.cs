using AI.Assistants;
using AI.Configurations;
using AI.Units;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AI.Managers
{
    public class GameManager : MonoInstaller
    {
		private PlayerComponent _player;
		private ICoroutineDispatcher _dispatcher;

        public override void InstallBindings()
        {
			Container.BindInstance(_player).AsSingle();
			//Список всех ботов в сцене
			Container.BindInstance(new LinkedList<BotComponent>()).AsSingle();
			//Конфигурации паттернов ботов
			Container.BindInstance(Resources.LoadAll<AIConfiguration>("AIParams/Configs")).AsSingle();

			Container.Bind<AIManager>().AsSingle();
			Container.Bind<MoveAssistant>().AsSingle();
			Container.Bind<StateAIAssistant>().AsSingle();
			Container.Bind<ActivityAssistant>().AsSingle();

			_dispatcher = gameObject.AddComponent<CoroutineDispatcher>();
			Container.BindInterfacesTo<CoroutineDispatcher>().FromInstance(_dispatcher).AsSingle();

			//Подготовка свойств персонажей
			var unitConfigs = Resources.LoadAll<StatsConfiguration>("UnitProperties");
			var dictionary = new Dictionary<UnitType, StatsData>(unitConfigs.Length);
			foreach (var config in unitConfigs) config.AddPair(dictionary);
			Container.BindInstance(dictionary).AsSingle();

			//Установка системы оповещения текущего контекста
			SignalBusInstaller.Install(Container);
			//Регистрация сигнала с выбрасыванием предупреждений, если нет подписчиков
			Container.DeclareSignal<ActionResultInfo>().WithId("Unit").OptionalSubscriberWithWarning();
			Container.DeclareSignal<ActionResultInfo>().WithId("Die").OptionalSubscriberWithWarning();

#if UNITY_EDITOR
			gameObject.AddComponent<DebugManager>();
#endif
		}

		private void OnValidate()
		{
			_player = FindObjectOfType<PlayerComponent>();
		}
	}
}
