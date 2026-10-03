using System;
using System.Collections.Generic;
using System.Globalization;
using BluetoothClientWP8.Model;
class Program
{
    static int checks, failures;
    static void Check(bool ok, string label) { checks++; if (!ok) { failures++; Console.WriteLine("FAIL: " + label); } }
    static void Main()
    {
        var previous = CultureInfo.CurrentCulture;
        try {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var item = new AccelerationItem();
            var names = new List<string>();
            item.PropertyChanged += (sender, args) => names.Add(args.PropertyName);
            item.Message = "1.25 -2.5 3.75";
            Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "valid telemetry");
            Check(names.Contains("X") && names.Contains("Y") && names.Contains("Z") && names.Contains("Message"), "valid notifications");
            names.Clear(); item.Message = "9 broken 8";
            Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "malformed frame preserves entire last sample");
            Check(names.Count == 1 && names[0] == "Message", "malformed frame has no partial axis notifications");
            item.Message = null;
            Check(item.X == 1.25 && item.Message == null, "null frame preserves sample and raw message");
            item.Message = ""; Check(item.Z == 3.75, "empty frame preserves sample");
            item.Message = "9 8"; Check(item.X == 1.25 && item.Y == -2.5, "short frame preserves sample");
            item.Message = "4.5 5.5 6.5"; Check(item.X == 4.5 && item.Y == 5.5 && item.Z == 6.5, "subsequent valid frame recovers");
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            item.Message = "1,25 -2,5 3,75"; Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "existing current-culture numeric contract retained");
            item.Message = "7 8 broken"; Check(item.X == 1.25 && item.Y == -2.5 && item.Z == 3.75, "invalid final axis preserves entire previous sample");
            var hit = new HitItem(); string notified = null;
            hit.PropertyChanged += (sender, args) => notified = args.PropertyName;
            hit.HitValue = 7; Check(hit.HitValue == 7 && notified == "HitValue", "hit value notifies its actual property");
        } finally { CultureInfo.CurrentCulture = previous; }
        Console.WriteLine(checks + " checks; " + failures + " failures");
        Environment.ExitCode = failures == 0 ? 0 : 1;
    }
}
