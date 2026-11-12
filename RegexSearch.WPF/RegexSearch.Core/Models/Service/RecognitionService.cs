using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegexSearch.Core.Models;

namespace RegexSearch.Core.Services
{
    public class RecognitionService
    {
        // АЛГОРИТМ 1: ПОИСК С ПОМОЩЬЮ РЕГУЛЯРНЫХ ВЫРАЖЕНИЙ //
        
        public List<MatchResult> SearchByRegex(string text)
        {
            var results = new List<MatchResult>();

            // Наше регулярное выражение: \b - граница слова, USER_ - текст, \d{5,8} - 5-8 цифр
            var regex = new Regex(@"\bUSER_\d{5,8}\b");
            int number = 1;

            // Ищем все совпадения в тексте
            foreach (Match m in regex.Matches(text))
            {
                results.Add(new MatchResult
                {
                    Number = number,
                    Value = m.Value,
                    Position = m.Index,
                    Length = m.Length
                });
                number++;
            }
            return results;
        }

      
        // АЛГОРИТМ 2: ПОИСК С ПОМОЩЬЮ КОНЕЧНОГО АВТОМАТА
     
        public List<MatchResult> SearchByAutomaton(string text)
        {
            var results = new List<MatchResult>();
            int number = 1;
            int i = 0; // Индекс, с которого начинаем попытку поиска

            // Идем по всему тексту
            while (i < text.Length)
            {
                int j = i;
                State state = State.Start; // Сбрасываем автомат в начало

                // Читаем символы, пока не дойдем до конца или не получим ошибку
                while (j < text.Length && state != State.Error)
                {
                    char c = text[j];
                    state = NextState(state, c); // Переход в новое состояние

                    // Если состояние принимающее (нашли 5, 6, 7 или 8 цифр)
                    if (IsAccepting(state))
                    {
                        // Проверяем, что справа от цифр нет букв или других цифр
                        bool rightBoundary = (j + 1 >= text.Length) || !IsWordChar(text[j + 1]);

                        if (rightBoundary)
                        {
                            // Сохраняем результат
                            results.Add(new MatchResult
                            {
                                Number = number,
                                Value = text.Substring(i, j - i + 1),
                                Position = i,
                                Length = j - i + 1
                            });
                            number++;
                            i = j + 1; // Перепрыгиваем через найденное
                            break;
                        }
                    }
                    j++; // Берем следующий символ
                }

                // Если не нашли (ошибка или конец строки), двигаемся на 1 символ вперед
                if (j >= text.Length || state == State.Error)
                {
                    i++;
                }
            }
            return results;
        }

        // Вспомогательный метод: проверяем, является ли символ частью слова (буква, цифра, _)
        private static bool IsWordChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        // Вспомогательный метод: является ли состояние принимающим
        private static bool IsAccepting(State s)
        {
            return s == State.Digit5 || s == State.Digit6 ||
                   s == State.Digit7 || s == State.Digit8;
        }

        // ГЛАВНАЯ ФУНКЦИЯ ПЕРЕХОДОВ (наша таблица переходов)
        private static State NextState(State state, char c)
        {
            bool digit = c >= '0' && c <= '9'; // Проверяем, цифра ли это

            switch (state)
            {
                case State.Start: return c == 'U' ? State.U : State.Error;
                case State.U: return c == 'S' ? State.US : State.Error;
                case State.US: return c == 'E' ? State.USE : State.Error;
                case State.USE: return c == 'R' ? State.USER : State.Error;
                case State.USER: return c == '_' ? State.Under : State.Error;
                case State.Under: return digit ? State.Digit1 : State.Error;
                case State.Digit1: return digit ? State.Digit2 : State.Error;
                case State.Digit2: return digit ? State.Digit3 : State.Error;
                case State.Digit3: return digit ? State.Digit4 : State.Error;
                case State.Digit4: return digit ? State.Digit5 : State.Error;
                case State.Digit5: return digit ? State.Digit6 : State.Error;
                case State.Digit6: return digit ? State.Digit7 : State.Error;
                case State.Digit7: return digit ? State.Digit8 : State.Error;
                case State.Digit8: return State.Error; // 9-я цифра уже запрещена
                default: return State.Error;
            }
        }
    }
}
