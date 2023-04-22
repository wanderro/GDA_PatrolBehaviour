using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    // Точки, по которым перемещается патруль
    [SerializeField] 
    private List<Transform> _patrolPoints;

    // Задержка до смены точки назначения
    [SerializeField]
    private float _directionChangeDelay;

    // Скорость передвижения
    [SerializeField] 
    private float _speed;

    private int _currentPoint; // Индекс текущей точки
    private float _time = 0f; // Время в пути
    private bool _isWaiting; // Находится ли в паузе объект
    private float _waitTimer = 0f; // Таймер ожидания

    private void Start()
    {
        // Задаём начальную позицию объекту
        transform.position = _patrolPoints[_currentPoint].position;
    }

    void Update()
    {
        // Если не в паузе:
        if (!_isWaiting)
        {
            // Индекс следующей точки
            var nextPoint = (_currentPoint + 1) % _patrolPoints.Count;
            
            // С помощью линейной интерполяции задаём новую позицию объекту
            transform.position = Vector3.Lerp(_patrolPoints[_currentPoint].position, _patrolPoints[nextPoint].position, _time);
            
            // Высчитываем время до перемещения на следующую точку
            _time += Time.deltaTime * _speed / Vector3.Distance(_patrolPoints[_currentPoint].position, _patrolPoints[nextPoint].position);
            
            // Если время = 1 (т.е. мы дошли до точки):
            if (_time >= 1f)
            {
                
                _time = 0f; // Обнуляем время
                _currentPoint = nextPoint; // Новый индекс для текущей точки
                _isWaiting = true; // Запускаем паузу
                _waitTimer = _directionChangeDelay; // Задаём время для таймера
            }
        }
        // Если в паузе:
        else
        {
            // Ждём...
            _waitTimer -= Time.deltaTime;
            // Если время паузы истекло:
            if (_waitTimer <= 0f)
            {
                // Заканчиваем паузу
                _isWaiting = false;
            }
        }

    }
}
