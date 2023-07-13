using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace AI.Units
{
    public class PlayerComponent : BaseUnit
    {
        private PlayerControls _controls;
		private PlayerCameraController _cameraController;


		private void Awake()
		{
			_controls = new PlayerControls();
		}

		protected override void Start()
		{
			base.Start();
		}

		private void Update()
		{
			OnMove(_controls.Player.Movement.ReadValue<Vector2>().ConvertToMoveVector3());
		}

		protected override void OnMove(Vector3 movement)
		{
			//Во время прочих анимаций пока что нельзя двигаться
			if (InAnimation)
			{
				base.OnMove(Vector3.zero);
				return;//todo
			}
			//Передаем родительскому классу вызов
			base.OnMove(movement);

			//Если : персонаж стоит на месте - ничего не вычисляем
			if (movement == Vector3.zero) return;

			//Движение персонажа
			transform.position += transform.TransformDirection(movement * _propertiesAssistant.GetMobility.MoveSpeed * Time.deltaTime);

			//Вращение персонажа
			transform.rotation = Quaternion.RotateTowards(transform.rotation, _cameraController.transform.rotation, _propertiesAssistant.GetMobility.RotateSpeed);
			transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
		}

		private void OnFastAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			//Пока что в новой системе ввода нет способа исключать двусмысленность чтения ввода
			if (UnityEngine.InputSystem.Keyboard.current[UnityEngine.InputSystem.Key.LeftShift].isPressed) return;

			_currentActionType = ActionType.FastAttack;
			SetAnimationState(_currentActionType);
		}

		private void OnStrongAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			_currentActionType = ActionType.StrongAttack;
			SetAnimationState(_currentActionType);
		}

		[Inject]
		private void Construct(SignalBus signal, Dictionary<UnitType, StatsData> @params)
		{
			_signal = signal;
			_propertiesAssistant = new UnitPropertiesAssistant(@params[_unit]);
			foreach (var element in _elements) element.Construct(this, _signal);
		}
		protected override void OnValidate()
		{
			base.OnValidate();
			_cameraController = FindObjectOfType<PlayerCameraController>();
		}

		private void OnEnable()
		{
			_controls.Player.Enable();
			_controls.Player.FastAttack.performed += OnFastAttack;
			_controls.Player.StrongAttack.performed += OnStrongAttack;
		}

		private void OnDisable()
		{
			_controls.Player.Disable();
			_controls.Player.FastAttack.performed -= OnFastAttack;
			_controls.Player.StrongAttack.performed -= OnStrongAttack;
		}

		private void OnDestroy()
		{
			_controls.Dispose();
		}
	}
}