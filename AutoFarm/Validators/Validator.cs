using System.Collections.Generic;
using AutoFarm.Rules;

namespace AutoFarm.Validators
{
    public class Validator
    {
        public object Value { get; set; }
        public List<IRule> RuleList { get; set; }

        public Validator()
        {
            RuleList = new List<IRule>();
        }

        public bool ValidateField()
        {
            bool response = true;
            RuleList.ForEach(r =>
            {
                bool result = r.CheckRule(Value);
                response = response && result;
            });
            return response;
        }
    }
}