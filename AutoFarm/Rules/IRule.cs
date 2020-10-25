namespace AutoFarm.Rules
{
	public interface IRule
	{
		bool CheckRule(object value);
	}
}