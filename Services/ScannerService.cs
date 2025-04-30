using ScanCRNet.Utility;
using System;
using System.Threading.Tasks;

public class ScannerService
{
    public event EventHandler<ScannedItemPair> OnItemScanned;

    public void Initialize()
    {
        // Setup CSD, sp.OnPageComp += etc
    }

    public async Task StartScanAsync()
    {
        // Call DoScanAsync
    }

    public void Terminate()
    {
        // Call Terminate
    }

    private void OnItem(object sender, OnePageCompletedEventArgs e)
    {
        OnItemScanned?.Invoke(this, e.Pair);
    }
}

