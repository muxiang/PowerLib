using System;

namespace PowerControl.IrregularControls
{
    [Flags]
    public enum ComputeApplications
    {
        Mike21Ladtap = 1,
        CAirDos = 2,
        HyDrus_1D = 4,
        PavanDose = 8
    }
}
