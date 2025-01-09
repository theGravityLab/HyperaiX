namespace HyperaiX.Abstractions.Modules;

public static class FeatureListBuilderExtensions
{
    public const string MARK_USE_BOTS = nameof(MARK_USE_BOTS);
    public const string MARK_USE_UNITS = nameof(MARK_USE_UNITS);

    public static IFeatureListBuilder WithBots(this IFeatureListBuilder self)
    {
        self.SetFeatureMark(MARK_USE_BOTS, true);
        return self;
    }

    public static IFeatureListBuilder WithUnits(this IFeatureListBuilder self)
    {
        self.SetFeatureMark(MARK_USE_UNITS, true);
        return self;
    }
}