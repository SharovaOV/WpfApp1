﻿using System;
using System.Collections.Generic;
using WpfApp1.Resources;
using System.Text.Json;

namespace WpfApp1.Models
{
    public class Question
    {
        public string Id { get; set; }
        public TypeQuest Type { get; set; }
        public string Value { get; set; }
        public bool MultyAnswer { get; set; }
        public TypeAnswer TypeAnswer { get; set; }
        public List<Answer> Answers { get; set; }
    }
}