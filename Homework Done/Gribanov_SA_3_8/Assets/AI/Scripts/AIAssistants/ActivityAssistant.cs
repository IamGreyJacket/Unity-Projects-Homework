using AI.Managers;
using AI.Units;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace AI.Assistants
{
    public class ActivityAssistant
    {
		private IReadOnlyCollection<BotComponent> _enemies;
		
		//[Inject]
		private AIManager _manager;
		private PlayerComponent _player;
		private StateAIAssistant _aiAssistant;
		private ICoroutineDispatcher _dispatcher;

		
		private ActivityAssistant(LinkedList<BotComponent> bots, PlayerComponent player, StateAIAssistant aiAssistant, ICoroutineDispatcher dispatcher)
		{
			_enemies = bots; _player = player; _dispatcher = dispatcher; _aiAssistant = aiAssistant;
		}

		private IEnumerator CheckHealthBot()//todo
		{
			while (true)
			{
				var removeUnits = new LinkedList<BotComponent>();
				foreach (var enemy in _enemies)
				{
					if (enemy.GetProperties.Health <= 0f)
					{
						removeUnits.AddLast(enemy);
					}
				}
				if(removeUnits.Count > 0) _manager.RemoveEnemies(removeUnits);
				yield return new WaitForSeconds(2f);
			}
		}

		public void FindNewTargetForBot(BotComponent bot)
        {
			//_aiAssistant.ClearStateAI(bot);
			List<float> distances = new List<float>();
			foreach(var enemy in _enemies)
            {
				if (bot.SideType == enemy.SideType)
				{
					distances.Add(-1);
					continue;
				}
				var distance = Vector3.SqrMagnitude(bot.transform.position - enemy.transform.position);
				distances.Add(distance);
			}
			var temp = new List<float>(distances);

			temp.Remove(-1);
			while (temp.Contains(-1))
			{
				Debug.LogWarning("-1 is still in List");
				temp.Remove(-1);
			}
			if (temp.Count != 0)
			{
				int indexOfTarget = distances.IndexOf(Mathf.Min(temp.ToArray()));
				var targets = new List<BotComponent>(_enemies);
				bot.Target = new TargetPoint(targets[indexOfTarget].transform);
			}
            else
            {
				Debug.LogWarning("There's no enemies on the field");
            }
			_aiAssistant.UpdateStateAI(bot);
		}

		private IEnumerator CheckActivity(object[] parameters)// float distance, float delay)
		{
			var distanceActivation = (float)parameters[1];
			var delay = (float)parameters[0];
			var distanceFocus = (float)parameters[2];

			distanceActivation *= distanceActivation;
			distanceFocus *= distanceFocus;
			
			while(true)
			{
				foreach (var bot in _enemies)
				{
#if UNITY_EDITOR
					var last = bot.Activity;
#endif
					var distance = Vector3.SqrMagnitude(bot.transform.position - bot.Target.GetPoint);
					bot.Activity = true;

#if UNITY_EDITOR
					if (last != bot.Activity) DebugManager.Log($"{bot.name} : changed its activity mode to '{bot.Activity}'", GetType());
#endif

					if (!bot.Activity) continue;
					if (distance < distanceFocus && !bot.Hunt && bot.Target == bot.SpawnTarget)
					{
						FindNewTargetForBot(bot);

#if UNITY_EDITOR
						DebugManager.Log($"{bot.name} : got close to 'Enemy'", GetType());
#endif
					}
					//Если : игрок близко - бот триггерится
					if (distance < distanceFocus && !bot.Hunt)
					{
						//ИГрок - новая цель бота
						bot.Hunt = true;
						bot.Target = bot.Target;
						//Обновляем поведение бота
						_aiAssistant.UpdateStateAI(bot);
#if UNITY_EDITOR
						DebugManager.Log($"{bot.name} : triggered on 'Enemy'", GetType());
#endif
					}
					//Игрок убежал от бота, а бот взаимодействовал с ним
					if(distance >= distanceFocus & bot.Hunt || bot.Target.GetPoint == Vector3.zero)
					{
						_aiAssistant.ClearStateAI(bot);
						FindNewTargetForBot(bot);
#if UNITY_EDITOR
						DebugManager.Log($"{bot.name} : lost 'Enemy'", GetType());
#endif
					}
					if (distance < distanceFocus / 20 && bot.Hunt)
					{
						Debug.Log($"SqrMagnitude: {distance} | Distance: {Vector3.Distance(bot.transform.position, bot.Target.GetPoint)}");
						//bot.Hunt = true;
						bot.Target = bot.Target;
						bot.SetVelocity(Vector3.zero);
						bot.transform.LookAt(bot.Target.GetPoint);
						_aiAssistant.UpdateStateAI(bot);
					}
				}

				yield return new WaitForSeconds(delay);
			}
		}

		public void Execute(float intervalCheckActivity, float distanceActivation, float distanceFocus)
		{
			//var func = new Func<IEnumerator>(() => CheckActivity(distanceActivation, intervalCheckActivity));
			
			_dispatcher.ExecuteAsync(CheckActivity, new object[] { intervalCheckActivity, distanceActivation, distanceFocus }, this);
		}
    }
}
