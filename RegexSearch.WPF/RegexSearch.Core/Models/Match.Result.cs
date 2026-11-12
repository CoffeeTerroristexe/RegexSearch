using System;
using System.Collections.Generic;
using System.Text;

namespace RegexSearch.Core.Models
{
    public class MatchResult
    {
        public int Number { get; set; }      // Номер совпадения (1, 2, 3...)
        public string Value { get; set; }    // Сама найденная строка
        public int Position { get; set; }    // Индекс начала в тексте
        public int Length { get; set; }      // Длина найденной строки
    }
}