using CobaltCoreModding.Definitions.ExternalItems;

namespace Weth.External;

public interface IMoreDifficultiesApi
{
	void RegisterAltStarters(Deck deck, StarterDeck starterDeck);
}
