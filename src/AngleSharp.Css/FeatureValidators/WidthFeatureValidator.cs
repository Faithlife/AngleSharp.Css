using AngleSharp.Css.Values;

namespace AngleSharp.Css.FeatureValidators
{
    using AngleSharp.Css.Converters;
    using AngleSharp.Css.Dom;
    using System;
    using static ValueConverters;

    sealed class WidthFeatureValidator : IFeatureValidator
    {
        public Boolean Validate(IMediaFeature feature, IRenderDevice renderDevice)
        {
            var length = LengthConverter.Convert(feature.Value);

            // Don't validate units that do not yet support conversion to PX from a renderDevice.
            if (length != null && length is not Length { Type: Length.Unit.Em or Length.Unit.Ex })
            {
                var desired = length.AsPx(renderDevice, RenderMode.Horizontal);
                var available = (Double)renderDevice.ViewPortWidth;

                if (feature.IsMaximum)
                {
                    return available <= desired;
                }
                else if (feature.IsMinimum)
                {
                    return available >= desired;
                }

                return desired == available;
            }

            return false;
        }
    }
}
