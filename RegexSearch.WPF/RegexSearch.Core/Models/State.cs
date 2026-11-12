using System;
using System.Collections.Generic;
using System.Text;

namespace RegexSearch.Core.Models
{
    public enum State
    {
        Start,                              // Начальное состояние
        U,                                  // Прочитана буква U
        US,                                 // Прочитано US
        USE,                                // Прочитано USE
        USER,                               // Прочитано USER
        Under,                              // Прочитано USER_
        Digit1, Digit2, Digit3, Digit4,     // Считаем с 1 по 4 цифру
        Digit5, Digit6, Digit7, Digit8,     // Считаем с 5 по 8 цифру (валидные)
        Accept,                             // Успех
        Error                               // Ошибка
    }
}