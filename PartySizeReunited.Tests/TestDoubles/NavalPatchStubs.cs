namespace PartySizeReunited.Tests.TestDoubles
{
    public class FakeHero
    {
        public bool IsHumanPlayerCharacter { get; set; }
    }

    public class FakeParty
    {
        public FakeHero? LeaderHero { get; set; }
    }
}
