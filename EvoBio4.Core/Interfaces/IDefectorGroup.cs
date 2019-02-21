namespace EvoBio4.Core.Interfaces
{
	public interface IDefectorGroup<TIndividual> : IIndividualGroup<TIndividual>
		where TIndividual : class, IIndividual
	{
	}
}