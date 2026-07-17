using Mercury.Demo.WinForms.v1.Controls;

namespace Mercury.Demo.WinForms.v1;

internal sealed class EnvelopeInspectionControls(MercuryGlassButton envelopeInspectionButton, MercuryGlassButton protectedPayloadButton,
    MercuryGlassButton headerMetadataButton, MercuryGlassButton footerMetadataButton, MercuryGlassPanel envelopeInspectionWorkspace)
{
        public (bool success, Exception? exception) Initialize()
        {
            try
            {




                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }

        public MercuryGlassButton EnvelopeInspectionButton { get; } = envelopeInspectionButton;
        public MercuryGlassButton ProtectedPayloadButton { get; } = protectedPayloadButton;
        public MercuryGlassButton HeaderMetadataButton { get; } = headerMetadataButton;
        public MercuryGlassButton FooterMetadataButton { get; } = footerMetadataButton;

        public MercuryGlassPanel EnvelopeInspectionWorkspace { get; } = envelopeInspectionWorkspace;
        
    }