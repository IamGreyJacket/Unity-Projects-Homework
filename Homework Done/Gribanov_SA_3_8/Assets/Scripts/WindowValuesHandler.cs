using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace AI 
{
    public class WindowValuesHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private StatsData _zigguratBotStats;

        #region InputFields
        //BaseParam
        [SerializeField, Header("Base Params")]
        private TMP_InputField _maxHealthInput;
        [SerializeField]
        private TMP_InputField _maxManaInput;
        [SerializeField]
        private TMP_InputField _hpRegenPerSecondInput;
        [SerializeField]
        private TMP_InputField _mpRegenPerSecondInput;

        [SerializeField, Header("Mobility Params")]
        private TMP_InputField _moveSpeedInput;
        [SerializeField]
        private TMP_InputField _rotateSpeedInput;
        [SerializeField]
        private TMP_InputField _jumpForceInput;

        [SerializeField, Header("Battle Params")]
        private TMP_InputField _armorInput;
        [SerializeField]
        private TMP_InputField _fastAttackDamageInput;
        [SerializeField]
        private TMP_InputField _strongAttackDamageInput;
        [SerializeField]
        private TMP_InputField _criticalMultiplierInput;

        [SerializeField, Header("Probability Params")]
        private TMP_InputField _criticalChanceInput;
        [SerializeField]
        private TMP_InputField _evadeChanceInput;
        [SerializeField]
        private TMP_InputField _missChanceInput;

        [SerializeField, Header("Ziggurat Params")]
        private TMP_InputField _botToSpawnCountInput;
        [SerializeField]
        private TMP_InputField _botSpawnDelayInput;
        //
        #endregion

        private bool _isOpen = false;
        private BotSpawner _currentZiggurat;

        private IEnumerator OpenMenu()
        {
            if (_isOpen) yield break;
            _isOpen = true;
            float speedMultiplier = 2f;
            var time = 0f;
            var startPos = transform.position;
            var endPos = transform.position;
            endPos.x -= 370;
            while (time < 1f / speedMultiplier)
            {
                transform.position = Vector3.Lerp(startPos, endPos, time * speedMultiplier);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = endPos;
        }

        private IEnumerator CloseMenu()
        {
            if (!_isOpen) yield break;
            float speedMultiplier = 2f;
            var time = 0f;
            var startPos = transform.position;
            var endPos = transform.position;
            endPos.x += 370;
            while (time < 1f / speedMultiplier)
            {
                transform.position = Vector3.Lerp(startPos, endPos, time * speedMultiplier);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = endPos;
            _isOpen = false;
            ClearWindow();
            yield return null;
        }

        public void OnZigguratSelect(BotSpawner selectedZiggurat)
        {
            _currentZiggurat = selectedZiggurat;
            _zigguratBotStats = _currentZiggurat.GetStats;

            _maxHealthInput.text = _zigguratBotStats.BaseParams.MaxHealth.ToString();
            _maxManaInput.text = _zigguratBotStats.BaseParams.MaxMana.ToString();
            _hpRegenPerSecondInput.text = _zigguratBotStats.BaseParams.HPRegenPerSec.ToString();
            _mpRegenPerSecondInput.text = _zigguratBotStats.BaseParams.MPRegenPerSec.ToString();

            _moveSpeedInput.text = _zigguratBotStats.MobilityParams.MoveSpeed.ToString();
            _rotateSpeedInput.text = _zigguratBotStats.MobilityParams.RotateSpeed.ToString();
            _jumpForceInput.text = _zigguratBotStats.MobilityParams.JumpForce.ToString();

            _armorInput.text = _zigguratBotStats.BattleParams.Armor.ToString();
            _fastAttackDamageInput.text = _zigguratBotStats.BattleParams.FastAttackDamage.ToString();
            _strongAttackDamageInput.text = _zigguratBotStats.BattleParams.StrongAttackDamage.ToString();
            _criticalMultiplierInput.text = _zigguratBotStats.BattleParams.CriticalMultiplier.ToString();

            _criticalChanceInput.text = _zigguratBotStats.ProbabilityParams.CriticalChance.ToString();
            _evadeChanceInput.text = _zigguratBotStats.ProbabilityParams.EvadeChance.ToString();
            _missChanceInput.text = _zigguratBotStats.ProbabilityParams.MissChance.ToString();

            _botToSpawnCountInput.text = _currentZiggurat.GetSpawnCount.ToString();
            _botSpawnDelayInput.text = _currentZiggurat.GetSpawnDelay.ToString();
            StartCoroutine(OpenMenu());
        }

        private void ClearWindow()
        {
            _currentZiggurat = null;
            _zigguratBotStats = null;

            _maxHealthInput.text = "";
            _maxManaInput.text = "";
            _hpRegenPerSecondInput.text = "";
            _mpRegenPerSecondInput.text = "";

            _moveSpeedInput.text = "";
            _rotateSpeedInput.text = "";
            _jumpForceInput.text = "";

            _armorInput.text = "";
            _fastAttackDamageInput.text = "";
            _strongAttackDamageInput.text = "";
            _criticalMultiplierInput.text = "";

            _criticalChanceInput.text = "";
            _evadeChanceInput.text = "";
            _missChanceInput.text = "";

            _botToSpawnCountInput.text = "";
            _botSpawnDelayInput.text = "";
        }

        public void OnConfirm()
        {
            if (_currentZiggurat == null) return;
            _zigguratBotStats = new StatsData()
            {
                BaseParams = new BaseParamsData()
                {
                    MaxHealth = Convert.ToSingle(_maxHealthInput.text),
                    MaxMana = Convert.ToSingle(_maxManaInput.text),
                    HPRegenPerSec = Convert.ToSingle(_hpRegenPerSecondInput.text),
                    MPRegenPerSec = Convert.ToSingle(_mpRegenPerSecondInput.text)
                },
                MobilityParams = new MobilityParamsData()
                {
                    MoveSpeed = Convert.ToSingle(_moveSpeedInput.text),
                    RotateSpeed = Convert.ToSingle(_rotateSpeedInput.text),
                    JumpForce = Convert.ToSingle(_jumpForceInput.text)
                },
                BattleParams = new BattleParamsData()
                {
                    Armor = Convert.ToSingle(_armorInput.text),
                    FastAttackDamage = Convert.ToSingle(_fastAttackDamageInput.text),
                    StrongAttackDamage = Convert.ToSingle(_strongAttackDamageInput.text),
                    CriticalMultiplier = Convert.ToSingle(_criticalChanceInput.text)
                },
                ProbabilityParams = new ProbabilityParamsData()
                {
                    CriticalChance = Convert.ToSingle(_criticalChanceInput.text),
                    EvadeChance = Convert.ToSingle(_evadeChanceInput.text),
                    MissChance = Convert.ToSingle(_missChanceInput.text)
                }
            };
            _currentZiggurat.SetZigguratParams(_zigguratBotStats, Convert.ToInt32(_botToSpawnCountInput.text), Convert.ToSingle(_botSpawnDelayInput.text));
            StartCoroutine(CloseMenu());
        }

    }

    //ToDelete
    [System.Serializable]
    public enum StatsType
    {
        MaxHealth,
        MaxMana,
        HPRegenPerSecond,
        MPRegenPerSecond,
        MoveSpeed,
        RotateSpeed,
        JumpForce,
        Armor,
        FastAttackDamage,
        StrongAttackDamage,
        CriticalMultiplier,
        CriticalChance,
        EvadeChance,
        MissChance
    }
}