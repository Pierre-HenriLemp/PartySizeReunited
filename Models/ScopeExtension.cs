using System.ComponentModel;
using System.Reflection;

namespace PartySizeReunited.Models
{
	public class ScopeExtension
	{
		public IScope Scope { get; }

		public ScopeExtension(IScope scope)
		{
			Scope = scope;
		}

		public override string ToString()
		{
			var field = Scope.GetType().GetField(Scope.ToString());
			var attribute = field.GetCustomAttribute<DescriptionAttribute>();
			return attribute?.Description ?? Scope.ToString();
		}
	}

	public enum IScope
	{
		[Description("Everyone (Except player)")]
		Everyone,

		[Description("Only player")]
		Only_player,

		[Description("Only player clan")]
		Only_player_clan,

		[Description("Only player faction")]
		Only_player_faction,

		[Description("Only enemies")]
		Only_ennemies
	}
}
